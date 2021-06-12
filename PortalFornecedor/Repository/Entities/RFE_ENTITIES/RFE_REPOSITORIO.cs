using System;
using System.ComponentModel.DataAnnotations;
using static Shared.Enums.Enums;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_REPOSITORIO : BASE_TABLE
    {
        [StringLength(70)]
        public string Chave { get; set; }
        [Required]
        public ETipoXML? TipoXml { get; set; }
        //[Required]
        [XmlType]
        public string XML { get; set; }
        [StringLength(200)]
        public string Cabecalho { get; set; }
        [StringLength(14)]
        public string CNPJ_DESTINATARIO { get; set; }
        public bool? Processado { get; set; }
        public long? NNF { get; set; }
        [StringLength(20)]
        public string CNPJ_Fornecedor { get; set; }
        public DateTime? DHEMI { get; set; }
        [StringLength(10)]
        public string SERIE { get; set; }
        public OrigemXML? OrigemXML { get; set; }
        public FlagServicos? flag_servico { get; set; }
        [StringLength(14)]
        public string CNPJ_ESTABELECIMENTO_DOWNLOAD;
        public FlgExecRegraNegocio? FLAG_REGRA_NEGOCIO { get; set; }
        [StringLength(2000)]
        public string MSG_ERRO_REGRA_NEGOCIO { get; set; }

        public DateTimeOffset? DHEMI_TZ { get; set; }
        [StringLength(25)]
        public string DHEMI_STRING { get; set; }
        public virtual RFE_NFSE RFE_NFSE { get; set; }
        public RFE_REPOSITORIO()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is RFE_REPOSITORIO rEPOSITORIO &&
                   Chave == rEPOSITORIO.Chave &&
                   CNPJ_DESTINATARIO == rEPOSITORIO.CNPJ_DESTINATARIO &&
                   NNF == rEPOSITORIO.NNF &&
                   CNPJ_Fornecedor == rEPOSITORIO.CNPJ_Fornecedor &&
                   DHEMI == rEPOSITORIO.DHEMI;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Chave, CNPJ_DESTINATARIO, NNF, CNPJ_Fornecedor, DHEMI);
        }
    }
}
