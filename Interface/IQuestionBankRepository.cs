using Quiz.DTO;
using Quiz.Models;

namespace Quiz.Interface
{
    public interface IQuestionBankRepository:IGenericRepository<QuestionBank>
    {
        public IEnumerable<QuestionBankWithQuestionsDTO> GetAll();
        public QuestionBankWithQuestionsDTO GetById(int Id);
        public QuestionBankWithQuestionsDTO GetByName(string Name);
        public void Update(int id, QuestionBankAddDTO obj);
    }
}
