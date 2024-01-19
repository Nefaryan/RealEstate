using MailKit;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Logging;
using RealEstate.Models;
using RealEstate.Models.Mail;
using RealEstate.Models.Util;
using RealEstate.Repository;
using RealEstate.Repository.DTO;
using RealEstate.Services.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.Services
{
    public class AppService : IAppService
    {
        private readonly IHouseService _houseService;
        private readonly IUserService _userService;
        private readonly IIssueService _issueService;
        private readonly ILogger<AppService> _logger;
        private readonly IEmailService _mailService;
        private readonly ICommentService _commentService;

        public AppService(IHouseService houseService, IUserService userService, IIssueService issueService, ILogger<AppService> logger, 
            IEmailService mailService)
        {
            _houseService = houseService;
            _userService = userService;
            _issueService = issueService;
            _logger = logger;
            _mailService = mailService;
        }

        public async Task CommentAnIusses(int IussesId,CommentDTO comment,int userId)
        {
            try
            {
                var issue = await _issueService.GetSingleIssue(IussesId);
                var user= await _userService.GetSingle(userId);
                if (issue == null|| user == null)
                {
                    throw new ArgumentException("Issue or User not found");
                }
                Comments c = new Comments();
                c.Utente = user;
                c.Date = DateTime.Now;
                c.Testo = comment.Text;
                issue.Commenti.Add(c);
                await _issueService.UpdateIssue(IussesId, issue);
                await _commentService.CreateComments(c);
                
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error{ex.Message}");
                throw;
            }
        }

        public async Task LeaveHouse(int housedId, int userTentantId)
        {
            try
            {
                var house = await _houseService.GetHouseById(housedId);
                var user = await _userService.GetSingle(userTentantId);
                if (house == null || user == null)
                {
                    throw new Exception("Error to find house and user");
                }
                house.Inquilino = null;
                house.Disponibilita = true;
                house.DataDisponibilità = DateTime.Now;
                await _houseService.UpdateHouse(housedId, house);
                MailData tenantMailData = new MailData();
                tenantMailData.ToEmail = user.Email;
                tenantMailData.Body = "Thank's for rent a House!";
                tenantMailData.Subject = "House Leave";
                MailData ownerMailData = new MailData();
                tenantMailData.ToEmail = house.Proprietario.Email;
                tenantMailData.Body = $"Your house: {house} has been successfully leave for" +
                    $"user {user.Username} ";
                tenantMailData.Subject = "Leave Done Done";
                _logger.LogInformation($"Leave house for user {userTentantId} as ben completed");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error{ex.Message}");
                throw;

            }
        }

      
        public async Task RentAnHouse(int houseId, int userTentantId)
        {
            try
            {
                var house = await _houseService.GetHouseById(houseId);
                var user = await _userService.GetSingle(userTentantId);
                if (house == null || user == null) 
                {
                    throw new Exception("Error to find house and user");
                }
                house.Inquilino = user;
                house.Disponibilita = false;
                await _houseService.UpdateHouse(houseId, house);
                MailData tenantMailData = new MailData();
                tenantMailData.ToEmail = user.Email;
                tenantMailData.Body = "Welcome to your new house!!";
                tenantMailData.Subject = "New house!";
                MailData ownerMailData = new MailData();
                tenantMailData.ToEmail = house.Proprietario.Email;
                tenantMailData.Body = $"Your house: {house} has been successfully rented ";
                tenantMailData.Subject = "Rent Done";
                await _mailService.SendEmailAsync(tenantMailData);
                await _mailService.SendEmailAsync(ownerMailData);
                _logger.LogInformation($"Rant for user whit id:{userTentantId} completed");

            }catch (Exception ex)
            {
                _logger.LogError($"Error{ex.Message}");
                throw;
            
            }
        }

        public async Task<List<House>> VacantAppartment()
        {
            try
            {
                var houses = await _houseService.GetAllHouse();
                var listOfVancantAppartment = houses.FindAll(h => h.Disponibilita == true);
                if(listOfVancantAppartment == null)
                {
                    throw new Exception("Error to find house");
                }
                return listOfVancantAppartment;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error{ex.Message}");
                throw;

            }

        }

        public async Task<List<House>> VacantAppartmentinZone(string zone)
        {
            try
            {
                var houses = await _houseService.GetAllHouse();
                var listOfVacantAppartmentInZone = houses.FindAll(h => h.Disponibilita == true && h.Citta == zone);
                if(listOfVacantAppartmentInZone == null)
                {
                    throw new Exception("Error to find house");
                }
                return listOfVacantAppartmentInZone;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error{ex.Message}");
                throw;

            }
        }
    }
}
