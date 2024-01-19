using RealEstate.Models.Mail;
using System.Threading.Tasks;

namespace RealEstate.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailData mailData);
    }
}
