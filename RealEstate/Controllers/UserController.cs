using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEstate.Services;
using RealEstate.Services.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RealEstate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult< List<User>> >GetAll()
        {
            return await _userService.GetAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult< User>> GetSingle(int id)
        {
            return await _userService.GetSingle(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
           await _userService.Create(user);
            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, User user)
        {
             await _userService.Update(id, user);
            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
           return await _userService.Delete(id);
            

        }
    }
}
