using AutoMapper;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http.Headers;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthenticateService _service;

        public AuthenticateController(ILogger<AuthenticateController> logger, IMapper mapper, IAuthenticateService service)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateAccountRequest request)
        {
            _logger.LogInformation($"Вызов метода Create с параметрами:" +
                $"\nLogin: { request.Login}" +
                $"\nPassword: { request.Password}");

            try
            {
                CreateAccountResponse createAccountResponse = _service.Create(request);
                return Ok(createAccountResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create account error.");
                return Ok(new AuthenticationResponse(701, "Create account error."));
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthenticationRequest request)
        {
            _logger.LogInformation($"Вызов метода Login с параметрами:" +
                $"\nLogin: { request.Login}" +
                $"\nPassword: { request.Password}");

            try
            {
                AuthenticationResponse authenticationResponse = _service.Login(request);
                if (authenticationResponse.Session != null)
                {
                    Response.Headers.Add("X-Session-Token", authenticationResponse.Session.SessionToken);
                }
                return Ok(authenticationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error.");
                return Ok(new AuthenticationResponse(812, "Login error."));
            }
        }

        [HttpGet("session")]
        public IActionResult GetSessionInfo()
        {
            _logger.LogInformation($"Вызов метода GetSessionInfo без параметров");

            StringValues authorization = Request.Headers[HeaderNames.Authorization];
            if (AuthenticationHeaderValue.TryParse(authorization, out AuthenticationHeaderValue authenticationHeaderValue))
            {
                string scheme = authenticationHeaderValue.Scheme;
                string token = authenticationHeaderValue.Parameter;
                if (string.IsNullOrEmpty(token))
                    return Unauthorized();

                AccountSessionDTO accountSessionDTO = _service.GetSession(token);
                if (accountSessionDTO == null)
                    return Unauthorized();

                return Ok(accountSessionDTO);
            }

            return Unauthorized();
        }
    }
}
