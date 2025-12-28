using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Quiz.DTO;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Repository
{
    public class ExamResultRepository :GenericRepository<ExamResult> ,IExamResultRepository
    {
        private readonly QuizContext quizContext;

        public ExamResultRepository(QuizContext quizContext) : base(quizContext)
        {
            this.quizContext = quizContext;
        }

        #region GetAllResultsForExam 
        public IEnumerable<ResultDTO> GetAllResultsForExam()
        {
            List<ResultDTO> results = quizContext.ExamResults.Include(e=>e.Exam).Include(e => e.User).Include(e=>e.UsersAnswers).Select(
                e=> new ResultDTO
                {
                    ExamID = e.ExamID,
                    ExamName = e.Exam.Title,
                    UseName = e.User.UserName,
                    userid = e.UserID,
                    date = e.Date,
                    score = e.Score,
                    FeedBack = e.FeedBack,
                    CreatorID = e.Exam.CreatedBy,
                    userAnswers = e.UsersAnswers
                   
                    
                }).ToList();
        
            return results;
        }

        #endregion

        #region Get Result For Exam To User
        public IEnumerable<ResultDTO> GetResultByUserId()
        {
           List<ResultDTO>  results= quizContext.ExamResults.Include(e=> e.Exam).Include(e=>e.User).Include(e=>e.UsersAnswers).Select(e=> new ResultDTO
           {
               CreatorID = e.Exam.CreatedBy,
               ExamID = e.ExamID,
               ExamName = e.Exam.Title,
               date = e.Date,
               FeedBack =e.FeedBack,
               userid = e.UserID,
               UseName = e.User.UserName,
               score = e.Score,
               userAnswers = e.UsersAnswers
               

           }).ToList();

            return results;
           
        }


        #endregion

        #region Get one Result to User
        public ResultDTO GetOneResultByUserID(string UserID , int ExamID)
        {
            ResultDTO result = quizContext.ExamResults.Include(e => e.Exam).Include(e => e.User).Include(e=>e.UsersAnswers).Select(i => new ResultDTO
            {
                CreatorID = i.Exam.CreatedBy,
                ExamID = i.ExamID,
                ExamName = i.Exam.Title,
                date = i.Date,
                FeedBack = i.FeedBack,
                userid = i.UserID,
                UseName = i.User.UserName,
                score = i.Score,
                userAnswers = i.UsersAnswers
               
            }).FirstOrDefault(e => e.userid == UserID && e.ExamID == ExamID);

            return result;
        }

        #endregion

        #region Get By ID Inculde
        public ExamResult GetById(int id)
        {
            return quizContext.ExamResults
                           .Include(r => r.Exam)
                           .FirstOrDefault(r => r.Id == id);
        }

        #endregion



    }
}
