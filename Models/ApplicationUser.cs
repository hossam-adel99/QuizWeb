using Microsoft.AspNetCore.Identity;

namespace Quiz.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ICollection<QuestionBank> QuestionBanks { get; set; }
        public ICollection<Exam> CreatedExams { get; set; }
        public ICollection<ExamResult> ExamResults { get; set; }
    }
}
