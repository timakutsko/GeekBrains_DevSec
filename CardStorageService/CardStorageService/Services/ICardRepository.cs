using CardStorageService.Data.Models;
using System.Collections.Generic;

namespace CardStorageService.Services
{
    public interface ICardRepository : IRepository<Card, string>
    {
        IList<Card> GetByClientId (string clientId);
    }
}
