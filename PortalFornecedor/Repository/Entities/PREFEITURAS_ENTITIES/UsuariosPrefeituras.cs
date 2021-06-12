namespace Repository.Entities.PREFEITURAS_ENTITIES
{
    public class UsuariosPrefeituras : Entity
    {
        public UsuariosPrefeituras()
        {

        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Cnpj { get; set; }
        public string CodigoMunicipio { get; set; }
        public string Email { get; set; }

    }
}
