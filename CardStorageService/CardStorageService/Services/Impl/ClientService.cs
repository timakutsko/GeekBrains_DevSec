using CardStorageService.Data.Contexts;
using CardStorageService.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardStorageService.Services.Impl
{
    public class ClientService : IClientService
    {
        private readonly CardStorageServiceDbContext _context;
        private readonly ILogger<ClientService> _logger;

        public ClientService(ILogger<ClientService> logger, CardStorageServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public int Create(Client item)
        {
            try
            {
                _logger.LogInformation($"Вызов метода Create с параметрами:" +
                    $"\nSurname: { item.Surname}" +
                    $"\nFirstName: { item.FirstName}");
                
                _context.Clients.Add(item);
                _context.SaveChanges();
                return item.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create client error.");
                throw new Exception("DB: Create client error.");
            }
        }

        public int Delete(Client item)
        {
            throw new System.NotImplementedException();
        }

        public IList<Client> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Client GetById(int id)
        {
            _logger.LogInformation($"Вызов метода GetById с параметрами:" +
                $"\nId: {id}");

            Client client = _context.Clients.SingleOrDefault(c => c.Id == id);
            if (client == null)
            {
                _logger.LogError("Client is not found.");
                throw new Exception("DB: Client is not found.");
            }
            else
                return client;
        }

        public int Update(Client item)
        {
            throw new System.NotImplementedException();
        }
    }
}
