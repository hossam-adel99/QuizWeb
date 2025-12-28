using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [ForeignKey("Creator")]
        public string CreatedBy { get; set; }

        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public ApplicationUser Creator { get; set; }

        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public ICollection<ExamResult> ExamResults { get; set; }
    }
}
