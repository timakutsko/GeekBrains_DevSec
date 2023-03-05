namespace CardStorageService.Models.Requests
{
    public class CreateAccountRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }
    }
}
