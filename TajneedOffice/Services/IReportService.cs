using TajneedOffice.Models;

namespace TajneedOffice.Services
{
    /// <summary>
    /// Service interface for report generation operations
    /// </summary>
    public interface IReportService
    {
        Task<byte[]> GenerateCandidateReportAsync(Guid candidateId);
        Task<byte[]> GenerateCommitteeSummaryReportAsync(Guid candidateId);
        Task<byte[]> GenerateFinalResultsReportAsync(int? categoryId = null, bool? isRecommended = null);
        Task<byte[]> GenerateStatisticalReportAsync(DateTime? fromDate = null, DateTime? toDate = null);
        Task<IEnumerable<Candidate>> GetCandidatesForReportAsync(int? categoryId = null, bool? isRecommended = null);
    }
} 