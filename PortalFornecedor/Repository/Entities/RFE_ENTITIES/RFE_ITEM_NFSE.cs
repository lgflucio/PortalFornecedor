using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_ITEM_NFSE : BASE_TABLE
    {
        [ForeignKey("HEADER")]
        public int ID_HEADER { get; set; }
        public virtual RFE_NFSE HEADER { get; set; }
        [StringLength(70)]
        public string CHNFSE { get; set; }
        [StringLength(3)]
        public string NITEM { get; set; }
        [StringLength(20)]
        public string CODIGO_SERVICO { get; set; }
        [StringLength(20)]
        public string CODIGO_SERVICO_CIA { get; set; }
        [StringLength(3000)]
        [VarcharType]
        public string DESCRICAO_SERVICO { get; set; }
        public int? QTD_ITEM_SERVICO { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALOR_UNITARIO { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALOR_TOTAL { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALOR_DESCONTO { get; set; }
        public bool? TRIBUTADO { get; set; }
        [StringLength(50)]
        public string STATUS { get; set; }
        [StringLength(60)]
        public string XPED { get; set; }
        [StringLength(12)]
        public string NITEMPED { get; set; }
        public string FOLHA_MEDICAO { get; set; }
    }
}
