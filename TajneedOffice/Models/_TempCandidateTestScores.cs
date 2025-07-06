using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Models
{
    /// <summary>
    /// TEMPORARY MODEL - Will be removed after migration
    /// </summary>
    public class CandidateTestScores
    {
        [Key]
        public Guid TestScoreId { get; set; } = Guid.NewGuid();
        public Guid CandidateId { get; set; }
        public decimal? EnglishScore { get; set; }
        public decimal? MathematicsScore { get; set; }
        public decimal? GeneralCultureScore { get; set; }
        public decimal? ArabicScore { get; set; }
        public decimal? FlightAptitudeScore { get; set; }
        public decimal? DrivingSkillScore { get; set; }
        public decimal? CommunicationSkillScore { get; set; }
        public decimal? ProfessionalEvaluationScore { get; set; }
        public virtual Candidate Candidate { get; set; } = null!;
    }
} 