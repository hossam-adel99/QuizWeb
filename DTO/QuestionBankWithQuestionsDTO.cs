using Quiz.Models;

namespace Quiz.DTO
{
    public class QuestionBankWithQuestionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserID { get; set; }
        public string Category { get; set; }
        public bool IsPrivate { get; set; }

        public ICollection<QuestionForQBDTO> Questions { get; set; }
    }
}
