using Microsoft.EntityFrameworkCore;
using Quiz.Interface;
using Quiz.Models;

namespace Quiz.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
       
            QuizContext context;
            DbSet<T> _dbSet;
            public GenericRepository(QuizContext _context)
            {
                context = _context;
                _dbSet = _context.Set<T>();

            }
            public void Add(T obj)
            {
                _dbSet.Add(obj);
            }

            public IEnumerable<T> GetAll()
            {
                return _dbSet;
            }

            public T GetById(int Id)
            {
                return _dbSet.Find(Id);
            }


            public void Remove(int id)
            {

                _dbSet.Remove(_dbSet.Find(id));
            }

            public void Update(int id, T obj)
            {
                var existingEntity = _dbSet.Find(id);
                if (existingEntity != null)
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(obj);
                }
            }
            public void Save()
            {
                context.SaveChanges();
            }
        }
    }

