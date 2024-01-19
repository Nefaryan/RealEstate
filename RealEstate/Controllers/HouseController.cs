using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEstate.Models.Mail;
using RealEstate.Services;
using RealEstate.Services.IServices;
using System;
using System.Threading.Tasks;

namespace RealEstate.Controllers
{
    [ApiController]
    [Route("api/house")]
    public class HouseController : ControllerBase
    {
        private readonly IHouseService _houseService;
        private readonly IEmailService _emailService;

        public HouseController(IHouseService houseService, IEmailService emailService)
        {
            _houseService = houseService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHouse([FromBody] House house)
        {
            try
            {
               await _houseService.CreateHouse(house);
                return Ok("House created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("{houseId}")]
        public async Task<IActionResult> GetHouseById(int houseId)
        {
            try
            {
                var house = await _houseService.GetHouseById(houseId);
                return Ok(house);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllHouse()
        {
            try
            {
                var houses = await _houseService.GetAllHouse();
                return Ok(houses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{houseId}")]
        public async Task<IActionResult> UpdateHouse(int houseId, [FromBody] House house) 
        {
            try
            {

                await _houseService.UpdateHouse(houseId, house);
                var houseUp = await _houseService.GetHouseById(houseId);
                if (houseUp.Inquilino == null && house.Inquilino != null) 
                {
                    
                    MailData mail = new MailData();
                    mail.ToEmail = houseUp.Proprietario.Email;
                    mail.Subject = "New Notification";
                    mail.Body =$"You have new Tenant in your house.";

                    await _emailService.SendEmailAsync(mail);
                }else if(houseUp.Inquilino != null && house.Inquilino == null)
                {
                    MailData mail = new MailData();
                    mail.ToEmail = houseUp.Proprietario.Email;
                    mail.Subject = "New Notification";
                    mail.Body = $"Your Tenant has left the house.";
                    await _emailService.SendEmailAsync(mail);


                }
                return Ok("House Updated");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

        [HttpDelete("{houseId}")]
        public async Task<IActionResult> DeleteHouse(int houseId) 
        {
            try
            {
               await _houseService.DeleteHouse(houseId);
                var house = await _houseService.GetHouseById(houseId);
                MailData mail = new MailData();
                mail.ToEmail = house.Proprietario.Email;
                mail.Subject = "New Notification";
                mail.Body = $"Your house has been successfully delete on the App .";
                await _emailService.SendEmailAsync(mail);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

    }
}
