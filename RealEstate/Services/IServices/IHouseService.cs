using RealEstate.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Services.IServices
{
    public interface IHouseService
    {
        public Task CreateHouse(House house);
        public Task UpdateHouse(int id, House house);
        public Task<string> DeleteHouse(int id);
        public Task<House> GetHouseById(int id);
        public Task<List<House>> GetAllHouse();

    }
}
