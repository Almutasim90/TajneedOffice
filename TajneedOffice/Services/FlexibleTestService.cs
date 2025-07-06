using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;

namespace TajneedOffice.Services
{
    /// <summary>
    /// Service implementation for flexible test system operations
    /// </summary>
    public class FlexibleTestService : IFlexibleTestService
    {
        private readonly TajneedOfficeDbContext _context;

        public FlexibleTestService(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestType>> GetActiveTestTypesAsync()
        {
            return await _context.TestTypes
                .Where(t => t.IsActive)
                .OrderBy(t => t.TestName)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryTestPath>> GetCategoryTestPathsAsync(int categoryId)
        {
            return await _context.CategoryTestPaths
                .Include(ctp => ctp.TestType)
                .Where(ctp => ctp.CategoryId == categoryId && ctp.IsActive)
                .OrderBy(ctp => ctp.OrderIndex)
                .ToListAsync();
        }

        public async Task<IEnumerable<CandidateTestResult>> GetCandidateTestResultsAsync(Guid candidateId)
        {
            return await _context.CandidateTestResults
                .Include(ctr => ctr.TestType)
                .Where(ctr => ctr.CandidateId == candidateId)
                .OrderBy(ctr => ctr.TestType.TestName)
                .ToListAsync();
        }

        public async Task<bool> ValidateCategoryWeightsAsync(int categoryId)
        {
            var totalWeight = await _context.CategoryTestPaths
                .Where(ctp => ctp.CategoryId == categoryId && ctp.IsActive)
                .SumAsync(ctp => ctp.Weight);

            return totalWeight == 100;
        }

        public async Task<decimal> CalculateCandidateRecruitmentScoreAsync(Guid candidateId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate == null) return 0;

            var categoryPaths = await GetCategoryTestPathsAsync(candidate.CategoryId ?? 0);
            var testResults = await GetCandidateTestResultsAsync(candidateId);

            decimal totalScore = 0;
            decimal totalPossibleWeight = 0;

            foreach (var path in categoryPaths)
            {
                var result = testResults.FirstOrDefault(tr => tr.TestTypeId == path.TestTypeId && tr.Status == "مؤكد");
                if (result != null)
                {
                    if (result.WeightedScore.HasValue)
                    {
                        totalScore += result.WeightedScore.Value;
                    }
                    else if (result.NumericScore.HasValue)
                    {
                        // Calculate weighted score if not already calculated
                        var weightedScore = (result.NumericScore.Value * path.Weight) / 100;
                        totalScore += weightedScore;

                        // Update the result with calculated weighted score
                        result.WeightedScore = weightedScore;
                        _context.Update(result);
                    }
                }
                totalPossibleWeight += path.Weight;
            }

            await _context.SaveChangesAsync();

            // Return percentage based on completed tests
            return totalPossibleWeight > 0 ? (totalScore * 100) / totalPossibleWeight : 0;
        }

        public async Task<FinalEvaluation> CalculateFinalEvaluationAsync(Guid candidateId)
        {
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.CandidateId == candidateId);

            if (candidate == null) 
                throw new ArgumentException("المرشح غير موجود");

            // Check if final evaluation already exists
            var existingEvaluation = await _context.FinalEvaluations
                .FirstOrDefaultAsync(fe => fe.CandidateId == candidateId);

            var finalEvaluation = existingEvaluation ?? new FinalEvaluation
            {
                CandidateId = candidateId,
                CreatedDate = DateTime.Now
            };

            // Calculate recruitment tests score (from our flexible system)
            finalEvaluation.RecruitmentTestsScore = await CalculateCandidateRecruitmentScoreAsync(candidateId);

            // Get committee evaluation score (temporarily set to 0)
            finalEvaluation.MainCommitteeScore = 0;

            // Professional test score (can be from specific professional tests)
            var professionalTestResults = await _context.CandidateTestResults
                .Include(ctr => ctr.TestType)
                .Where(ctr => ctr.CandidateId == candidateId && 
                            ctr.TestType.TestCode == "PRO" && 
                            ctr.Status == "مؤكد")
                .FirstOrDefaultAsync();

            if (professionalTestResults != null)
            {
                finalEvaluation.ProfessionalTestScore = professionalTestResults.NumericScore ?? 0;
            }

            // Calculate percentages based on the second table weights
            // These percentages should be configurable, but using the example from the image:
            // Recruitment tests: varies by category
            // Committee: varies by category  
            // Professional: varies by category

            // For now, using a sample calculation (this should be configurable)
            decimal recruitmentWeight = 60; // Example weight
            decimal committeeWeight = 30;   // Example weight
            decimal professionalWeight = 10; // Example weight

            finalEvaluation.RecruitmentTestsPercentage = (finalEvaluation.RecruitmentTestsScore * recruitmentWeight) / 100;
            finalEvaluation.MainCommitteePercentage = (finalEvaluation.MainCommitteeScore * committeeWeight) / 100;
            finalEvaluation.ProfessionalTestPercentage = (finalEvaluation.ProfessionalTestScore * professionalWeight) / 100;

            // Calculate final percentage
            finalEvaluation.FinalPercentage = 
                (finalEvaluation.RecruitmentTestsPercentage ?? 0) +
                (finalEvaluation.MainCommitteePercentage ?? 0) +
                (finalEvaluation.ProfessionalTestPercentage ?? 0);

            // Determine final grade
            if (finalEvaluation.FinalPercentage >= 90) finalEvaluation.FinalGrade = "ممتاز";
            else if (finalEvaluation.FinalPercentage >= 80) finalEvaluation.FinalGrade = "جيد جداً";
            else if (finalEvaluation.FinalPercentage >= 70) finalEvaluation.FinalGrade = "جيد";
            else if (finalEvaluation.FinalPercentage >= 60) finalEvaluation.FinalGrade = "مقبول";
            else finalEvaluation.FinalGrade = "راسب";

            // Determine final decision
            if (finalEvaluation.FinalPercentage >= 60)
            {
                finalEvaluation.FinalDecision = "مقبول";
            }
            else
            {
                finalEvaluation.FinalDecision = "مرفوض";
                finalEvaluation.DecisionReason = "عدم تحقيق الحد الأدنى المطلوب";
            }

            finalEvaluation.EvaluationStatus = "مكتمل";
            finalEvaluation.UpdatedDate = DateTime.Now;

            if (existingEvaluation == null)
            {
                _context.FinalEvaluations.Add(finalEvaluation);
            }
            else
            {
                _context.FinalEvaluations.Update(finalEvaluation);
            }

            await _context.SaveChangesAsync();
            return finalEvaluation;
        }

        public async Task<bool> IsCandidateTestingCompleteAsync(Guid candidateId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate == null) return false;

            var requiredTests = await _context.CategoryTestPaths
                .Where(ctp => ctp.CategoryId == candidate.CategoryId && ctp.IsActive && ctp.IsMandatory)
                .CountAsync();

            var completedTests = await _context.CandidateTestResults
                .Where(ctr => ctr.CandidateId == candidateId && ctr.Status == "مؤكد")
                .CountAsync();

            return completedTests >= requiredTests;
        }
    }
} 