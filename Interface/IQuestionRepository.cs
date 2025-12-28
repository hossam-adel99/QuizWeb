using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz.DTO;
using Quiz.Models;

namespace Quiz.Interface
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        public List<GetBankQuestionDTO> GetBankQuestion(int BankID);
        public List<GetBankQuestionDTO> FilterQuestions(int BankID, int type);
    }
}
