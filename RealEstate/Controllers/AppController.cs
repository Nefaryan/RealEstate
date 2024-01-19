using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstate.Repository.DTO;
using RealEstate.Services;
using RealEstate.Services.IServices;
using System;
using System.Threading.Tasks;

namespace RealEstate.Controllers
{
    [ApiController]
    [Route("api/app")]
    public class AppController: ControllerBase
    {
        private readonly IAppService _appService;
        private readonly ILogger<AppController> _logger;

        public AppController(IAppService appService, ILogger<AppController> logger)
        {
            _appService = appService;
            _logger = logger;
        }

        [HttpPut("PutRent")]
        public async Task<IActionResult> RentAppartment(int houseId, int userTentantId)
        {
            try
            {
                _logger.LogInformation($"Rent house {houseId} to user {userTentantId}");
                await _appService.RentAnHouse(houseId, userTentantId);
                _logger.LogInformation($"Rent complated");
                return Ok();

            }
            catch(Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        [HttpPut("PutLeave")]
        public async Task<IActionResult> LeaveAppartment(int houseId, int userTentantId)
        {
            try
            {
                _logger.LogInformation($"Leaving house {houseId} for user {userTentantId}");
                await _appService.LeaveHouse(houseId, userTentantId);
                _logger.LogInformation("Leaving completed");
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }

        }
        [HttpGet("VacantAppartment")]
        public async Task<IActionResult> SeeAllVacantAppartment()
        {
            try
            {
                _logger.LogInformation("Getting all vacant house");
                var house = await _appService.VacantAppartment();
                return Ok(house);   
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        [HttpGet("VacantAppartemntInZone")]
        public async Task<IActionResult> SeeAllVacantAppartemntInZone(string zone)
        {
            try
            {
                _logger.LogInformation($"Find vacant house in zone {zone}");
                var houses = await _appService.VacantAppartmentinZone(zone);
                return Ok(houses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CommentIssues(int IssueId, [FromBody] CommentDTO commentDTO, int userId)
        {
            try
            {
                _logger.LogInformation($"Comment Issues");
                await _appService.CommentAnIusses(IssueId, commentDTO, userId);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
