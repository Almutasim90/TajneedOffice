using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using Microsoft.AspNetCore.Authorization;

namespace TajneedOffice.Controllers
{
    /// <summary>
    /// Controller for managing candidate test results in the flexible test system
    /// </summary>
    [Authorize]
    public class CandidateTestResultsController : Controller
    {
        private readonly TajneedOfficeDbContext _context;

        public CandidateTestResultsController(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var testResults = await _context.CandidateTestResults
                .Include(c => c.Candidate)
                    .ThenInclude(c => c.Category)
                .Include(c => c.TestType)
                .OrderBy(c => c.Candidate.FullName)
                .ToListAsync();

            return View(testResults);
        }

        public async Task<IActionResult> CandidateTests(Guid? id)
        {
            if (id == null) return NotFound();

            var candidate = await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.TestResults)
                    .ThenInclude(tr => tr.TestType)
                .FirstOrDefaultAsync(c => c.CandidateId == id);

            if (candidate == null) return NotFound();

            ViewBag.Candidate = candidate;
            return View(candidate.TestResults.ToList());
        }

        // GET: CandidateTestResults/Create
        public async Task<IActionResult> Create(Guid? candidateId, int? testTypeId)
        {
            var candidates = await _context.Candidates
                .Include(c => c.Category)
                .Where(c => c.IsActive)
                .OrderBy(c => c.FullName)
                .ToListAsync();

            ViewBag.CandidateId = new SelectList(candidates, "CandidateId", "FullName", candidateId);
            
            var testTypes = await _context.TestTypes
                .Where(t => t.IsActive)
                .OrderBy(t => t.TestName)
                .ToListAsync();
            
            ViewBag.TestTypeId = new SelectList(testTypes, "TestTypeId", "TestName", testTypeId);

            var candidateTestResult = new CandidateTestResult();
            if (candidateId.HasValue)
            {
                candidateTestResult.CandidateId = candidateId.Value;
            }
            if (testTypeId.HasValue)
            {
                candidateTestResult.TestTypeId = testTypeId.Value;
                var testType = await _context.TestTypes.FindAsync(testTypeId.Value);
                ViewBag.TestType = testType;
            }

            return View(candidateTestResult);
        }

        // POST: CandidateTestResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,TestTypeId,NumericScore,TextResult,TestDate,EvaluatorName,Notes")] CandidateTestResult candidateTestResult)
        {
            if (ModelState.IsValid)
            {
                // Check if result already exists for this candidate and test type
                var existingResult = await _context.CandidateTestResults
                    .FirstOrDefaultAsync(c => c.CandidateId == candidateTestResult.CandidateId && 
                                            c.TestTypeId == candidateTestResult.TestTypeId);

                if (existingResult != null)
                {
                    ModelState.AddModelError("", "يوجد نتيجة مسبقة لهذا المرشح في هذا الاختبار");
                }
                else
                {
                    var testType = await _context.TestTypes.FindAsync(candidateTestResult.TestTypeId);
                    if (testType != null)
                    {
                        // Validate score based on test type criteria
                        if (testType.CriteriaType == "درجات")
                        {
                            if (!candidateTestResult.NumericScore.HasValue)
                            {
                                ModelState.AddModelError("NumericScore", "يجب إدخال درجة رقمية لهذا النوع من الاختبارات");
                            }
                            else if (candidateTestResult.NumericScore < testType.MinScore || candidateTestResult.NumericScore > testType.MaxScore)
                            {
                                ModelState.AddModelError("NumericScore", $"الدرجة يجب أن تكون بين {testType.MinScore} و {testType.MaxScore}");
                            }
                            else
                            {
                                // Determine if passed
                                candidateTestResult.IsPassed = candidateTestResult.NumericScore >= testType.PassingScore;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(candidateTestResult.TextResult))
                            {
                                ModelState.AddModelError("TextResult", "يجب إدخال نتيجة نصية لهذا النوع من الاختبارات");
                            }
                            else
                            {
                                // Determine if passed based on text result
                                candidateTestResult.IsPassed = candidateTestResult.TextResult == "لائق" || candidateTestResult.TextResult == "ناجح";
                            }
                        }

                        if (ModelState.IsValid)
                        {
                            // Calculate weighted score
                            var candidate = await _context.Candidates.FindAsync(candidateTestResult.CandidateId);
                            var categoryTestPath = await _context.CategoryTestPaths
                                .FirstOrDefaultAsync(ctp => ctp.CategoryId == candidate.CategoryId && 
                                                          ctp.TestTypeId == candidateTestResult.TestTypeId && ctp.IsActive);

                            if (categoryTestPath != null && candidateTestResult.NumericScore.HasValue)
                            {
                                candidateTestResult.WeightedScore = (candidateTestResult.NumericScore * categoryTestPath.Weight) / 100;
                            }

                            candidateTestResult.Status = "مؤكد";
                            candidateTestResult.CreatedDate = DateTime.Now;
                            
                            _context.Add(candidateTestResult);
                            await _context.SaveChangesAsync();
                            
                            TempData["SuccessMessage"] = "تم إضافة نتيجة الاختبار بنجاح";
                            return RedirectToAction(nameof(CandidateTests), new { id = candidateTestResult.CandidateId });
                        }
                    }
                }
            }

            var candidates = await _context.Candidates
                .Include(c => c.Category)
                .Where(c => c.IsActive)
                .OrderBy(c => c.FullName)
                .ToListAsync();

            ViewBag.CandidateId = new SelectList(candidates, "CandidateId", "FullName", candidateTestResult.CandidateId);
            
            var testTypes = await _context.TestTypes
                .Where(t => t.IsActive)
                .OrderBy(t => t.TestName)
                .ToListAsync();
            
            ViewBag.TestTypeId = new SelectList(testTypes, "TestTypeId", "TestName", candidateTestResult.TestTypeId);

            if (candidateTestResult.TestTypeId != 0)
            {
                var testType = await _context.TestTypes.FindAsync(candidateTestResult.TestTypeId);
                ViewBag.TestType = testType;
            }

            return View(candidateTestResult);
        }

        // GET: CandidateTestResults/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateTestResult = await _context.CandidateTestResults
                .Include(c => c.Candidate)
                .Include(c => c.TestType)
                .FirstOrDefaultAsync(c => c.TestResultId == id);

            if (candidateTestResult == null)
            {
                return NotFound();
            }

            var candidates = await _context.Candidates
                .Include(c => c.Category)
                .Where(c => c.IsActive)
                .OrderBy(c => c.FullName)
                .ToListAsync();

            ViewBag.CandidateId = new SelectList(candidates, "CandidateId", "FullName", candidateTestResult.CandidateId);
            
            var testTypes = await _context.TestTypes
                .Where(t => t.IsActive)
                .OrderBy(t => t.TestName)
                .ToListAsync();
            
            ViewBag.TestTypeId = new SelectList(testTypes, "TestTypeId", "TestName", candidateTestResult.TestTypeId);
            ViewBag.TestType = candidateTestResult.TestType;

            return View(candidateTestResult);
        }

        // POST: CandidateTestResults/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TestResultId,CandidateId,TestTypeId,NumericScore,TextResult,IsPassed,TestDate,EvaluatorName,Notes,Status,WeightedScore,CreatedDate")] CandidateTestResult candidateTestResult)
        {
            if (id != candidateTestResult.TestResultId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var testType = await _context.TestTypes.FindAsync(candidateTestResult.TestTypeId);
                    if (testType != null)
                    {
                        // Validate score based on test type criteria
                        if (testType.CriteriaType == "درجات")
                        {
                            if (!candidateTestResult.NumericScore.HasValue)
                            {
                                ModelState.AddModelError("NumericScore", "يجب إدخال درجة رقمية لهذا النوع من الاختبارات");
                            }
                            else if (candidateTestResult.NumericScore < testType.MinScore || candidateTestResult.NumericScore > testType.MaxScore)
                            {
                                ModelState.AddModelError("NumericScore", $"الدرجة يجب أن تكون بين {testType.MinScore} و {testType.MaxScore}");
                            }
                            else
                            {
                                // Determine if passed
                                candidateTestResult.IsPassed = candidateTestResult.NumericScore >= testType.PassingScore;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(candidateTestResult.TextResult))
                            {
                                ModelState.AddModelError("TextResult", "يجب إدخال نتيجة نصية لهذا النوع من الاختبارات");
                            }
                            else
                            {
                                // Determine if passed based on text result
                                candidateTestResult.IsPassed = candidateTestResult.TextResult == "لائق" || candidateTestResult.TextResult == "ناجح";
                            }
                        }

                        if (ModelState.IsValid)
                        {
                            // Recalculate weighted score
                            var candidate = await _context.Candidates.FindAsync(candidateTestResult.CandidateId);
                            var categoryTestPath = await _context.CategoryTestPaths
                                .FirstOrDefaultAsync(ctp => ctp.CategoryId == candidate.CategoryId && 
                                                          ctp.TestTypeId == candidateTestResult.TestTypeId && ctp.IsActive);

                            if (categoryTestPath != null && candidateTestResult.NumericScore.HasValue)
                            {
                                candidateTestResult.WeightedScore = (candidateTestResult.NumericScore * categoryTestPath.Weight) / 100;
                            }

                            candidateTestResult.UpdatedDate = DateTime.Now;
                            candidateTestResult.UpdatedBy = User.Identity?.Name ?? "نظام";
                            
                            _context.Update(candidateTestResult);
                            await _context.SaveChangesAsync();
                            
                            TempData["SuccessMessage"] = "تم تحديث نتيجة الاختبار بنجاح";
                            return RedirectToAction(nameof(CandidateTests), new { id = candidateTestResult.CandidateId });
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateTestResultExists(candidateTestResult.TestResultId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var candidates = await _context.Candidates
                .Include(c => c.Category)
                .Where(c => c.IsActive)
                .OrderBy(c => c.FullName)
                .ToListAsync();

            ViewBag.CandidateId = new SelectList(candidates, "CandidateId", "FullName", candidateTestResult.CandidateId);
            
            var testTypes = await _context.TestTypes
                .Where(t => t.IsActive)
                .OrderBy(t => t.TestName)
                .ToListAsync();
            
            ViewBag.TestTypeId = new SelectList(testTypes, "TestTypeId", "TestName", candidateTestResult.TestTypeId);
            
            if (candidateTestResult.TestTypeId != 0)
            {
                var testType = await _context.TestTypes.FindAsync(candidateTestResult.TestTypeId);
                ViewBag.TestType = testType;
            }

            return View(candidateTestResult);
        }

        // GET: CandidateTestResults/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateTestResult = await _context.CandidateTestResults
                .Include(c => c.Candidate)
                .Include(c => c.TestType)
                .FirstOrDefaultAsync(m => m.TestResultId == id);

            if (candidateTestResult == null)
            {
                return NotFound();
            }

            return View(candidateTestResult);
        }

        // POST: CandidateTestResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var candidateTestResult = await _context.CandidateTestResults.FindAsync(id);
            if (candidateTestResult != null)
            {
                var candidateId = candidateTestResult.CandidateId;
                _context.CandidateTestResults.Remove(candidateTestResult);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "تم حذف نتيجة الاختبار بنجاح";
                return RedirectToAction(nameof(CandidateTests), new { id = candidateId });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: API endpoint to get test type details
        [HttpGet]
        public async Task<IActionResult> GetTestTypeDetails(int testTypeId)
        {
            var testType = await _context.TestTypes.FindAsync(testTypeId);
            if (testType == null)
            {
                return Json(new { success = false, message = "نوع الاختبار غير موجود" });
            }

            return Json(new
            {
                success = true,
                testType = new
                {
                    testTypeId = testType.TestTypeId,
                    testName = testType.TestName,
                    criteriaType = testType.CriteriaType,
                    minScore = testType.MinScore,
                    maxScore = testType.MaxScore,
                    passingScore = testType.PassingScore
                }
            });
        }

        private bool CandidateTestResultExists(Guid id)
        {
            return _context.CandidateTestResults.Any(e => e.TestResultId == id);
        }
    }
} 