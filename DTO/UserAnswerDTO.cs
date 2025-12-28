using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DTO
{
    public class UserAnswerDTO
    {
        public int id { set; get; }
        public int ResultID { get; set; }

        public string UserName { set; get; }
        public string QuestionText { get; set; }

        public int QID { set; get; }
        public string UserAnswerText { get; set; }
        public string? IsCorrect { get; set; }

        public double? point { set; get; }

        
    }
}
