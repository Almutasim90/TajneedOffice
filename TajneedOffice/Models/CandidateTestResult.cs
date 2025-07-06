using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents test results for candidates in the new flexible system
    /// </summary>
    public class CandidateTestResult
    {
        [Key]
        public Guid TestResultId { get; set; } = Guid.NewGuid();

        [Required]
        [Display(Name = "المرشح")]
        public Guid CandidateId { get; set; }

        [Required]
        [Display(Name = "نوع الاختبار")]
        public int TestTypeId { get; set; }

        [Display(Name = "الدرجة الرقمية")]
        public decimal? NumericScore { get; set; }

        [StringLength(50)]
        [Display(Name = "النتيجة النصية")]
        public string? TextResult { get; set; } // "لائق", "غير لائق", "ناجح", "راسب"

        [Display(Name = "مجتاز")]
        public bool? IsPassed { get; set; }

        [Required]
        [Display(Name = "تاريخ الاختبار")]
        public DateTime TestDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        [Display(Name = "المقيم")]
        public string? EvaluatorName { get; set; }

        [StringLength(1000)]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Required]
        [Display(Name = "حالة النتيجة")]
        public string Status { get; set; } = "معلق"; // "معلق", "مؤكد", "ملغي"

        [Display(Name = "النتيجة الموزونة")]
        public decimal? WeightedScore { get; set; } // الدرجة بعد تطبيق الوزن

        [Required]
        [Display(Name = "تاريخ الإدخال")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "تاريخ التحديث")]
        public DateTime? UpdatedDate { get; set; }

        [StringLength(100)]
        [Display(Name = "المحدث بواسطة")]
        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual Candidate Candidate { get; set; } = null!;
        public virtual TestType TestType { get; set; } = null!;
    }
} 