namespace Repository.Entities.PREFEITURAS_ENTITIES
{
    public class TagsXmls : Entity
    {
        public string TagDefault { get; set; }
        public string Tag { get; set; }
        public int TipoXmlId { get; set; }
        public string CodigoIbge { get; set; }
        public virtual TiposXmls TipoXml { get; set; }
    }
}
