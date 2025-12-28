using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Quiz.DTO;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Repository
{
    public class QuestionBankRepository : IQuestionBankRepository
    {
        private readonly QuizContext quizContext;

        public QuestionBankRepository(QuizContext quizContext)
        {
            this.quizContext = quizContext;
        }


        public IEnumerable<QuestionBankWithQuestionsDTO> GetAll()
        {
            // return quizContext.QuestionBanks.Include(q => q.Questions).Include(q => q.User).ToList();
           List<QuestionBank> questionBanks = quizContext.QuestionBanks.Include(q => q.Questions).Include(q => q.User).ToList();

            List<QuestionBankWithQuestionsDTO> questioBankDTOs = new List<QuestionBankWithQuestionsDTO>();

            foreach (var cat in questionBanks)
            {
                questioBankDTOs.Add(new QuestionBankWithQuestionsDTO
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Category = cat.Category,
                    IsPrivate = cat.IsPrivate,
                    UserID = cat.UserID,
                    Questions = cat.Questions.Select(c => new QuestionForQBDTO
                    {
                        Id = c.Id,
                        Text = c.Text,
                        Type=c.Type,
                        CorrectAnswer = c.CorrectAnswer
                    }).ToList()
                });
            }

            return questioBankDTOs;
        }

        public QuestionBankWithQuestionsDTO GetById(int Id)
        {
            QuestionBank bankFromDb = quizContext.QuestionBanks.Include(q => q.Questions).Include(q => q.User).FirstOrDefault(q => q.Id == Id);
            if (bankFromDb == null)
                return null;

            QuestionBankWithQuestionsDTO bank = new QuestionBankWithQuestionsDTO
            {
                Id = bankFromDb.Id,
                Name = bankFromDb.Name,
                Category = bankFromDb.Category,
                IsPrivate = bankFromDb.IsPrivate,
                UserID = bankFromDb.UserID,
                Questions = bankFromDb.Questions.Select(c => new QuestionForQBDTO
                {
                    Id = c.Id,
                    Text = c.Text,
                    Type = c.Type,
                    CorrectAnswer = c.CorrectAnswer
                }).ToList()
            };
            return bank;
        }
        public QuestionBankWithQuestionsDTO GetByName(string Name)
        {
            QuestionBank bankFromDb = quizContext.QuestionBanks.Include(q => q.Questions).Include(q => q.User).FirstOrDefault(q => q.Name == Name);
            if (bankFromDb == null)
                return null;

            QuestionBankWithQuestionsDTO bank = new QuestionBankWithQuestionsDTO
            {
                Id = bankFromDb.Id,
                Name = bankFromDb.Name,
                Category = bankFromDb.Category,
                IsPrivate = bankFromDb.IsPrivate,
                UserID = bankFromDb.UserID,
                Questions = bankFromDb.Questions.Select(c => new QuestionForQBDTO
                {
                    Id = c.Id,
                    Text = c.Text,
                    Type = c.Type,
                    CorrectAnswer = c.CorrectAnswer
                }).ToList()
            };
            return bank;
        }

        public void Add(QuestionBank obj)
        {
            quizContext.Add(obj);
        }


        public void Remove(int id)
        {
            QuestionBank bankFromDb = quizContext.QuestionBanks.Include(q => q.Questions).Include(q => q.User).FirstOrDefault(q => q.Id == id);

            quizContext.Remove(bankFromDb);
        }

        public void Save()
        {
            quizContext.SaveChanges();
        }

        public void Update(int id, QuestionBankAddDTO obj)
        {
            QuestionBank bankFromDb = quizContext.QuestionBanks.Include(q => q.Questions).Include(q => q.User).FirstOrDefault(q => q.Id == id);

            if (bankFromDb != null)
            {
                bankFromDb.Name= obj.Name;
                bankFromDb.IsPrivate= obj.IsPrivate;
                bankFromDb.Category= obj.Category;
                if(obj.Questions!=null)
                {
                    bankFromDb.Questions = obj.Questions;
                }
            }
        }


        IEnumerable<QuestionBank> IGenericRepository<QuestionBank>.GetAll()
        {
            throw new NotImplementedException();
        }

        QuestionBank IGenericRepository<QuestionBank>.GetById(int Id)
        {
            return quizContext.QuestionBanks.Include(q => q.User).FirstOrDefault(q => q.Id == Id);
        }

        public void Update(int id, QuestionBank obj)
        {
            throw new NotImplementedException();
        }

    }
}
