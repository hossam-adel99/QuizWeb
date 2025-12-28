using Microsoft.AspNetCore.Mvc;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Repository
{
    public class OptionRepository : IOptionRepository
    {
        private readonly QuizContext context;

        public OptionRepository(QuizContext context)
        {
            this.context = context;
        }

        public void AddOption(List<Option> option)
        {
            context.Options.AddRange(option);    
        }

        public void Add(Option obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Option> GetAll()
        {
            throw new NotImplementedException();
        }

        public Option GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int QuestionID)
        {
            List<Option> questionFromDB = context.Options.Where(o=>o.QuestionID == QuestionID).ToList();
            context.RemoveRange(questionFromDB);
        }


        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(int id, Option obj)
        {
            throw new NotImplementedException();
        }
    }
}
