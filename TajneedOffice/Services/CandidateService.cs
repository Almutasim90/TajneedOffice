using Microsoft.EntityFrameworkCore;
using TajneedOffice.Data;
using TajneedOffice.Models;

namespace TajneedOffice.Services
{
    /// <summary>
    /// Service implementation for candidate management operations
    /// </summary>
    public class CandidateService : ICandidateService
    {
        private readonly TajneedOfficeDbContext _context;

        public CandidateService(TajneedOfficeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Candidate>> GetAllCandidatesAsync()
        {
            return await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .Include(c => c.TestScores)
                .Include(c => c.TestResults)
                    .ThenInclude(tr => tr.TestType)
                .OrderByDescending(c => c.CandidateId)
                .ToListAsync();
        }

        public async Task<Candidate?> GetCandidateByIdAsync(Guid id)
        {
            return await _context.Candidates
                .Include(c => c.Category)
                .Include(c => c.CurrentRank)
                .Include(c => c.CurrentAirbase)
                .Include(c => c.TestScores)
                .Include(c => c.TestResults)
                    .ThenInclude(tr => tr.TestType)
                .FirstOrDefaultAsync(c => c.CandidateId == id);
        }

        public async Task<Candidate> CreateCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task<Candidate> UpdateCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync();
            return candidate;
        }

        public async Task DeleteCandidateAsync(Guid id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Candidate>> SearchCandidatesAsync(string searchTerm, int? categoryId = null, string? status = null)
        {
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

            return await query.OrderByDescending(c => c.CandidateId).ToListAsync();
        }

        public async Task<IEnumerable<CandidateCategory>> GetAllCategoriesAsync()
        {
            return await _context.CandidateCategories.OrderBy(c => c.CategoryName).ToListAsync();
        }

        public async Task<IEnumerable<Rank>> GetAllRanksAsync()
        {
            return await _context.Ranks.OrderBy(r => r.RankOrder).ToListAsync();
        }

        public async Task<IEnumerable<Airbase>> GetAllAirbasesAsync()
        {
            return await _context.Airbases.Where(a => a.IsActive).OrderBy(a => a.AirbaseName).ToListAsync();
        }
    }
} 