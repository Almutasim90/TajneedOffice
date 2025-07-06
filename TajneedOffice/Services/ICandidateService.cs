using TajneedOffice.Models;

namespace TajneedOffice.Services
{
    /// <summary>
    /// Service interface for candidate management operations
    /// </summary>
    public interface ICandidateService
    {
        Task<IEnumerable<Candidate>> GetAllCandidatesAsync();
        Task<Candidate?> GetCandidateByIdAsync(Guid id);
        Task<Candidate> CreateCandidateAsync(Candidate candidate);
        Task<Candidate> UpdateCandidateAsync(Candidate candidate);
        Task DeleteCandidateAsync(Guid id);
        Task<IEnumerable<Candidate>> SearchCandidatesAsync(string searchTerm, int? categoryId = null, string? status = null);
        Task<IEnumerable<CandidateCategory>> GetAllCategoriesAsync();
        Task<IEnumerable<Rank>> GetAllRanksAsync();
        Task<IEnumerable<Airbase>> GetAllAirbasesAsync();
    }
} 