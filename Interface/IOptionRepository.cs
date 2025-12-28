using Quiz.Models;

namespace Quiz.Interface
{
    public interface IOptionRepository:IGenericRepository<Option>
    {
        public void AddOption(List<Option> option);
    }
}
