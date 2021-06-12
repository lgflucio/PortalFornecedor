
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils;
using static Shared.Enums.Enums;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_NFSE : BASE_TABLE
    {

        public RFE_NFSE()
        {
            Itens = new List<RFE_ITEM_NFSE>();
        }
        private string _TOMADORCNPJ, _TOMADORCPF, _PRESTADORCNPJ, _PRESTADORCPF;

        public TipoNFSE? TIPO_NFSE { get; set; }
        public EStatusRFeNFSe? STATUS { get; set; }
        public StatusNDDFrete? STATUS_NDD_FRETE { get; set; }

        [StringLength(70)]
        public string CHNFSE { get; set; }
        [StringLength(14)]
        public string MUNGER { get; set; }
        public long? NUNFSE { get; set; }
        [StringLength(10)]
        public string SERIENFSE { get; set; }
        [StringLength(10)]
        public string TIPO { get; set; }
        public DateTime? DHEMI { get; set; }
        [StringLength(5)]
        public string NATUREZAOPERACAO { get; set; }
        [StringLength(5)]
        public string REGIMEESPECIALTRIBUTACAO { get; set; }
        [StringLength(10)]
        public string OPTANTESIMPLES { get; set; }
        [StringLength(12)]
        public string COMPETENCIA { get; set; }
        public string DISCRIMINACAO { get; set; }
        [StringLength(50)]
        public string CODVERIFICACAO { get; set; }

        [StringLength(20)]
        public string CODSERVNFSE { get; set; }
        [StringLength(100)]
        public string CODSERV_CODTRIB_NFSE { get; set; }
        [StringLength(255)]
        public string TOMADORRAZAO { get; set; }
        [StringLength(28)]
        public string TOMADORCNPJ
        {
            get { return _TOMADORCNPJ; }
            set { _TOMADORCNPJ = string.IsNullOrWhiteSpace(value) ? "" : Util.DesformatarCNPJCPF(value); }
        }
        [StringLength(16)]
        public string TOMADORCPF
        {
            get { return _TOMADORCPF; }
            set { _TOMADORCPF = string.IsNullOrWhiteSpace(value) ? "" : Util.DesformatarCNPJCPF(value); }
        }
        [StringLength(255)]
        public string PRESTADORRAZAO { get; set; }
        [StringLength(28)]
        public string PRESTADORCNPJ
        {
            get { return _PRESTADORCNPJ; }
            set { _PRESTADORCNPJ = string.IsNullOrWhiteSpace(value) ? "" : Util.DesformatarCNPJCPF(value); }
        }
        [StringLength(16)]
        public string PRESTADORCPF
        {
            get { return _PRESTADORCPF; }
            set { _PRESTADORCPF = string.IsNullOrWhiteSpace(value) ? "" : Util.DesformatarCNPJCPF(value); }
        }
        [StringLength(2)]
        public string PRESTADORUF { get; set; }
        [StringLength(50)]
        public string INSCRICAO { get; set; }
        [StringLength(10)]
        public string RETENCAOISS { get; set; }
        [StringLength(200)]
        public string PRESTADORNOME { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORSERVICO { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORDEDUCOES { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORPIS { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORCOFINS { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORINSS { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORIR { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORCSLL { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ISSRETIDO { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORISS { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OUTRASRETENCOES { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BASECALCULO { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ALIQUOTA { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORLIQUIDO { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALOR_DESCONTO { get; set; }
        public int? LINHA_OC { get; set; }
        [StringLength(50)]
        public string NUMERO_OC { get; set; }
        [ForeignKey("RFE_REPOSITORIO")]
        public int? IDXML { get; set; }
        [ForeignKey("IDXML")]
        public virtual RFE_REPOSITORIO RFE_REPOSITORIO { get; set; }
        public bool? NOTA_DIVERSA { get; set; }

        [StringLength(15)]
        public string TIPONFE { get; set; }
        [StringLength(25)]
        public string UTILIZACAOFISCAL { get; set; }
        [StringLength(4)]
        public string CFOPENTRADA { get; set; }
        [StringLength(60)]
        public string XPED { get; set; }
        [StringLength(60)]
        public string NITEMPED { get; set; }
        public virtual List<RFE_ITEM_NFSE> Itens { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VALORTOTAL { get; set; }

        [StringLength(100)]
        public string MUNICIPIOGERADOR { get; set; }
        public string ATIVIDADE { get; set; }
        public string OUTRASINFORMACOES { get; set; }
        public string DESCRICAO_TIPO_SERVICO { get; set; }

        public string LOCAL_PRESTACAO { get; set; }
        public string UF_PRESTACAO { get; set; }
        public string COD_IBGE_PRESTACAO { get; set; }

        public DateTime? DT_ENTRADA { get; set; }
        public DateTime? DT_ENTRADA_REPOSIT { get; set; }
        public DateTimeOffset? DT_VENCIMENTO { get; set; }
        public DateTimeOffset? DT_PREENCHIMENTO_FINALIZADO { get; set; }

        //CAMPOS DE CONTROLE DE INTEGRAÇÃO
        public TipoIntegracao? INTEGRACAO_TIPO_AUX { get; set; }
        public TipoIntegracao? INTEGRACAO_TIPO { get; set; }
        public StatusIntegracaoERP? STATUS_INTEGRACAO_ERP { get; set; }

        public DateTime? DataEnvioERP { get; set; }
        public DateTime? DataUltimaLeituraRetornoERP { get; set; }
        public ExecutarMetodo? EXECUTAR_METODO { get; set; }

        [StringLength(1000)]
        public string SEARCH_OC_JSON { get; set; }

        public TipoCaixaEmail? TIPO_CAIXA_ORIGEM { get; set; }
        [StringLength(255)]
        public string NOME_ORIGINAL_ARQ_CONVERSAO { get; set; }
        public bool? INTERMEDIACAO { get; set; }

        public int? ID_ANEXOS_NFSE { get; set; }
        public StatusProtocolo? STATUS_PROTOCOLO { get; set; }

        [StringLength(200)]
        public string SOLICITANTE_AREA { get; set; }
        [StringLength(200)]
        public string SOLICITANTE_NOME { get; set; }
        [StringLength(200)]
        public string UPLOAD_AREA { get; set; }
        [StringLength(200)]
        public string UPLOAD_NOME { get; set; }
        [StringLength(200)]
        public string FATURA_NDDFRETE { get; set; }

    }
}
