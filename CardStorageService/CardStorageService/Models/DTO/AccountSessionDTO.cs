namespace CardStorageService.Models.DTO
{
    public class AccountSessionDTO
    {
        public int SessionId { get; set; }

        public string SessionToken { get; set; }

        public AccountDTO Account { get; set; }
    }
}
