using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DTO
{
    public class QuestionForQBDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType? Type { get; set; }

        public string? CorrectAnswer { get; set; }
    }
}
