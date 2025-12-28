using Quiz.Models;

namespace Quiz.DTO
{
    public class GetBankQuestionDTO
    {
        public int QstID { get; set; }
        public string QstText { get; set; }
        public string QuestionType { get; set; }
        public string CorAnswer { get; set; }

        public List<string>? options { get; set; }

    }
}
