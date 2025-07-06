using TajneedOffice.Models;

namespace TajneedOffice.Services
{
    /// <summary>
    /// Interface for flexible test system services
    /// </summary>
    public interface IFlexibleTestService
    {
        Task<IEnumerable<TestType>> GetActiveTestTypesAsync();
        Task<IEnumerable<CategoryTestPath>> GetCategoryTestPathsAsync(int categoryId);
        Task<IEnumerable<CandidateTestResult>> GetCandidateTestResultsAsync(Guid candidateId);
        Task<bool> ValidateCategoryWeightsAsync(int categoryId);
        Task<decimal> CalculateCandidateRecruitmentScoreAsync(Guid candidateId);
        Task<FinalEvaluation> CalculateFinalEvaluationAsync(Guid candidateId);
        Task<bool> IsCandidateTestingCompleteAsync(Guid candidateId);
    }
} 