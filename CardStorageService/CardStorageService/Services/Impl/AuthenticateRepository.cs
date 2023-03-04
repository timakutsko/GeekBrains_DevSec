using CardStorageService.Data.Contexts;
using CardStorageService.Data.Models;
using CardStorageService.Models;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CardStorageService.Services.Impl
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly CardStorageServiceDbContext _context;
        private readonly ILogger<AuthenticateRepository> _logger;
        private readonly IOptions<DataBaseOptions> _options;
        private const string _secretKey = "TestKey2@";

        public AuthenticateRepository(ILogger<AuthenticateRepository> logger, CardStorageServiceDbContext context, IOptions<DataBaseOptions> options)
        {
            _logger = logger;
            _context = context;
            _options = options;
        }

        public AccountSessionDTO GetSession(string sessionToken)
        {
            throw new NotImplementedException();
        }

        public AuthenticationResponse Login(AuthenticationRequest authenticationRequest)
        {
            try
            {
                _logger.LogInformation($"Вызов метода Login с параметрами:" +
                    $"\nLogin: { authenticationRequest.Login}" +
                    $"\nPassword: { authenticationRequest.Password }");

                
                // Заглушка для создания первого юзера
                _context.Accounts.Add(new Account
                {
                    EMail = authenticationRequest.Login,
                    PassowrdSalt = PasswordUtils.CreatePasswordValues(authenticationRequest.Password).passwordSalt,
                    PassowrdHash = PasswordUtils.CreatePasswordValues(authenticationRequest.Password).passwordHash,

                });
                _context.SaveChanges();
                return new AuthenticationResponse();
                
                
                //var account = _context.Accounts.FirstOrDefault(a => a.EMail == authenticationRequest.Login);
                //if (account == null)
                //{
                //    _logger.LogError("Account not found.");
                //    throw new Exception("DB: Account not found.");
                //}

                //_context.Cards.Add(item);
                //_context.SaveChanges();
                //return item.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error.");
                throw new Exception("DB: Login error.");
            }
        }

        private string CreateSessionToken(int id)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, id.ToString())}),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
