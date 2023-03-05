using CardStorageService.Data.Models;

namespace CardStorageService.Services
{
    public interface IClientService : ICardStorageService<Client, int>
    {
    }
}
