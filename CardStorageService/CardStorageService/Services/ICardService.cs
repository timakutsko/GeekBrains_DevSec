using CardStorageService.Data.Models;
using System.Collections.Generic;

namespace CardStorageService.Services
{
    public interface ICardService : ICardStorageService<Card, string>
    {
        IList<Card> GetByClientId (string clientId);
    }
}
