using Microsoft.EntityFrameworkCore;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Repository
{
    public class ExamQuestionsRepository : GenericRepository<ExamQuestion> ,IExamQuestionsRepository
    {
        private readonly QuizContext context;
        public ExamQuestionsRepository(QuizContext context) : base(context)
        {
            this.context = context;
        }
        public IEnumerable<ExamQuestion> GetAllByExamId(int examId)
        {
            return context.ExamQuestions
                .Include(eq => eq.Question)
                .Select(eq => new ExamQuestion
                {
                    Id = eq.Id,
                    QuestionID = eq.QuestionID,
                    ExamID = eq.ExamID,
                    Points = eq.Points,
                    Question = new Question
                    {
                        Id = eq.Question.Id,
                        Text = eq.Question.Text,
                        Type = eq.Question.Type,
                        CorrectAnswer = eq.Question.CorrectAnswer,
                        Options = eq.Question.Options
                    }
                })
                .Where(eq => eq.ExamID == examId)
                .ToList();
        }
        
    }
}
