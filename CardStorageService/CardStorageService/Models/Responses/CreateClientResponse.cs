namespace CardStorageService.Models.Responses
{
    public class CreateClientResponse : IOperationResult
    {
        public CreateClientResponse(int? clientId)
        {
            ClientId = clientId;
        }

        public CreateClientResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public int? ClientId { get; set; } 
    }
}
