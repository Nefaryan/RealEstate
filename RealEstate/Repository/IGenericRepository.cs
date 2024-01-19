using RealEstate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task Create(T entity);
        public Task Update(int id,T entity);
        public Task<string> Delete(int id);
        public Task<T> GetSingle(int id); 
        public Task<List<T>> GetAll();
        public Task<User> GetSingleByUsername(string username);
    }
}
