using Quiz.Models;

namespace Quiz.Repository
{
    public class ExamRepository : GenericRepository<Exam>
    {
        private readonly QuizContext context;
        public ExamRepository(QuizContext context) : base(context)
        {
            this.context = context;
        }
        public IEnumerable<Exam> GetExamsByUserId(string userId)
        {
            return context.Exams.Where(e => e.CreatedBy == userId).ToList();
        }
    }
}
