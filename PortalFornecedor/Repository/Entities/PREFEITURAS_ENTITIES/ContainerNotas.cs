using System;
using static Shared.Extensions.XmlTypesExtension;

namespace Repository.Entities.PREFEITURAS_ENTITIES
{
    public class ContainerNotas : Entity
    {
        public string Chave { get; set; }
        [XmlType]
        public string Xml { get; set; }
        public string Cabecalho { get; set; }
        public string TomadorCnpj { get; set; }
        public long Numero { get; set; }
        public string PrestadorCnpj { get; set; }
        public DateTime DataEmissao { get; set; }
        public string Origem { get; set; }
        public virtual NotasServicos Nfse { get; set; }
    }
}
