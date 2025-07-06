using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// Represents military airbases in the Royal Air Force
    /// </summary>
    public class Airbase
    {
        [Key]
        public int AirbaseId { get; set; }

        [Required]
        [StringLength(255)]
        public string AirbaseName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
} 