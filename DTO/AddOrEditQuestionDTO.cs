using System.ComponentModel.DataAnnotations;
using Quiz.Models;

namespace Quiz.DTO
{
    public class AddOrEditQuestionDTO
    {
        [Required(ErrorMessage = "Question text is required")]
        public string QstText { get; set; }

        [Range(0,2,ErrorMessage = "Question type is required")]
        public int QuestionType { get; set; }

        [Required(ErrorMessage = "Correct Answer is required")]
        public string CorAnswer { get; set; }
        public int BankID { get; set; }

        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
    }
}
