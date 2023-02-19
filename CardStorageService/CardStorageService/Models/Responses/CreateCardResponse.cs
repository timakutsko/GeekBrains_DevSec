namespace CardStorageService.Models.Responses
{
    public class CreateCardResponse : IOperationResult
    {
        public CreateCardResponse(string cardId)
        {
            CardId = cardId;
        }

        public CreateCardResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string CardId { get; set; } 
    }
}
