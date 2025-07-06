using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents candidates for recruitment in the Royal Air Force
    /// </summary>
    public class Candidate
    {
        [Key]
        public Guid CandidateId { get; set; } = Guid.NewGuid();

        public int? CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string NationalIdNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ServiceNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(100)]
        public string Governorate { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone1 { get; set; }

        [StringLength(20)]
        public string? Phone2 { get; set; }

        [StringLength(20)]
        public string? Phone3 { get; set; }

        public int? CurrentRankId { get; set; }

        public int? CurrentAirbaseId { get; set; }

        [StringLength(255)]
        public string? Department { get; set; }

        [StringLength(255)]
        public string? JobTitle { get; set; }

        [StringLength(255)]
        public string? Major { get; set; }

        [StringLength(255)]
        public string? University { get; set; }

        public int? GraduationYear { get; set; }

        [StringLength(50)]
        public string? MarksGrade { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(100)]
        public string CurrentStatus { get; set; } = "جديد";

        // Navigation properties
        public virtual CandidateCategory Category { get; set; } = null!;
        public virtual Rank? CurrentRank { get; set; }
        public virtual Airbase? CurrentAirbase { get; set; }
        
        // TEMPORARY - will be removed after migration
        public virtual CandidateTestScores? TestScores { get; set; }
        
        // New test system navigation properties
        public virtual ICollection<CandidateTestResult> TestResults { get; set; } = new List<CandidateTestResult>();

        // Final evaluation
        public virtual FinalEvaluation? FinalEvaluation { get; set; }
    }
} 