using CardStorageService.Data.Models;
using CardStorageService.Models.DTO;

namespace CardStorageService.Models.Responses
{
    public class CreateAccountResponse : IOperationResult
    {
        public CreateAccountResponse(Account account)
        {
            Account = account;
        }

        public CreateAccountResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public Account Account { get; set; }
    }
}
