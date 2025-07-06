using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents the final comprehensive evaluation for a candidate
    /// Combines all test scores, committee evaluation, and professional assessment
    /// </summary>
    public class FinalEvaluation
    {
        [Key]
        public int FinalEvaluationId { get; set; }

        [Required]
        public Guid CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; } = null!;

        // الصفات الشخصية (8 صفات)
        public int AppearanceScore { get; set; } // المظهر
        public string? AppearanceStrength { get; set; }
        public string? AppearanceWeakness { get; set; }

        public int PersonalityScore { get; set; } // الشخصية
        public string? PersonalityStrength { get; set; }
        public string? PersonalityWeakness { get; set; }

        public int IntelligenceScore { get; set; } // الذكاء والقدرة العقلية
        public string? IntelligenceStrength { get; set; }
        public string? IntelligenceWeakness { get; set; }

        public int SocialSkillsScore { get; set; } // الإتصالات والميول الشخصية
        public string? SocialSkillsStrength { get; set; }
        public string? SocialSkillsWeakness { get; set; }

        public int ResponsibilityScore { get; set; } // تحمل المسؤولية
        public string? ResponsibilityStrength { get; set; }
        public string? ResponsibilityWeakness { get; set; }

        public int CommunicationScore { get; set; } // وضوح التعبير
        public string? CommunicationStrength { get; set; }
        public string? CommunicationWeakness { get; set; }

        public int AwarenessScore { get; set; } // الوعي والإدراك
        public string? AwarenessStrength { get; set; }
        public string? AwarenessWeakness { get; set; }

        public int AmbitionScore { get; set; } // الطموح
        public string? AmbitionStrength { get; set; }
        public string? AmbitionWeakness { get; set; }

        // الدرجة النهائية والتقدير
        public int TotalScore { get; set; }
        [StringLength(20)]
        public string? FinalGrade { get; set; } // ممتاز، جيد جدًا، ...

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime EvaluationDate { get; set; } = DateTime.Now;

        [Display(Name = "درجة اختبارات الترشيح")]
        [Range(0, 100)]
        public decimal? RecruitmentTestsScore { get; set; }

        [Display(Name = "نسبة اختبارات الترشيح")]
        [Range(0, 100)]
        public decimal? RecruitmentTestsPercentage { get; set; }

        [Display(Name = "درجة اللجنة الرئيسية")]
        [Range(0, 100)]
        public decimal? MainCommitteeScore { get; set; }

        [Display(Name = "نسبة اللجنة الرئيسية")]
        [Range(0, 100)]
        public decimal? MainCommitteePercentage { get; set; }

        [Display(Name = "درجة اختبار المهنة")]
        [Range(0, 100)]
        public decimal? ProfessionalTestScore { get; set; }

        [Display(Name = "نسبة اختبار المهنة")]
        [Range(0, 100)]
        public decimal? ProfessionalTestPercentage { get; set; }

        [Display(Name = "النسبة النهائية")]
        [Range(0, 100)]
        public decimal? FinalPercentage { get; set; }

        [Display(Name = "التقدير النهائي")]
        [StringLength(50)]
        public string? FinalDecision { get; set; } // "مقبول", "مرفوض", "مؤجل"

        [Display(Name = "سبب القرار")]
        [StringLength(500)]
        public string? DecisionReason { get; set; }

        [Display(Name = "ملاحظات عامة")]
        [StringLength(1000)]
        public string? GeneralNotes { get; set; }

        [Required]
        [Display(Name = "حالة التقييم")]
        public string EvaluationStatus { get; set; } = "قيد المراجعة"; // "قيد المراجعة", "مكتمل", "معتمد"

        [Display(Name = "تاريخ الاعتماد")]
        public DateTime? ApprovalDate { get; set; }

        [StringLength(100)]
        [Display(Name = "معتمد بواسطة")]
        public string? ApprovedBy { get; set; }

        [Required]
        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "تاريخ التحديث")]
        public DateTime? UpdatedDate { get; set; }
    }
} 