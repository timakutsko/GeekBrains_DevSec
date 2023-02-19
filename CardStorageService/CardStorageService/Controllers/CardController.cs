using AutoMapper;
using CardStorageService.Data.Models;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CardStorageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly IMapper _mapper;
        private readonly ICardRepository _repository;

        public CardController(ILogger<CardController> logger, IMapper mapper, ICardRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }
        
        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateCardRequest request)
        {
            _logger.LogInformation($"Вызов метода CreateCardRequest с параметрами:" +
                $"\nClientId: { request.ClientId}" +
                $"\nCardNumber: { request.CardNumber}" +
                $"\nName: { request.Name}" +
                $"\nCVV2: { request.CVV2}" +
                $"\nExpDate: { request.ExpDate.ToString("d")}");

            try
            {
                var cardId = _repository.Create(_mapper.Map<Card>(request));
                return Ok(new CreateCardResponse(cardId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create card error.");
                return Ok(new CreateCardResponse(1012, "Create card error."));
            }
        }

        [HttpGet("get-by-client-id")]
        public IActionResult GetByClientId([FromQuery] string clientId)
        {
            _logger.LogInformation($"Вызов метода GetByClientId с параметрами:" +
                $"\nclientId: { clientId}");

            try
            {
                var cards = _repository.GetByClientId(clientId);
                return Ok(new GetCardResponse()
                {
                    Cards = cards.Select(c => _mapper.Map<CardDTO>(c)).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get card error.");
                return Ok(new CreateCardResponse(1013, "Get card error."));
            }
        }
    }
}
