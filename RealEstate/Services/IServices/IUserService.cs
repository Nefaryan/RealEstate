using RealEstate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Services.IServices
{
    public interface IUserService
    {
        public Task Create(User user);
        public Task Update(int id, User user);
        public Task<string> Delete(int id);
        public Task< User> GetSingle(int id);
        public Task< List<User>> GetAll();
      
    }
}
