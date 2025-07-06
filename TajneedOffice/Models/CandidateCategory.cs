using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents candidate categories for recruitment
    /// </summary>
    public class CandidateCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "اسم التصنيف")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "رمز التصنيف")]
        public string CategoryCode { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
} 