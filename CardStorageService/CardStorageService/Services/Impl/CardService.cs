using CardStorageService.Data.Contexts;
using CardStorageService.Data.Models;
using CardStorageService.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardStorageService.Services.Impl
{
    public class CardService : ICardService
    {
        private readonly CardStorageServiceDbContext _context;
        private readonly ILogger<CardService> _logger;
        private readonly IOptions<DataBaseOptions> _options;

        public CardService(ILogger<CardService> logger, CardStorageServiceDbContext context, IOptions<DataBaseOptions> options)
        {
            _logger = logger;
            _context = context;
            _options = options;
        }

        public string Create(Card item)
        {
            try
            {
                _logger.LogInformation($"Вызов метода Create с параметрами:" +
                    $"\nClientId: { item.ClientId}" +
                    $"\nCardNumber: { item.CardNumber}" +
                    $"\nName: { item.Name}" +
                    $"\nCVV2: { item.CVV2}" +
                    $"\nExpDate: { item.ExpDate.ToString("d")}");

                var client = _context.Clients.FirstOrDefault(c => c.Id == item.ClientId);
                if(client == null)
                {
                    _logger.LogError("Client not found.");
                    throw new Exception("DB: Client not found.");
                }
                
                _context.Cards.Add(item);
                _context.SaveChanges();
                return item.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create card error.");
                throw new Exception("DB: Create card error.");
            }
        }

        public int Delete(Card item)
        {
            throw new NotImplementedException();
        }

        public IList<Card> GetAll()
        {
            throw new NotImplementedException();
        }


        #region Плохой пример без использования EF. Позволяет легко вставлять sql-инъекции
        public IList<Card> GetByClientId(string clientId)
        {
            _logger.LogInformation($"Вызов метода Create с параметрами:" +
                    $"\nClientId: { clientId}");

            List<Card> cards = new List<Card>();
            using (SqlConnection sqlConnection = new SqlConnection(_options.Value.ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand($"select * from cards where ClientId = {clientId}", sqlConnection))
                {
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        cards.Add(new Card
                        {
                            Id = new Guid(reader["Id"].ToString()),
                            CardNumber = reader["CardNumber"].ToString(),
                            Name = reader["Name"].ToString(),
                            CVV2 = reader["CVV2"].ToString(),
                            ExpDate = Convert.ToDateTime(reader["ExpDate"])
                        });
                    }
                }
            }
            return cards;
        }
        #endregion

        public Card GetById(string id)
        {
            return _context.Cards.SingleOrDefault(c => c.Id == new Guid(id));
        }

        public int Update(Card item)
        {
            throw new NotImplementedException();
        }
    }
}
