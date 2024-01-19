using Microsoft.AspNetCore.Mvc;
using RealEstate.Models.Mail;
using RealEstate.Services.IServices;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RealEstate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
       
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // POST api/<EmailController>
        [HttpPost ("sendMail")]
        public async Task< ActionResult> Post(MailData mailData)
        {
            await _emailService.SendEmailAsync(mailData);
            return Ok();
        }

       
    }
}
