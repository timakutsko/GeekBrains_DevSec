﻿namespace CardStorageService.Models.Responses
{
    public interface IOperationResult
    {
        int ErrorCode { get; }
        
        string ErrorMessage { get; }
    }
}
