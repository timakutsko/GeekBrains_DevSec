using CardStorageService.Data.Models;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;

namespace CardStorageService.Services
{
    public interface IAuthenticateService
    {
        CreateAccountResponse Create(CreateAccountRequest createAccountRequest);

        AuthenticationResponse Login(AuthenticationRequest authenticationRequest);

        public AccountSessionDTO GetSession(string sessionToken);
    }
}
