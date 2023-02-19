using CardStorageService.Models.DTO;
using System.Collections.Generic;

namespace CardStorageService.Models.Responses
{
    public class GetCardResponse : IOperationResult
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public IList<CardDTO> Cards { get; set; }
    }
}
