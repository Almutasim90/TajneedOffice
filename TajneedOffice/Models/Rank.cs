using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents military ranks in the Royal Air Force
    /// </summary>
    public class Rank
    {
        [Key]
        public int RankId { get; set; }

        [Required]
        [StringLength(100)]
        public string RankName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? RankCode { get; set; }

        public int RankOrder { get; set; }

        // Navigation properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
} 