using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.ViewModels
{
    /// <summary>
    /// View model for user login
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "الرقم العسكري مطلوب")]
        [Display(Name = "الرقم العسكري")]
        public string ServiceNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تذكرني")]
        public bool RememberMe { get; set; }
    }
} 