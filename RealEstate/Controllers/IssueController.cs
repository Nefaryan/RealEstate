using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEstate.Models.Mail;
using RealEstate.Services.IServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using RealEstate.Services;
using RealEstate.Repository.DTO;

namespace RealEstate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;
        private readonly IHouseService _houseService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public IssueController(IIssueService issueService, IHouseService houseService, IUserService userService, IEmailService emailService)
        {
            _issueService = issueService;
            _houseService = houseService;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<List<IssueDTO>>> Get()
        {
            try
            {
                var issues = await _issueService.GetAllIssues();

                return Ok(issues);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<IssueDTO>> Get(int id)
        {
            try
            {
                var issue = await _issueService.GetSingleIssue(id);
                if (issue == null)
                {
                    return NotFound();
                }

                return Ok(issue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<IssueDTO>> Post([FromBody] Issue issue)
        {
            if (issue == null)
            {
                return BadRequest("Invalid request body");
            }

            try
            {
                await _issueService.CreateIssue(issue);

                if (issue.Proprietà != null)
                {
                    var idProprieta = issue.Proprietà.Id;
                    var house = await _houseService.GetHouseById(idProprieta);
                    if (house != null)
                    {
                        var iduser = house.Proprietario.Id;
                        var user = await _userService.GetSingle(iduser);
                        if (user != null)
                        {
                            MailData mail = new MailData();
                            mail.ToEmail = user.Email;
                            mail.Subject = "New Notification";
                            mail.Body = "You have new information about the state of the house. Please check the issue";

                            await _emailService.SendEmailAsync(mail);
                        }
                    }
                }

                return CreatedAtAction("Get", new { id = issue.Id }, issue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("{userid}/{houseId}/addIssue")]
        public async Task<IActionResult> AddIssueToHouseAsync(int houseId, int userid, [FromBody] IssueDTO issue)
        {
            if (issue == null)
            {
                return BadRequest("Invalid request body");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await GetUser(userid);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var house = await _houseService.GetHouseById(houseId);
                if (house == null)
                {
                    return NotFound("House not found");
                }
                Issue issue1 = new();

                issue1.Utente = user;
                issue1.Proprietà = house;


                await _issueService.CreateIssue(issue1);

                return CreatedAtAction(nameof(Get), new { id = issue1.Id }, issue1);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }


        [HttpDelete("{houseId}/{userid}/deleteIssue/{issueId}")]
        [Authorize]
        public async Task<IActionResult> DeleteIssueInHouseAsync(int houseId, int userid, int issueId)
        {
            try
            {
                var user = await GetUser(userid);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var house = await _houseService.GetHouseById(houseId);
                if (house == null)
                {
                    return NotFound("House not found");
                }

                var existingIssue = await _issueService.GetSingleIssue(issueId);
                if (existingIssue == null)
                {
                    return NotFound("Issue not found");
                }

                await _issueService.Delete(issueId);

                return Ok("Issue deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPut("{houseId}/{userid}/updateIssue/{issueId}")]
        public async Task<IActionResult> UpdateIssueInHouseAsync(int houseId, int userid, int issueId, [FromBody] IssueDTO updatedIssue)
        {
            if (updatedIssue == null)
            {
                return BadRequest("Invalid request body");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await GetUser(userid);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var house = await _houseService.GetHouseById(houseId);
                if (house == null)
                {
                    return NotFound("House not found");
                }

                var existingIssue = await _issueService.GetSingleIssue(issueId);
                if (existingIssue == null)
                {
                    return NotFound("Issue not found");
                }


                existingIssue.Name = updatedIssue.Name;
                existingIssue.Description = updatedIssue.Description;
                existingIssue.Stato = updatedIssue.State;
                existingIssue.DataApertura = updatedIssue.DataApertura;
                existingIssue.DataChiusura = updatedIssue.DataChiusura;


                await _issueService.UpdateIssue(issueId, existingIssue);

                return Ok(existingIssue);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }



        private async Task<User> GetUser(int userId)
        {
            return await _userService.GetSingle(userId);
        }



        [HttpGet("{id}/status")]
        [Authorize]
        public async Task<IActionResult> GetIssueStatus(int id)
        {
            try
            {
                var issue = await _issueService.GetSingleIssue(id);
                if (issue == null)
                {
                    return NotFound("Issue not found");
                }

                return Ok(issue.Stato);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] IssueDTO issue)
        {
            try
            {
                var existingIssue = await _issueService.GetSingleIssue(id);

                if (existingIssue == null)
                {
                    return NotFound();
                }


                existingIssue.Name = issue.Name;
                existingIssue.Description = issue.Description;
                existingIssue.DataApertura = issue.DataApertura;
                existingIssue.DataChiusura = issue.DataChiusura;
                existingIssue.Proprietà = issue.House;
                existingIssue.Stato = issue.State;
                existingIssue.Utente = issue.User;


                await _issueService.UpdateIssue(id, existingIssue);

                return NoContent();
            }
            catch (Exception ex)
            {



                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task< IActionResult> Delete(int id)
        {
            try { 
            var result = await _issueService.Delete(id);

                if (result == "Item deleted")
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}
