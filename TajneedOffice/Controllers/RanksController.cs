using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using TajneedOffice.Helpers;

namespace TajneedOffice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RanksController : Controller
    {
        private readonly TajneedOfficeDbContext _context;

        public RanksController(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        // GET: Ranks
        public async Task<IActionResult> Index()
        {
            var ranks = await _context.Ranks.OrderBy(r => r.RankId).ToListAsync();
            return View(ranks);
        }

        // GET: Ranks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.RankId == id);
            if (rank == null)
                return NotFound();

            return View(rank);
        }

        // GET: Ranks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ranks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RankName")] Rank rank)
        {
            if (ModelState.IsValid)
            {
                // Check if rank name already exists
                var existingRank = await _context.Ranks
                    .FirstOrDefaultAsync(r => r.RankName == rank.RankName);
                
                if (existingRank != null)
                {
                    ModelState.AddModelError("RankName", "اسم الرتبة موجود بالفعل");
                    this.AddErrorNotification("اسم الرتبة موجود بالفعل");
                    return View(rank);
                }

                _context.Ranks.Add(rank);
                await _context.SaveChangesAsync();
                this.AddSuccessNotification("تم إنشاء الرتبة بنجاح");
                return RedirectToAction(nameof(Index));
            }
            return View(rank);
        }

        // GET: Ranks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var rank = await _context.Ranks.FindAsync(id);
            if (rank == null)
                return NotFound();

            return View(rank);
        }

        // POST: Ranks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RankId,RankName")] Rank rank)
        {
            if (id != rank.RankId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if rank name already exists (excluding current rank)
                    var existingRank = await _context.Ranks
                        .FirstOrDefaultAsync(r => r.RankName == rank.RankName && r.RankId != rank.RankId);
                    
                    if (existingRank != null)
                    {
                        ModelState.AddModelError("RankName", "اسم الرتبة موجود بالفعل");
                        this.AddErrorNotification("اسم الرتبة موجود بالفعل");
                        return View(rank);
                    }

                    _context.Ranks.Update(rank);
                    await _context.SaveChangesAsync();
                    this.AddSuccessNotification("تم تحديث الرتبة بنجاح");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankExists(rank.RankId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rank);
        }

        // GET: Ranks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.RankId == id);
            if (rank == null)
                return NotFound();

            return View(rank);
        }

        // POST: Ranks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rank = await _context.Ranks.FindAsync(id);
            if (rank != null)
            {
                _context.Ranks.Remove(rank);
                await _context.SaveChangesAsync();
                this.AddSuccessNotification("تم حذف الرتبة بنجاح");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RankExists(int id)
        {
            return _context.Ranks.Any(e => e.RankId == id);
        }
    }
} 