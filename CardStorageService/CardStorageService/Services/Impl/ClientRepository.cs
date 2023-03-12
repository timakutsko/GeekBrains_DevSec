using CardStorageService.Data.Contexts;
using CardStorageService.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CardStorageService.Services.Impl
{
    public class ClientRepository : IClientRepository
    {
        private readonly CardStorageServiceDbContext _context;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(ILogger<ClientRepository> logger, CardStorageServiceDbContext context)
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

        public IList<Client> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public int Update(Client item)
        {
            throw new System.NotImplementedException();
        }
    }
}
