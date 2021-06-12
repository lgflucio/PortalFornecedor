namespace Repository.Entities.PREFEITURAS_ENTITIES
{
    public class Municipios : Entity
    {
        public string SiglaUf { get; set; }
        public int CodigoUf { get; set; }
        public int CodigoMunicipio { get; set; }
        public string Descricao { get; set; }
        public string CodigoIbge { get; set; }
    }
}
