using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class QuestionBank
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        public string Category { get; set; }
        public bool IsPrivate { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Question>? Questions { get; set; }

    

    



    }
}
