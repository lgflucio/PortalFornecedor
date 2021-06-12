namespace Repository.DTOs
{
    public class AuthenticateDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
