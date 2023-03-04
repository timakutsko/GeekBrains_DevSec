using CardStorageService.Data.Models;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;

namespace CardStorageService.Services
{
    public interface IAuthenticateRepository
    {
        AuthenticationResponse Login(AuthenticationRequest authenticationRequest);

        public AccountSessionDTO GetSession(string sessionToken);
    }
}
