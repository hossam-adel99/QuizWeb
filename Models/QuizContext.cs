using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Quiz.Models
{
    public class QuizContext: IdentityDbContext<ApplicationUser>
    {
   

            public QuizContext(DbContextOptions<QuizContext> options) : base(options)
            {


            }

        public DbSet<QuestionBank> QuestionBanks { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

    }
}
