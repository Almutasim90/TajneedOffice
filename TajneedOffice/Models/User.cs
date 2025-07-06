using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents users in the Tajneed Office system with military-specific properties
    /// </summary>
    public class User : IdentityUser
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "الرقم العسكري")]
        public string ServiceNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "الرتبة")]
        public int RankId { get; set; }

        [StringLength(100)]
        [Display(Name = "المنصب")]
        public string? Position { get; set; }

        [Required]
        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Rank Rank { get; set; } = null!;
    }
} 