using TajneedOffice.Models;

namespace TajneedOffice.Services
{
    /// <summary>
    /// Service interface for candidate import operations
    /// </summary>
    public interface ICandidateImportService
    {
        Task<ImportResult> ImportCandidatesFromExcelAsync(Stream fileStream, int categoryId);
        Task<byte[]> ExportCandidatesToExcelAsync(IEnumerable<Candidate> candidates);
        Task<byte[]> GetExcelTemplateAsync(int categoryId);
    }

    /// <summary>
    /// Result of import operation
    /// </summary>
    public class ImportResult
    {
        public bool Success { get; set; }
        public int ImportedCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; } = string.Empty;
    }
} 