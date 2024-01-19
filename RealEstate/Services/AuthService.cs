using RealEstate.Models;
using RealEstate.Repository.DTO;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using RealEstate.Repository;
using RealEstate.Models.DTO;
using RealEstate.Models.Util;
using Microsoft.EntityFrameworkCore.Internal;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstate.Services
{
    public class AuthService
    {

        private readonly IGenericRepository<User> _repository;
        private readonly ILogger<AuthService> _logger;
        private readonly IGenericRepository<Roles> _roles;

        public AuthService(IGenericRepository<User> repository, ILogger<AuthService> logger, IGenericRepository<Roles> roles)
        {
            _repository = repository;
            _logger = logger;
            _roles = roles;
        }

        private async Task RegisterUser(RegistrationDTO registrationDTO, int roleId)
        {
            try
            {
                if (await RegisterCheck(registrationDTO.Username, registrationDTO.email))
                {
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDTO.Password, salt);

                    User user = new User()
                    {
                        Username = registrationDTO.Username,
                        PasswordHash = hashedPassword,
                        Salt = salt,
                        Ruolo = await _roles.GetSingle(roleId),
                        Email = registrationDTO.email
                    };

                    await _repository.Create(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        public async Task RegistrationForTenant(RegistrationDTO registrationDTO)
        {
            await RegisterUser(registrationDTO, 3);
        }

        public async Task RegistrationForOwner(RegistrationDTO registrationDTO)
        {
            await RegisterUser(registrationDTO, 2);
        }

        public async Task RegistrationForAdmin(RegistrationDTO registrationDTO)
        {
            await RegisterUser(registrationDTO, 1);
        }

        public async Task ChangePassword(int userId, ChangePasswordDTO newPasswordDTO)
        {
            try
            {
                var user = await _repository.GetSingle(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (newPasswordDTO.Password == null)
                {
                    throw new Exception("New password cannot be null");
                }

                string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newPasswordDTO.Password, user.Salt);
                user.PasswordHash = newHashedPassword;
                await _repository.Update(userId, user);
                _logger.LogInformation($"New Hashed Password: {newHashedPassword}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        public async Task<UserWithToken> LoginForTenant(LoginDTO loginDTO)
        {
            try
            {
                var user = await _repository.GetSingleByUsername(loginDTO.Username);
                var role = await _roles.GetSingle(3);
                if (user.Ruolo == role)
                    return Login(user, loginDTO.Password);
                else
                    _logger.LogWarning($"User {loginDTO.Username} not found");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }
        public async Task<UserWithToken> LoginForAdmin(LoginDTO loginDTO)
        {
            try
            {
                var user = await _repository.GetSingleByUsername(loginDTO.Username);
                var role = await _roles.GetSingle(1);
                if (user.Ruolo == role)
                    return Login(user, loginDTO.Password);
                else
                    _logger.LogWarning($"User {loginDTO.Username} not found");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        public async Task<UserWithToken> LoginForOwner(LoginDTO loginDTO)
        {
            try
            {
                var user = await _repository.GetSingleByUsername(loginDTO.Username);
                var role = await _roles.GetSingle(2);
                if (user.Ruolo == role)
                    return Login(user, loginDTO.Password);
                else
                    _logger.LogWarning($"User {loginDTO.Username} not found");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash, string salt)
        {
            var hashedEnteredPassword = BCrypt.Net.BCrypt.HashPassword(enteredPassword, salt);
            return hashedEnteredPassword == storedPasswordHash;
        }

        private UserWithToken Login(User user, string enteredPassword)
        {
            try
            {
                if (VerifyPassword(enteredPassword, user.PasswordHash, user.Salt))
                {
                    _logger.LogInformation($"User {user.Username} logged in successfully.");
                    var token = GenerateJwtToken(user);

                    return new UserWithToken
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Token = token
                    };
                }
                else
                {
                    _logger.LogWarning($"Failed login attempt for user {user.Username}.");
                    return null; // Ritorna null se l'autenticazione fallisce
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }


        }

        private async Task<bool> UsernameCheck(string username)
        {
            try
            {
                var user = await _repository.GetSingleByUsername(username);
                if (user != null)
                {
                    _logger.LogError("There is already a user with this username.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        private async Task<bool> RegisterCheck(string username, string email)
        {
            bool b = await EmailCheck(email);
            if (!b)
            {
                _logger.LogError($"Esiste già un user associato a {email}");
                return false;
            }
            bool c = await UsernameCheck(username);
            if (!c)
            {
                _logger.LogError($"Esiste già un user con  {username} come username");
                return false;
            }

            return true;
        }
        private async Task<bool> EmailCheck(string email)
        {
            try
            {
                var users = await _repository.GetAll();

                foreach (var user in users)
                {
                    if (user.Email == email)
                    {
                        _logger.LogError("There is already a user with this email.");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes("una-chiave-placeholder-per-motivi-di-test");
            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
            };

            // Configurazione del token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Scadenza del token ad 1 ora dalla creazione
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
