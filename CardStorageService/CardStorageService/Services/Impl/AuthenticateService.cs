using CardStorageService.Data.Contexts;
using CardStorageService.Data.Models;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CardStorageService.Services.Impl
{
    public class AuthenticateService : IAuthenticateService
    {
        public const string SecretKey = "kYp3s6v9y/B?E(H+";
        private readonly ILogger<AuthenticateService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Dictionary<string, AccountSessionDTO> _sessions = new Dictionary<string, AccountSessionDTO>();

        public AuthenticateService(ILogger<AuthenticateService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public AccountSessionDTO GetSession(string sessionToken)
        {
            try
            {
                _logger.LogInformation($"Вызов метода GetSession с параметрами:" +
                        $"\nSessionToken: { sessionToken}");

                AccountSessionDTO accountSessionDTO;

                lock (_sessions)
                {
                    _sessions.TryGetValue(sessionToken, out accountSessionDTO);
                }

                if (accountSessionDTO == null)
                {
                    using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
                    {
                        CardStorageServiceDbContext scopedContext = serviceScope.ServiceProvider.GetRequiredService<CardStorageServiceDbContext>();
                        AccountSession accountSession = GetAccountSessionByToken(scopedContext, sessionToken);
                        if (accountSession == null)
                            return null;

                        Account account = GetAccountBySession(scopedContext, accountSession);
                        if (account == null)
                            return null;

                        accountSessionDTO = CreateSession(account, accountSession);
                        if (accountSessionDTO != null)
                        {
                            lock (_sessions)
                            {
                                if (!_sessions.ContainsKey(accountSessionDTO.SessionToken))
                                    _sessions[accountSessionDTO.SessionToken] = accountSessionDTO;
                            }
                        }
                    }
                }

                return accountSessionDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetSession error.");
                throw new Exception("DB: GetSession error.");
            }
        }

        public CreateAccountResponse Create(CreateAccountRequest createAccountRequest)
        {
            try
            {
                _logger.LogInformation($"Вызов метода Create с параметрами:" +
                    $"\nLogin: { createAccountRequest.Login}" +
                    $"\nPassword: { createAccountRequest.Password }" +
                    $"\nSurname: { createAccountRequest.Surname }" +
                    $"\nFirstName: { createAccountRequest.FirstName }");

                using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
                {
                    CardStorageServiceDbContext scopedContext = serviceScope.ServiceProvider.GetRequiredService<CardStorageServiceDbContext>();
                    Account account = GetAccountByLogin(scopedContext, createAccountRequest.Login);
                    if (account != null)
                    {
                        _logger.LogError("Account wath added earlier.");
                        return new CreateAccountResponse(101, "Account wath added earlier!");
                    }

                    account = new Account
                    {
                        EMail = createAccountRequest.Login,
                        Surname = createAccountRequest.Surname,
                        FirstName = createAccountRequest.FirstName,
                    };

                    (string passwordSalt, string passwordHash) = PasswordUtils.CreatePasswordValues(createAccountRequest.Password);
                    if (passwordSalt == null || passwordHash == null)
                    {
                        _logger.LogError("Invalid password sald and password hash created.");
                        return new CreateAccountResponse(102, "Invalid password sald and password hash created!");
                    }

                    account.PassowrdSalt = passwordSalt;
                    account.PassowrdHash = passwordHash;

                    scopedContext.Accounts.Add(account);
                    scopedContext.SaveChanges();

                    return new CreateAccountResponse(account);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create error.");
                throw new Exception("DB: Create error.");
            }
        }

        public AuthenticationResponse Login(AuthenticationRequest authenticationRequest)
        {
            try
            {
                _logger.LogInformation($"Вызов метода Login с параметрами:" +
                    $"\nLogin: { authenticationRequest.Login}" +
                    $"\nPassword: { authenticationRequest.Password }");

                using IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
                {
                    CardStorageServiceDbContext scopedContext = serviceScope.ServiceProvider.GetRequiredService<CardStorageServiceDbContext>();
                    Account account =
                        !string.IsNullOrWhiteSpace(authenticationRequest.Login) ?
                        GetAccountByLogin(scopedContext, authenticationRequest.Login) : null;
                    if (account == null)
                    {
                        _logger.LogError("Account not found.");
                        return new AuthenticationResponse(201, "Account not found!");
                    }

                    if (!PasswordUtils.PasswordVerify(authenticationRequest.Password, account.PassowrdSalt, account.PassowrdHash))
                    {
                        _logger.LogError("Invalid password.");
                        return new AuthenticationResponse(202, "Invalid password!");
                    }

                    AccountSession accountSession = new AccountSession
                    {
                        AccountId = account.Id,
                        SessionToken = CreateSessionToken(account),
                        IsClosed = false,
                    };

                    scopedContext.AccountSessions.Add(accountSession);
                    scopedContext.SaveChanges();

                    AccountSessionDTO accountSessionDTO = CreateSession(account, accountSession);
                    lock (_sessions)
                    {
                        if (!_sessions.ContainsKey(accountSessionDTO.SessionToken))
                            _sessions[accountSessionDTO.SessionToken] = accountSessionDTO;
                    }

                    return new AuthenticationResponse(accountSessionDTO);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error.");
                throw new Exception("DB: Login error.");
            }
        }

        private string CreateSessionToken(Account account)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecretKey);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Name, account.EMail)}),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        private Account GetAccountByLogin(CardStorageServiceDbContext context, string login)
        {
            return context.Accounts.FirstOrDefault(a => a.EMail == login);
        }

        private Account GetAccountBySession(CardStorageServiceDbContext context, AccountSession session)
        {
            return context.Accounts.FirstOrDefault(a => a.Id == session.AccountId);
        }

        private AccountSession GetAccountSessionByToken(CardStorageServiceDbContext context, string token)
        {
            return context.AccountSessions.FirstOrDefault(s => s.SessionToken == token);
        }

        private AccountSessionDTO CreateSession(Account account, AccountSession accountSession)
        {
            return new AccountSessionDTO
            {
                SessionId = accountSession.Id,
                SessionToken = accountSession.SessionToken,
                Account = new AccountDTO
                {
                    AccountId = account.Id,
                    EMail = account.EMail,
                    Surname = account.Surname,
                    FirstName = account.FirstName,
                    Locked = account.Locked
                }
            };
        }
    }
}
