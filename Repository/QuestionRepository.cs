using Quiz.DTO;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuizContext context;

        public QuestionRepository(QuizContext context) 
        {
            this.context = context;
        }
        public Question GetById(int Id)
        {
            Question question = context.Questions.FirstOrDefault(x => x.Id == Id);
            return question;    
        }

        public List<GetBankQuestionDTO> GetBankQuestion(int BankID)
        {
            List<GetBankQuestionDTO> Questions = context.Questions.Where(q=>q.BankID == BankID)
                .Select(q=> new GetBankQuestionDTO
                {
                    QstID = q.Id,
                    QstText = q.Text,
                    QuestionType = q.Type.ToString(),
                    CorAnswer= q.CorrectAnswer,
                    options = q.Type==QuestionType.MCQ? context.Options.Where(o=>o.QuestionID ==q.Id).Select(o=>o.OptionText).ToList() :null
                }).ToList();    
            return Questions;
        }
        public List<GetBankQuestionDTO> FilterQuestions(int BankID, int type)
        {
            List<GetBankQuestionDTO> Questions = context.Questions
                .Where(q => q.BankID == BankID && q.Type == (QuestionType)type)
                .Select(q => new GetBankQuestionDTO
                {
                    QstID = q.Id,
                    QstText = q.Text,
                    CorAnswer = q.CorrectAnswer,
                    options = q.Type == QuestionType.MCQ
                        ? context.Options
                            .Where(o => o.QuestionID == q.Id)
                            .Select(o => o.OptionText)
                            .ToList()
                        : null
                }).ToList();
            return Questions;
        }

        public void Add(Question Question)
        {
            context.Questions.Add(Question);
        }
        public void Remove(int QuestionID)
        {
            Question questionFromDB = context.Questions.FirstOrDefault(q => q.Id == QuestionID);
            context.Remove(questionFromDB);
        }

        public void Save()
        {
            context.SaveChanges();
        }
        

        public IEnumerable<Question> GetAll()
        {
            throw new NotImplementedException();
        }





        public void Update(int id, Question obj)
        {
            throw new NotImplementedException();
        }
    }
}
