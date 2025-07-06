using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using TajneedOffice.Helpers;

namespace TajneedOffice.Controllers
{
    /// <summary>
    /// Controller for candidate management operations
    /// </summary>
    [Authorize]
    public class CandidatesController : Controller
    {
        private readonly TajneedOfficeDbContext _context;
        private readonly ILogger<CandidatesController> _logger;

        public CandidatesController(TajneedOfficeDbContext context, ILogger<CandidatesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Candidates
        public async Task<IActionResult> Index(string searchTerm, int? categoryId, string? status)
        {
            ViewBag.Categories = await _context.CandidateCategories.OrderBy(c => c.CategoryName).ToListAsync();
            ViewBag.Ranks = await _context.Ranks.OrderBy(r => r.RankId).ToListAsync();
            ViewBag.Airbases = await _context.Airbases.OrderBy(a => a.AirbaseName).ToListAsync();

            var query = _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => 
                    c.FullName.Contains(searchTerm) || 
                    c.NationalIdNumber.Contains(searchTerm) ||
                    c.ServiceNumber!.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(c => c.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(c => c.CurrentStatus == status);
            }

            var candidates = await query.OrderByDescending(c => c.CandidateId).ToListAsync();
            
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedStatus = status;

            return View(candidates);
        }

        // GET: Candidates/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .Include(c => c.TestScores)
                .Include(c => c.TestResults)
                    .ThenInclude(tr => tr.TestType)
                // Additional includes removed temporarily
                .FirstOrDefaultAsync(c => c.CandidateId == id.Value);

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // GET: Candidates/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.CandidateCategories.OrderBy(c => c.CategoryName).ToListAsync();
            var ranks = await _context.Ranks.OrderBy(r => r.RankId).ToListAsync();
            var airbases = await _context.Airbases.OrderBy(a => a.AirbaseName).ToListAsync();

            _logger.LogInformation("Create GET: Categories={CategoryCount}, Ranks={RankCount}, Airbases={AirbaseCount}", 
                categories.Count, ranks.Count, airbases.Count);

            ViewBag.Categories = categories;
            ViewBag.Ranks = ranks;
            ViewBag.Airbases = airbases;

            return View();
        }

        // POST: Candidates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Candidate candidate)
        {
            try
            {
                _logger.LogInformation("=== POST Create Debug ===");
                
                // فحص القيم الخام من الطلب
                var rawCategoryId = Request.Form["CategoryId"].ToString();
                _logger.LogInformation("Raw CategoryId from form: '{RawCategoryId}'", rawCategoryId);
                _logger.LogInformation("CategoryId property: {CategoryId}", candidate.CategoryId);
                _logger.LogInformation("CategoryId HasValue: {HasValue}", candidate.CategoryId.HasValue);
                if (candidate.CategoryId.HasValue)
                    _logger.LogInformation("CategoryId Value: {Value}", candidate.CategoryId.Value);
                
                _logger.LogInformation("ModelState.IsValid: {IsValid}", ModelState.IsValid);
                _logger.LogInformation("ModelState.ErrorCount: {ErrorCount}", ModelState.ErrorCount);
                
                // التحقق من وجود الفئة أولاً
                if (!candidate.CategoryId.HasValue || candidate.CategoryId.Value == 0)
                {
                    ModelState.AddModelError("CategoryId", "يرجى اختيار الفئة");
                    _logger.LogWarning("CategoryId validation failed - no value provided");
                }
                else
                {
                    // التحقق من وجود الفئة في قاعدة البيانات
                    var categoryExists = await _context.CandidateCategories.AnyAsync(c => c.CategoryId == candidate.CategoryId.Value);
                    if (!categoryExists)
                    {
                        ModelState.AddModelError("CategoryId", "الفئة المختارة غير صحيحة");
                        _logger.LogWarning("CategoryId validation failed - category {CategoryId} does not exist", candidate.CategoryId.Value);
                    }
                    else
                    {
                        _logger.LogInformation("CategoryId validation passed - category {CategoryId} exists", candidate.CategoryId.Value);
                    }
                }
                
                // تنظيف ModelState للحقول غير المطلوبة حسب الفئة
                await CleanupModelStateByCategory(candidate.CategoryId);
                
                // التحقق من صحة التاريخ
                if (candidate.DateOfBirth == default(DateTime) || candidate.DateOfBirth > DateTime.Now)
                {
                    ModelState.AddModelError("DateOfBirth", "يرجى إدخال تاريخ ميلاد صحيح");
                }

                // التحقق من الرقم المدني للتأكد من عدم التكرار
                if (!string.IsNullOrEmpty(candidate.NationalIdNumber))
                {
                    var existingCandidate = await _context.Candidates
                        .FirstOrDefaultAsync(c => c.NationalIdNumber == candidate.NationalIdNumber);
                    if (existingCandidate != null)
                    {
                        ModelState.AddModelError("NationalIdNumber", "الرقم المدني موجود مسبقاً");
                    }
                }

                if (ModelState.IsValid)
                {
                    _context.Candidates.Add(candidate);
                    await _context.SaveChangesAsync();
                    this.AddSuccessNotification("تم إنشاء المرشح بنجاح");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // جمع أخطاء التحقق لعرضها
                    var errors = new List<string>();
                    foreach (var modelState in ModelState)
                    {
                        foreach (var error in modelState.Value.Errors)
                        {
                            var errorMessage = $"{modelState.Key}: {error.ErrorMessage}";
                            errors.Add(errorMessage);
                            _logger.LogError("ModelState Error - Field: {Field}, Error: {ErrorMessage}", 
                                modelState.Key, error.ErrorMessage);
                        }
                    }
                    
                    this.AddErrorNotification($"يرجى التحقق من البيانات المدخلة. الأخطاء: {string.Join(", ", errors)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating candidate");
                this.AddErrorNotification($"حدث خطأ أثناء إنشاء المرشح: {ex.Message}");
            }

            ViewBag.Categories = await _context.CandidateCategories.OrderBy(c => c.CategoryName).ToListAsync();
            ViewBag.Ranks = await _context.Ranks.OrderBy(r => r.RankId).ToListAsync();
            ViewBag.Airbases = await _context.Airbases.OrderBy(a => a.AirbaseName).ToListAsync();

            return View(candidate);
        }

        private async Task CleanupModelStateByCategory(int? categoryId)
        {
            if (!categoryId.HasValue || categoryId.Value == 0) return;
            
            // جلب التصنيف لمعرفة نوعه
            var category = await _context.CandidateCategories.FindAsync(categoryId.Value);
            if (category == null) return;

            var categoryCode = category.CategoryCode;

            // إزالة أخطاء الحقول الاختيارية حسب الفئة
            switch (categoryCode)
            {
                case "CAT-PLT": // المرشحيين الطيارين
                    // إزالة أخطاء الحقول العسكرية والتعليمية
                    RemoveModelStateErrors(new[] { "ServiceNumber", "CurrentRankId", "CurrentAirbaseId", 
                        "Department", "JobTitle", "Major", "University", "GraduationYear", "MarksGrade" });
                    break;

                case "CAT-MUG": // المرشحيين الجامعيين العسكريين
                case "CAT-CUG": // المرشحيين الجامعيين المدنيين
                    // إزالة أخطاء الحقول العسكرية فقط
                    RemoveModelStateErrors(new[] { "ServiceNumber", "CurrentRankId", "CurrentAirbaseId", 
                        "Department", "JobTitle" });
                    break;

                case "CAT-LSO": // ضباط الخدمة المحدودة
                case "CAT-NCO": // ضباط الصف (رقباء/عرفاء)
                case "CAT-TCN": // ضباط الصف الكلية التقنية العسكرية
                case "CAT-CNP": // ضباط الصف المدنيين للترفيع
                    // إزالة أخطاء الحقول التعليمية فقط
                    RemoveModelStateErrors(new[] { "Major", "University", "GraduationYear", "MarksGrade" });
                    break;
            }
        }

        private void RemoveModelStateErrors(string[] fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                if (ModelState.ContainsKey(fieldName))
                {
                    ModelState.Remove(fieldName);
                }
            }
        }

        // GET: Candidates/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .Include(c => c.TestScores)
                .Include(c => c.TestResults)
                    .ThenInclude(tr => tr.TestType)
                .FirstOrDefaultAsync(c => c.CandidateId == id.Value);

            if (candidate == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.CandidateCategories.OrderBy(c => c.CategoryName).ToListAsync();
            ViewBag.Ranks = await _context.Ranks.OrderBy(r => r.RankId).ToListAsync();
            ViewBag.Airbases = await _context.Airbases.OrderBy(a => a.AirbaseName).ToListAsync();

            return View(candidate);
        }

        // POST: Candidates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CandidateId,CategoryId,FullName,NationalIdNumber,ServiceNumber,DateOfBirth,Governorate,Phone1,Phone2,Phone3,CurrentRankId,CurrentAirbaseId,Department,JobTitle,Major,University,GraduationYear,MarksGrade,Address,IsActive,CurrentStatus")] Candidate candidate)
        {
            if (id != candidate.CandidateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Candidates.Update(candidate);
                    await _context.SaveChangesAsync();
                    this.AddSuccessNotification("تم تحديث المرشح بنجاح");
                }
                catch (Exception)
                {
                    if (await _context.Candidates.FindAsync(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.CandidateCategories.OrderBy(c => c.CategoryName).ToListAsync();
            ViewBag.Ranks = await _context.Ranks.OrderBy(r => r.RankId).ToListAsync();
            ViewBag.Airbases = await _context.Airbases.OrderBy(a => a.AirbaseName).ToListAsync();

            return View(candidate);
        }

        // GET: Candidates/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .FirstOrDefaultAsync(c => c.CandidateId == id.Value);

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
                this.AddSuccessNotification("تم حذف المرشح بنجاح");
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 