using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TajneedOffice.Data;
using TajneedOffice.Models;

namespace TajneedOffice.Controllers
{
    /// <summary>
    /// Main controller for the application
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TajneedOfficeDbContext _context;

        public HomeController(ILogger<HomeController> logger, TajneedOfficeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get dashboard statistics
            var totalCandidates = await _context.Candidates.CountAsync();
            var activeCandidates = await _context.Candidates.Where(c => c.IsActive).CountAsync();
            var categoryCounts = await _context.Candidates
                .GroupBy(c => c.Category.CategoryName)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.TotalCandidates = totalCandidates;
            ViewBag.ActiveCandidates = activeCandidates;
            ViewBag.CategoryCounts = categoryCounts;

            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            // Get comprehensive dashboard data
            var totalCandidates = await _context.Candidates.CountAsync();
            var activeCandidates = await _context.Candidates.Where(c => c.IsActive).CountAsync();
            
            // Get recent candidates
            var recentCandidates = await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .OrderByDescending(c => c.CandidateId)
                .Take(10)
                .ToListAsync();

            // Get category statistics
            var categoryStats = await _context.Candidates
                .GroupBy(c => c.Category.CategoryName)
                .Select(g => new 
                { 
                    Category = g.Key, 
                    Count = g.Count(),
                    ActiveCount = g.Count(c => c.IsActive)
                })
                .ToListAsync();

            // Get status statistics
            var statusStats = await _context.Candidates
                .GroupBy(c => c.CurrentStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.TotalCandidates = totalCandidates;
            ViewBag.ActiveCandidates = activeCandidates;
            ViewBag.RecentCandidates = recentCandidates;
            ViewBag.CategoryStats = categoryStats;
            ViewBag.StatusStats = statusStats;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
