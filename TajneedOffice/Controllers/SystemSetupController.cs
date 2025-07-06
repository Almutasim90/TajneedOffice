using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TajneedOffice.Data;

namespace TajneedOffice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemSetupController : Controller
    {
        private readonly TajneedOfficeDbContext _context;

        public SystemSetupController(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        // GET: SystemSetup
        public IActionResult Index()
        {
            ViewBag.TestTypesCount = _context.TestTypes.Count();
            ViewBag.CategoriesCount = _context.CandidateCategories.Count();
            ViewBag.CategoryTestPathsCount = _context.CategoryTestPaths.Count();
            ViewBag.RanksCount = _context.Ranks.Count();
            ViewBag.AirbasesCount = _context.Airbases.Count();

            return View();
        }

        // POST: SystemSetup/InitializeData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InitializeData()
        {
            try
            {
                DbInitializer.Initialize(_context);
                TempData["SuccessMessage"] = "تم إضافة البيانات الأساسية بنجاح!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"حدث خطأ: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: SystemSetup/ClearTestData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearTestData()
        {
            try
            {
                // Clear test data in reverse order of dependencies
                _context.CategoryTestPaths.RemoveRange(_context.CategoryTestPaths);
                _context.TestTypes.RemoveRange(_context.TestTypes);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "تم حذف بيانات الاختبارات بنجاح!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"حدث خطأ: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 