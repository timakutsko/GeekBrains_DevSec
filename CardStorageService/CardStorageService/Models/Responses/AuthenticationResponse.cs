using CardStorageService.Models.DTO;

namespace CardStorageService.Models.Responses
{
    public class AuthenticationResponse : IOperationResult
    {
        // Заглушка для создания первого юзера
        public AuthenticationResponse()
        {
        }

        public AuthenticationResponse(AccountSessionDTO session)
        {
            Session = session;
        }

        public AuthenticationResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public AccountSessionDTO Session { get; set; }
    }
}
