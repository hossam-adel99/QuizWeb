using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Quiz.DTO;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Repository
{
    public class UserAnswerRepository:GenericRepository<UserAnswer> , IUserAnswerRepository
    {
        private readonly QuizContext quizContext;

        public UserAnswerRepository(QuizContext quizContext) : base(quizContext)
        {
            this.quizContext = quizContext;
        }

        public List<UserAnswerDTO> GetAnswersForResult(int ResultID)
        {
            List<UserAnswerDTO> userAnswers = quizContext.UserAnswers.Include(e=>e.Question).Include(e=>e.Result).Where(e => e.ResultID == ResultID).Select(
                e=> new UserAnswerDTO
                {
                   id = e.Id,
                   ResultID = e.ResultID,
                   IsCorrect = e.IsCorrect,
                   UserAnswerText = e.UserAnswerText,
                   QuestionText = e.Question.Text,
                   UserName = e.Result.User.UserName,
                   point =e.point,
                   QID = e.QuestionID
                  
                }).ToList();

            return userAnswers;
        }

        #region Get By ID Inculde
        public UserAnswer GetById(int id)
        {
            return quizContext.UserAnswers
                           .Include(r => r.Result)
                           .ThenInclude(e => e.Exam)
                           .FirstOrDefault(r => r.Id == id);
        }

        #endregion
    }
}
