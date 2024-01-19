using RealEstate.Models;
using RealEstate.Repository.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Services.IServices
{
    public interface IAppService
    {
        public Task RentAnHouse(int houseId, int userTentantId );
        public Task LeaveHouse(int housedId, int userTentantId );
        public Task<List<House>> VacantAppartment();
        public Task<List<House>> VacantAppartmentinZone(string zone);
        public Task CommentAnIusses(int IussesId,CommentDTO commentDTO,int userId);
        
    }
}
