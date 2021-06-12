using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Shared.Enums.Enums;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_PORTAL_HEADER_NOTA : BASE_TABLE
    {
        public RFE_PORTAL_HEADER_NOTA()
        {
            ITENS_NOTA = new List<RFE_PORTAL_ITEM_NOTA>();
        }
        [StringLength(28)]
        public string CNPJ_FORNECEDOR { get; set; }
        [StringLength(40)]
        public string PROTOCOLO { get; set; }
        public long NUMERO_NOTA { get; set; }
        [StringLength(10)]
        public string SERIE { get; set; }
        [StringLength(255)]
        public string FILE_NAME { get; set; }
        [StringLength(7)]
        public string FILE_EXTENSION { get; set; }
        public int ID_ANEXO_NFSE { get; set; }
        public bool FLG_CANCELADO { get; set; }
        [StringLength(1000)]
        public string MOTIVO_CANCELAMENTO { get; set; }
        public StatusCard STATUS_CARD { get; set; }
        public StatusProtocolo STATUS_PROTOCOLO { get; set; }
        public DateTimeOffset DT_UPLOAD { get; set; }
        public StatusExportacaoPortal STATUS_EXPORTACAO { get; set; }

        public virtual List<RFE_PORTAL_ITEM_NOTA> ITENS_NOTA { get; set; }
        [NotMapped]
        public virtual RFE_ANEXOS_NFSE ANEXO_NFSE { get; set; }

        public DateTimeOffset? DT_EMISSAO { get; set; }
        public DateTimeOffset? DT_VENCIMENTO { get; set; }
        public DateTimeOffset? DT_PREENCHIMENTO_FINALIZADO { get; set; }

        [StringLength(2)]
        public string UF_PRESTACAO { get; set; }
        [StringLength(200)]
        public string MUN_PRESTACAO { get; set; }
        [StringLength(20)]
        public string COD_IBGE_PRESTACAO { get; set; }

        [StringLength(200)]
        public string SOLICITANTE_AREA { get; set; }
        [StringLength(200)]
        public string SOLICITANTE_NOME { get; set; }
        [StringLength(200)]
        public string UPLOAD_AREA { get; set; }
        [StringLength(200)]
        public string UPLOAD_NOME { get; set; }
    }
}
