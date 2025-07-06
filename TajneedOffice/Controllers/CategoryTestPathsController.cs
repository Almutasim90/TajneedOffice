using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using Microsoft.AspNetCore.Authorization;

namespace TajneedOffice.Controllers
{
    /// <summary>
    /// Controller for managing category test paths - defining which tests apply to which categories
    /// </summary>
    [Authorize]
    public class CategoryTestPathsController : Controller
    {
        private readonly TajneedOfficeDbContext _context;

        public CategoryTestPathsController(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        // GET: CategoryTestPaths
        public async Task<IActionResult> Index(int? categoryId)
        {
            var categoryTestPaths = _context.CategoryTestPaths
                .Include(c => c.Category)
                .Include(c => c.TestType)
                .Where(c => c.IsActive)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                categoryTestPaths = categoryTestPaths.Where(c => c.CategoryId == categoryId.Value);
                ViewBag.SelectedCategory = await _context.CandidateCategories.FindAsync(categoryId.Value);
            }

            ViewBag.Categories = new SelectList(
                await _context.CandidateCategories.Where(c => c.CategoryId != 0).ToListAsync(),
                "CategoryId", "CategoryName", categoryId);

            var paths = await categoryTestPaths
                .OrderBy(c => c.Category.CategoryName)
                .ThenBy(c => c.OrderIndex)
                .ToListAsync();

            return View(paths);
        }

        // GET: CategoryTestPaths/CategoryConfiguration/5
        public async Task<IActionResult> CategoryConfiguration(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.CandidateCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryPaths = await _context.CategoryTestPaths
                .Include(c => c.TestType)
                .Where(c => c.CategoryId == id && c.IsActive)
                .OrderBy(c => c.OrderIndex)
                .ToListAsync();

            ViewBag.Category = category;
            ViewBag.TotalWeight = categoryPaths.Sum(c => c.Weight);
            ViewBag.AvailableTests = await _context.TestTypes
                .Where(t => t.IsActive && !categoryPaths.Select(cp => cp.TestTypeId).Contains(t.TestTypeId))
                .ToListAsync();

            return View(categoryPaths);
        }

        // GET: CategoryTestPaths/Create
        public async Task<IActionResult> Create(int? categoryId)
        {
            ViewBag.CategoryId = new SelectList(
                await _context.CandidateCategories.Where(c => c.CategoryId != 0).ToListAsync(),
                "CategoryId", "CategoryName", categoryId);
            
            ViewBag.TestTypeId = new SelectList(
                await _context.TestTypes.Where(t => t.IsActive).ToListAsync(),
                "TestTypeId", "TestName");

            var categoryTestPath = new CategoryTestPath();
            if (categoryId.HasValue)
            {
                categoryTestPath.CategoryId = categoryId.Value;
                // Set next order index
                var maxOrder = await _context.CategoryTestPaths
                    .Where(c => c.CategoryId == categoryId.Value && c.IsActive)
                    .MaxAsync(c => (int?)c.OrderIndex) ?? 0;
                categoryTestPath.OrderIndex = maxOrder + 1;
            }

            return View(categoryTestPath);
        }

        // POST: CategoryTestPaths/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,TestTypeId,Weight,OrderIndex,IsMandatory,Notes")] CategoryTestPath categoryTestPath)
        {
            if (ModelState.IsValid)
            {
                // Check if this test is already assigned to this category
                var existingPath = await _context.CategoryTestPaths
                    .FirstOrDefaultAsync(c => c.CategoryId == categoryTestPath.CategoryId && 
                                            c.TestTypeId == categoryTestPath.TestTypeId && 
                                            c.IsActive);

                if (existingPath != null)
                {
                    ModelState.AddModelError("TestTypeId", "هذا الاختبار مضاف مسبقاً لهذه الفئة");
                }
                else
                {
                    // Check total weight doesn't exceed 100%
                    var currentTotalWeight = await _context.CategoryTestPaths
                        .Where(c => c.CategoryId == categoryTestPath.CategoryId && c.IsActive)
                        .SumAsync(c => c.Weight);

                    if (currentTotalWeight + categoryTestPath.Weight > 100)
                    {
                        ModelState.AddModelError("Weight", $"مجموع الأوزان سيتجاوز 100%. الوزن المتاح: {100 - currentTotalWeight}%");
                    }
                    else
                    {
                        categoryTestPath.CreatedDate = DateTime.Now;
                        categoryTestPath.IsActive = true;
                        
                        _context.Add(categoryTestPath);
                        await _context.SaveChangesAsync();
                        
                        TempData["SuccessMessage"] = "تم إضافة الاختبار للمسار بنجاح";
                        return RedirectToAction(nameof(CategoryConfiguration), new { id = categoryTestPath.CategoryId });
                    }
                }
            }

            ViewBag.CategoryId = new SelectList(
                await _context.CandidateCategories.Where(c => c.CategoryId != 0).ToListAsync(),
                "CategoryId", "CategoryName", categoryTestPath.CategoryId);
            
            ViewBag.TestTypeId = new SelectList(
                await _context.TestTypes.Where(t => t.IsActive).ToListAsync(),
                "TestTypeId", "TestName", categoryTestPath.TestTypeId);

            return View(categoryTestPath);
        }

        // GET: CategoryTestPaths/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryTestPath = await _context.CategoryTestPaths
                .Include(c => c.Category)
                .Include(c => c.TestType)
                .FirstOrDefaultAsync(c => c.PathId == id);

            if (categoryTestPath == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(
                await _context.CandidateCategories.Where(c => c.CategoryId != 0).ToListAsync(),
                "CategoryId", "CategoryName", categoryTestPath.CategoryId);
            
            ViewBag.TestTypeId = new SelectList(
                await _context.TestTypes.Where(t => t.IsActive).ToListAsync(),
                "TestTypeId", "TestName", categoryTestPath.TestTypeId);

            return View(categoryTestPath);
        }

        // POST: CategoryTestPaths/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PathId,CategoryId,TestTypeId,Weight,OrderIndex,IsMandatory,Notes,IsActive,CreatedDate")] CategoryTestPath categoryTestPath)
        {
            if (id != categoryTestPath.PathId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check total weight doesn't exceed 100%
                    var currentTotalWeight = await _context.CategoryTestPaths
                        .Where(c => c.CategoryId == categoryTestPath.CategoryId && c.PathId != id && c.IsActive)
                        .SumAsync(c => c.Weight);

                    if (currentTotalWeight + categoryTestPath.Weight > 100)
                    {
                        ModelState.AddModelError("Weight", $"مجموع الأوزان سيتجاوز 100%. الوزن المتاح: {100 - currentTotalWeight}%");
                    }
                    else
                    {
                        _context.Update(categoryTestPath);
                        await _context.SaveChangesAsync();
                        
                        TempData["SuccessMessage"] = "تم تحديث مسار الاختبار بنجاح";
                        return RedirectToAction(nameof(CategoryConfiguration), new { id = categoryTestPath.CategoryId });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryTestPathExists(categoryTestPath.PathId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.CategoryId = new SelectList(
                await _context.CandidateCategories.Where(c => c.CategoryId != 0).ToListAsync(),
                "CategoryId", "CategoryName", categoryTestPath.CategoryId);
            
            ViewBag.TestTypeId = new SelectList(
                await _context.TestTypes.Where(t => t.IsActive).ToListAsync(),
                "TestTypeId", "TestName", categoryTestPath.TestTypeId);

            return View(categoryTestPath);
        }

        // POST: CategoryTestPaths/UpdateOrder
        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] List<int> pathIds)
        {
            try
            {
                for (int i = 0; i < pathIds.Count; i++)
                {
                    var path = await _context.CategoryTestPaths.FindAsync(pathIds[i]);
                    if (path != null)
                    {
                        path.OrderIndex = i + 1;
                        _context.Update(path);
                    }
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "تم تحديث ترتيب الاختبارات بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "خطأ في تحديث الترتيب: " + ex.Message });
            }
        }

        // GET: CategoryTestPaths/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryTestPath = await _context.CategoryTestPaths
                .Include(c => c.Category)
                .Include(c => c.TestType)
                .FirstOrDefaultAsync(m => m.PathId == id);

            if (categoryTestPath == null)
            {
                return NotFound();
            }

            return View(categoryTestPath);
        }

        // POST: CategoryTestPaths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryTestPath = await _context.CategoryTestPaths.FindAsync(id);
            if (categoryTestPath != null)
            {
                // Soft delete
                categoryTestPath.IsActive = false;
                _context.Update(categoryTestPath);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "تم حذف الاختبار من المسار بنجاح";
                return RedirectToAction(nameof(CategoryConfiguration), new { id = categoryTestPath.CategoryId });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryTestPaths/ValidateWeights/5
        public async Task<IActionResult> ValidateWeights(int categoryId)
        {
            var category = await _context.CandidateCategories.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound();
            }

            var totalWeight = await _context.CategoryTestPaths
                .Where(c => c.CategoryId == categoryId && c.IsActive)
                .SumAsync(c => c.Weight);

            var result = new
            {
                categoryName = category.CategoryName,
                totalWeight = totalWeight,
                isValid = totalWeight == 100,
                message = totalWeight == 100 ? "مجموع الأوزان صحيح (100%)" : 
                         totalWeight > 100 ? $"مجموع الأوزان يتجاوز 100% بقيمة {totalWeight - 100}%" :
                         $"مجموع الأوزان أقل من 100% بقيمة {100 - totalWeight}%"
            };

            return Json(result);
        }

        private bool CategoryTestPathExists(int id)
        {
            return _context.CategoryTestPaths.Any(e => e.PathId == id);
        }
    }
} 