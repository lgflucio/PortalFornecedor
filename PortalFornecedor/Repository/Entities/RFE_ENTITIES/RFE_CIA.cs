using System.ComponentModel.DataAnnotations;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_CIA : BASE_TABLE
    {
        [Required]
        [StringLength(28)]
        public string CNPJ { get; set; }

        [StringLength(60)]
        public string DESCRICAO { get; set; }

        [StringLength(20)]
        public string STATUS { get; set; }

        [StringLength(200)]
        public string ERPCIA01 { get; set; }
        [StringLength(200)]
        public string ERPCIA02 { get; set; }
        [StringLength(200)]
        public string ERPCIA03 { get; set; }

        public int? GRP_ID { get; set; }
    }
}
