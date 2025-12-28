using Quiz.DTO;
using Quiz.Models;

namespace Quiz.Interface
{
    public interface IUserAnswerRepository:IGenericRepository<UserAnswer>
    {
        public List<UserAnswerDTO> GetAnswersForResult(int ResultID);
    }
}
