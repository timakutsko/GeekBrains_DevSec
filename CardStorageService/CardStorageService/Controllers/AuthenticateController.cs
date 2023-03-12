using AutoMapper;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CardStorageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthenticateRepository _repository;

        public AuthenticateController(ILogger<AuthenticateController> logger, IMapper mapper, IAuthenticateRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthenticationRequest request)
        {
            _logger.LogInformation($"Вызов метода Login с параметрами:" +
                $"\nLogin: { request.Login}" +
                $"\nPassword: { request.Password}");

            try
            {
                AuthenticationResponse authenticationResponse = _repository.Login(request);
                return Ok(authenticationResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error.");
                return Ok(new AuthenticationResponse(812, "Login error."));
            }
        }
    }
}
