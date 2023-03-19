using AutoMapper;
using CardStorageService.Data.Models;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IMapper _mapper;
        private readonly IClientService _service;

        public ClientController(ILogger<ClientController> logger, IMapper mapper, IClientService service)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateClientRequest request)
        {
            _logger.LogInformation($"Вызов метода CreateClientRequest с параметрами:" +
                $"\nSurname: { request.Surname}" +
                $"\nFirstName: { request.FirstName}");

            try
            {
                var clientId = _service.Create(_mapper.Map<Client>(request));
                return Ok(new CreateClientResponse(clientId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create client error.");
                return Ok(new CreateClientResponse(912, "Create client error."));
            }
        }
    }
}
