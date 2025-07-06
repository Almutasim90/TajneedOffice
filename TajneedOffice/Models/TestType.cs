using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents different types of tests that can be assigned to candidates
    /// </summary>
    public class TestType
    {
        [Key]
        public int TestTypeId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "اسم الاختبار")]
        public string TestName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "وصف الاختبار")]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "نوع المعيار")]
        public string CriteriaType { get; set; } = string.Empty; // "درجات", "لائق_غير_لائق", "ناجح_راسب"

        [Display(Name = "الحد الأدنى للدرجة")]
        public decimal? MinScore { get; set; }

        [Display(Name = "الحد الأقصى للدرجة")]
        public decimal? MaxScore { get; set; }

        [Display(Name = "درجة النجاح")]
        public decimal? PassingScore { get; set; }

        [Required]
        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        [Display(Name = "رمز الاختبار")]
        public string? TestCode { get; set; }

        // Navigation properties
        public virtual ICollection<CategoryTestPath> CategoryTestPaths { get; set; } = new List<CategoryTestPath>();
        public virtual ICollection<CandidateTestResult> CandidateTestResults { get; set; } = new List<CandidateTestResult>();
    }
} 