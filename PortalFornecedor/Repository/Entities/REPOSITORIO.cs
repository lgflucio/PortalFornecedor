using Repository.Entities.RFE_ENTITIES;
using System;

namespace Repository.Entities
{
    public class REPOSITORIO : BASE_TABLE
    {
        public string Chave { get; set; }
        [XmlType]
        public string XML { get; set; }
        public string CNPJ_CPFPrestador { get; set; }
        public string CNPJ_CPFTomador { get; set; }
        public DateTime DtEmissao { get; set; }
    }
}
