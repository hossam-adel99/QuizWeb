using Quiz.DTO;
using Quiz.Models;

namespace Quiz.Interface
{
    public interface IExamResultRepository:IGenericRepository<ExamResult>
    {
        public IEnumerable<ResultDTO> GetAllResultsForExam();

        public IEnumerable<ResultDTO> GetResultByUserId();

        public ResultDTO GetOneResultByUserID(string UserID, int ExamID);

    }
}
