using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents the test path configuration for each candidate category
    /// Defines which tests apply to which category and their weights
    /// </summary>
    public class CategoryTestPath
    {
        [Key]
        public int PathId { get; set; }

        [Required]
        [Display(Name = "فئة المرشح")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "نوع الاختبار")]
        public int TestTypeId { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "الوزن (%)")]
        public decimal Weight { get; set; }

        [Required]
        [Display(Name = "ترتيب الاختبار")]
        public int OrderIndex { get; set; }

        [Required]
        [Display(Name = "إجباري")]
        public bool IsMandatory { get; set; } = true;

        [Required]
        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Required]
        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual CandidateCategory Category { get; set; } = null!;
        public virtual TestType TestType { get; set; } = null!;
    }
} 