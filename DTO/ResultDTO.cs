using Quiz.Models;

namespace Quiz.DTO
{
    public class ResultDTO
    {
        public string userid { set; get; }
        public string UseName { set; get; }

        public string ExamName { set; get; }

        public int ExamID { set; get; }

        public double? score { set; get; }

        public DateTime date = DateTime.Now;

        public string FeedBack { set; get; }

        public string CreatorID { set; get; }
        
        public  ICollection<UserAnswer>? userAnswers { set; get; }


    }
}
