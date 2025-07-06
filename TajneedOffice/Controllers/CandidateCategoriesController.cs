using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using TajneedOffice.Helpers;

namespace TajneedOffice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CandidateCategoriesController : Controller
    {
        private readonly TajneedOfficeDbContext _context;

        public CandidateCategoriesController(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        // GET: CandidateCategories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.CandidateCategories.OrderBy(c => c.CategoryId).ToListAsync();
            return View(categories);
        }

        // GET: CandidateCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var category = await _context.CandidateCategories.FindAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // GET: CandidateCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CandidateCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,Description,CategoryCode")] CandidateCategory category)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate name
                var existsByName = await _context.CandidateCategories.AnyAsync(c => c.CategoryName == category.CategoryName);
                if (existsByName)
                {
                    ModelState.AddModelError("CategoryName", "اسم التصنيف موجود بالفعل");
                    this.AddErrorNotification("اسم التصنيف موجود بالفعل");
                    return View(category);
                }

                // Check for duplicate code
                var existsByCode = await _context.CandidateCategories.AnyAsync(c => c.CategoryCode == category.CategoryCode);
                if (existsByCode)
                {
                    ModelState.AddModelError("CategoryCode", "رمز التصنيف موجود بالفعل");
                    this.AddErrorNotification("رمز التصنيف موجود بالفعل");
                    return View(category);
                }

                _context.CandidateCategories.Add(category);
                await _context.SaveChangesAsync();
                this.AddSuccessNotification("تم إضافة التصنيف بنجاح");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CandidateCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var category = await _context.CandidateCategories.FindAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: CandidateCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,CategoryCode")] CandidateCategory category)
        {
            if (id != category.CategoryId)
                return NotFound();
            if (ModelState.IsValid)
            {
                // Check for duplicate name (excluding current)
                var existsByName = await _context.CandidateCategories.AnyAsync(c => c.CategoryName == category.CategoryName && c.CategoryId != category.CategoryId);
                if (existsByName)
                {
                    ModelState.AddModelError("CategoryName", "اسم التصنيف موجود بالفعل");
                    this.AddErrorNotification("اسم التصنيف موجود بالفعل");
                    return View(category);
                }

                // Check for duplicate code (excluding current)
                var existsByCode = await _context.CandidateCategories.AnyAsync(c => c.CategoryCode == category.CategoryCode && c.CategoryId != category.CategoryId);
                if (existsByCode)
                {
                    ModelState.AddModelError("CategoryCode", "رمز التصنيف موجود بالفعل");
                    this.AddErrorNotification("رمز التصنيف موجود بالفعل");
                    return View(category);
                }

                _context.CandidateCategories.Update(category);
                await _context.SaveChangesAsync();
                this.AddSuccessNotification("تم تعديل التصنيف بنجاح");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CandidateCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var category = await _context.CandidateCategories.FindAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: CandidateCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.CandidateCategories.FindAsync(id);
            if (category != null)
            {
                _context.CandidateCategories.Remove(category);
                await _context.SaveChangesAsync();
                this.AddSuccessNotification("تم حذف التصنيف بنجاح");
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: CandidateCategories/SeedData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                DbInitializer.Initialize(_context);
                this.AddSuccessNotification("تم إدخال بيانات التصنيفات بنجاح");
            }
            catch (Exception ex)
            {
                this.AddErrorNotification("حدث خطأ أثناء إدخال البيانات: " + ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 