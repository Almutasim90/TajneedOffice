using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using Microsoft.AspNetCore.Authorization;

namespace TajneedOffice.Controllers
{
    /// <summary>
    /// Controller for managing test types in the flexible test system
    /// </summary>
    [Authorize]
    public class TestTypesController : Controller
    {
        private readonly TajneedOfficeDbContext _context;

        public TestTypesController(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        // GET: TestTypes
        public async Task<IActionResult> Index()
        {
            var testTypes = await _context.TestTypes
                .Where(t => t.IsActive)
                .OrderBy(t => t.TestName)
                .ToListAsync();
            
            return View(testTypes);
        }

        // GET: TestTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testType = await _context.TestTypes
                .Include(t => t.CategoryTestPaths)
                    .ThenInclude(ctp => ctp.Category)
                .FirstOrDefaultAsync(m => m.TestTypeId == id);

            if (testType == null)
            {
                return NotFound();
            }

            return View(testType);
        }

        // GET: TestTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestName,Description,CriteriaType,MinScore,MaxScore,PassingScore,TestCode")] TestType testType)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate test name
                var existingTest = await _context.TestTypes
                    .FirstOrDefaultAsync(t => t.TestName == testType.TestName && t.IsActive);
                
                if (existingTest != null)
                {
                    ModelState.AddModelError("TestName", "يوجد اختبار بنفس الاسم مسبقاً");
                    return View(testType);
                }

                // Check for duplicate test code if provided
                if (!string.IsNullOrEmpty(testType.TestCode))
                {
                    var existingCode = await _context.TestTypes
                        .FirstOrDefaultAsync(t => t.TestCode == testType.TestCode && t.IsActive);
                    
                    if (existingCode != null)
                    {
                        ModelState.AddModelError("TestCode", "يوجد اختبار بنفس الرمز مسبقاً");
                        return View(testType);
                    }
                }

                testType.CreatedDate = DateTime.Now;
                testType.IsActive = true;
                
                _context.Add(testType);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "تم إنشاء نوع الاختبار بنجاح";
                return RedirectToAction(nameof(Index));
            }
            
            return View(testType);
        }

        // GET: TestTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testType = await _context.TestTypes.FindAsync(id);
            if (testType == null)
            {
                return NotFound();
            }
            
            return View(testType);
        }

        // POST: TestTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestTypeId,TestName,Description,CriteriaType,MinScore,MaxScore,PassingScore,TestCode,IsActive,CreatedDate")] TestType testType)
        {
            if (id != testType.TestTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check for duplicate test name (excluding current record)
                    var existingTest = await _context.TestTypes
                        .FirstOrDefaultAsync(t => t.TestName == testType.TestName && t.TestTypeId != id && t.IsActive);
                    
                    if (existingTest != null)
                    {
                        ModelState.AddModelError("TestName", "يوجد اختبار بنفس الاسم مسبقاً");
                        return View(testType);
                    }

                    // Check for duplicate test code if provided (excluding current record)
                    if (!string.IsNullOrEmpty(testType.TestCode))
                    {
                        var existingCode = await _context.TestTypes
                            .FirstOrDefaultAsync(t => t.TestCode == testType.TestCode && t.TestTypeId != id && t.IsActive);
                        
                        if (existingCode != null)
                        {
                            ModelState.AddModelError("TestCode", "يوجد اختبار بنفس الرمز مسبقاً");
                            return View(testType);
                        }
                    }

                    _context.Update(testType);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "تم تحديث نوع الاختبار بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestTypeExists(testType.TestTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            return View(testType);
        }

        // GET: TestTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testType = await _context.TestTypes
                .Include(t => t.CategoryTestPaths)
                .Include(t => t.CandidateTestResults)
                .FirstOrDefaultAsync(m => m.TestTypeId == id);

            if (testType == null)
            {
                return NotFound();
            }

            // Check if test type is being used
            ViewBag.CanDelete = !testType.CategoryTestPaths.Any() && !testType.CandidateTestResults.Any();
            ViewBag.UsageMessage = testType.CategoryTestPaths.Any() ? "لا يمكن حذف هذا النوع لأنه مستخدم في مسارات التقييم" :
                                  testType.CandidateTestResults.Any() ? "لا يمكن حذف هذا النوع لأنه يحتوي على نتائج اختبارات" : "";

            return View(testType);
        }

        // POST: TestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testType = await _context.TestTypes
                .Include(t => t.CategoryTestPaths)
                .Include(t => t.CandidateTestResults)
                .FirstOrDefaultAsync(t => t.TestTypeId == id);

            if (testType != null)
            {
                // Check if test type is being used
                if (testType.CategoryTestPaths.Any() || testType.CandidateTestResults.Any())
                {
                    TempData["ErrorMessage"] = "لا يمكن حذف نوع الاختبار لأنه مستخدم في النظام";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }

                // Soft delete
                testType.IsActive = false;
                _context.Update(testType);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "تم حذف نوع الاختبار بنجاح";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TestTypeExists(int id)
        {
            return _context.TestTypes.Any(e => e.TestTypeId == id);
        }
    }
} 