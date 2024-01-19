using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstate.Repository.DTO;
using RealEstate.Services;
using RealEstate.Services.IServices;
using System.Threading.Tasks;
using System;
using RealEstate.Models.DTO;

namespace RealEstate.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController: ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService autorunService, ILogger<AuthController> logger)
        {
            _authService = autorunService;
            _logger = logger;
        }

        [HttpPost("register/Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationDTO registrationDTO)
        {
            try
            {
                await _authService.RegistrationForAdmin(registrationDTO);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("register/Tenant")]
        public async Task<IActionResult> RegisterTenant([FromBody] RegistrationDTO registrationDTO)
        {
            try
            {
                await _authService.RegistrationForAdmin(registrationDTO);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("register/Owner")]
        public async Task<IActionResult> RegisterOwner([FromBody] RegistrationDTO registrationDTO)
        {
            try
            {
                await _authService.RegistrationForOwner(registrationDTO);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("changepassword/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDTO newPassword)
        {
            try
            {
                await _authService.ChangePassword(userId, newPassword);
                return Ok("Password changed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("Login/Admin")]
        public async Task<IActionResult> LoginAdminUser([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var user = await _authService.LoginForAdmin(loginDTO);
                if (user != null)
                    return Ok($"User logged in successfully {user.Token}");// è presenti il token nel return per motivi di test con Swagger--
                else
                    return Unauthorized("Credenziali di login errate");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost("Login/Owner")]
        public async Task<IActionResult> LoginOwnerUser([FromBody] LoginDTO loginDTO)
        {
            try
            {
                await _authService.LoginForOwner(loginDTO);
                return Ok("User logged in successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost("Login/Tenant")]
        public async Task<IActionResult> LoginTenantUser([FromBody] LoginDTO loginDTO)
        {
            try
            {
                await _authService.LoginForTenant(loginDTO);
                return Ok("User logged in successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
