using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Quiz.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        [ForeignKey("Exam")]
        public int ExamID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        public double? Score { get; set; }
        public DateTime Date { get; set; }

        public string? FeedBack { set; get; }

        [JsonIgnore]
        public Exam Exam { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<UserAnswer> UsersAnswers { get; set; }
    }
}
