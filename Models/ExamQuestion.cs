using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class ExamQuestion
    {
        public int Id { get; set; }

        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        [ForeignKey("Exam")]
        public int ExamID { get; set; }
        public double Points { get; set; }


        public Question Question { get; set; }
        public Exam Exam { get; set; }
    }
}
