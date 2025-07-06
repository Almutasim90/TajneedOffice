using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;
using TajneedOffice.Helpers;

namespace TajneedOffice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AirbasesController : Controller
    {
        private readonly TajneedOfficeDbContext _context;

        public AirbasesController(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        // GET: Airbases
        public async Task<IActionResult> Index()
        {
            var airbases = await _context.Airbases.OrderBy(a => a.AirbaseId).ToListAsync();
            return View(airbases);
        }

        // GET: Airbases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var airbase = await _context.Airbases.FirstOrDefaultAsync(a => a.AirbaseId == id);
            if (airbase == null)
                return NotFound();

            return View(airbase);
        }

        // GET: Airbases/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airbases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AirbaseName")] Airbase airbase)
        {
            if (ModelState.IsValid)
            {
                // Check if airbase name already exists
                var existingAirbase = await _context.Airbases
                    .FirstOrDefaultAsync(a => a.AirbaseName == airbase.AirbaseName);
                
                if (existingAirbase != null)
                {
                    ModelState.AddModelError("AirbaseName", "اسم القاعدة الجوية موجود بالفعل");
                    this.AddErrorNotification("اسم القاعدة الجوية موجود بالفعل");
                    return View(airbase);
                }

                _context.Airbases.Add(airbase);
                await _context.SaveChangesAsync();
                this.AddSuccessNotification("تم إنشاء القاعدة الجوية بنجاح");
                return RedirectToAction(nameof(Index));
            }
            return View(airbase);
        }

        // GET: Airbases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var airbase = await _context.Airbases.FindAsync(id);
            if (airbase == null)
                return NotFound();

            return View(airbase);
        }

        // POST: Airbases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AirbaseId,AirbaseName")] Airbase airbase)
        {
            if (id != airbase.AirbaseId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if airbase name already exists (excluding current airbase)
                    var existingAirbase = await _context.Airbases
                        .FirstOrDefaultAsync(a => a.AirbaseName == airbase.AirbaseName && a.AirbaseId != airbase.AirbaseId);
                    
                    if (existingAirbase != null)
                    {
                        ModelState.AddModelError("AirbaseName", "اسم القاعدة الجوية موجود بالفعل");
                        this.AddErrorNotification("اسم القاعدة الجوية موجود بالفعل");
                        return View(airbase);
                    }

                    _context.Airbases.Update(airbase);
                    await _context.SaveChangesAsync();
                    this.AddSuccessNotification("تم تحديث القاعدة الجوية بنجاح");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirbaseExists(airbase.AirbaseId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(airbase);
        }

        // GET: Airbases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var airbase = await _context.Airbases.FirstOrDefaultAsync(a => a.AirbaseId == id);
            if (airbase == null)
                return NotFound();

            return View(airbase);
        }

        // POST: Airbases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airbase = await _context.Airbases.FindAsync(id);
            if (airbase != null)
            {
                _context.Airbases.Remove(airbase);
                await _context.SaveChangesAsync();
                this.AddSuccessNotification("تم حذف القاعدة الجوية بنجاح");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AirbaseExists(int id)
        {
            return _context.Airbases.Any(e => e.AirbaseId == id);
        }
    }
} 