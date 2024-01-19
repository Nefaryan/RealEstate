using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Models;
using RealEstate.Repository;
using RealEstate.Repository.DTO;
using RealEstate.Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _repository;
        private readonly ILogger<UserService> _logger;
        public UserService(IGenericRepository<User> repository)
        {
            _repository = repository;
        }
        public async Task Create(User user)
        {
            await _repository.Create(user);
        }

        public async Task<string> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<User>> GetAll()
        {
            var listUser = await _repository.GetAll();

            return listUser;
        }

        public async Task<User> GetSingle(int id)
        {
            var t = await _repository.GetSingle(id);
            return t;
        }

        public async Task Update(int id, User user)
        {
            await _repository.Update(id, user);
        }

        
    }
}
