using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Data;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly RealEstateContext _context;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(RealEstateContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Create(T entity)
        {
            try
            {
                _logger.LogInformation("Adding entity");
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Entity Created");

            }
            catch(Exception ex)
            {
                _logger.LogError($"Error:{ex.Message}");
                throw;
            }
        }

        public async Task<string> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Item");
                var t = await _context.Set<T>().FindAsync(id);
                if (t == null)
                {
                    throw new Exception("Item not find");
                }
                _context.Set<T>().Remove(t);
               await _context.SaveChangesAsync();
                _logger.LogInformation("Item Deleted");
                return "Item deleted";

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex.Message}");
                throw;
            }
        }

        public async Task<List<T>> GetAll()
        {
            try
            {
                _logger.LogInformation("Get all item");
                var list = await _context.Set<T>().ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex.Message}");
                throw;
            }
        }

        public async Task<T> GetSingle(int id)
        {
            try
            {
                _logger.LogInformation("Finding item");
                var t = await _context.Set<T>().FindAsync(id);
                if (t == null)
                {
                    throw new Exception("Item not found");
                }
                _logger.LogInformation("Item found");
                return t;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex.Message}");
                throw;
            }
        }

        public async Task Update(int id, T updatedEntity)
        {
            try
            {
                var existingEntity = await _context.Set<T>().FindAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogInformation("Item not found");
                    throw new Exception("Item not found");
                }
                _context.Entry(existingEntity).State = EntityState.Detached;
                _context.Update(existingEntity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Item Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetSingleByUsername(string username)
        {
            try
            {
                var user = await _context.Set<User>().Include(u => u.Ruolo).FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    _logger.LogInformation("User not found");
                    return null;
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
