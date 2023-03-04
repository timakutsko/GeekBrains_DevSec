namespace CardStorageService.Models.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

        public string EMail { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public bool Locked { get; set; }
    }
}
