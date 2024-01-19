using Microsoft.Extensions.Logging;
using RealEstate.Models;
using RealEstate.Models.Util;
using RealEstate.Repository;
using RealEstate.Services.IServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace RealEstate.Services
{
    public class HouseService : IHouseService
    {
        private readonly IGenericRepository<House> _houseRepository;
        private readonly ILogger<HouseService> _logger;

        public HouseService(IGenericRepository<House> houseRepository, ILogger<HouseService> logger)
        {
            _houseRepository = houseRepository;
            _logger = logger;
        }

        public async Task CreateHouse(House house)
        {
            try
            {
                _logger.LogInformation("Creating House");
                await _houseRepository.Create(house);
                _logger.LogInformation("House created");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<House> GetHouseById(int id) 
        {
            try
            {
                _logger.LogInformation($"Getting house whit id: {id}");
                return await _houseRepository.GetSingle(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<House>> GetAllHouse()
        {
            try
            {
                _logger.LogInformation("Getting all House");
                return await _houseRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }

        }

        public async Task UpdateHouse(int houseId, House updatedHouse)
        {
            try
            {
                _logger.LogInformation($"Updating house whit Id: {houseId}");
                await _houseRepository.Update(houseId, updatedHouse);
                _logger.LogInformation("House updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        public async Task<string> DeleteHouse(int houseId) 
        {
            try
            {
                _logger.LogInformation($"Deleting user with Id: {houseId}");
                return await _houseRepository.Delete(houseId);


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }

        }

       
    }
}
