namespace Repository.DTOs
{
    public static class CodigoIbgeDTO
    {
        public const string Barueri      = "3505708";
        public const string Campinas     = "3509502";
        public const string Chapeco      = "4204202";
        public const string Guarulhos    = "3518800";
        public const string Manaus       = "1302603";
        public const string RioDeJaneiro = "3304557";
        public const string Santos       = "3548500";
        public const string SaoPaulo     = "3550308";
        public const string Uberlandia   = "3170206";
        public const string Blumenau     = "4202404";

        public static string GetMunicipioGerador(string municipio)
        {
            switch (municipio)
            {
                case "BARUERI":
                    return Barueri;

                case "CAMPINAS":
                    return Campinas;

                case "CHAPECO":
                    return Chapeco;

                case "GUARULHOS":
                    return Guarulhos;

                case "MANAUS":
                    return Manaus;

                case "RIO DE JANEIRO":
                    return RioDeJaneiro;

                case "SANTOS":
                    return Santos;

                case "SAO PAULO":
                    return SaoPaulo;

                case "UBERLANDIA":
                    return Uberlandia;

                default:
                    return "";
            }

        }
    }
}
