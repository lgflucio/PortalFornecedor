namespace Repository.DTOs
{
    public class AuthReturnDTO
    {
        public AuthReturnDTO()
        {

        }
        public AuthReturnDTO(string message, string usuarioNome = null, string token = null)
        {

        }

        public string Token { get; set; }
        public string TokenType { get; set; }
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public ContextUserDTO User { get; set; }

    }
}
