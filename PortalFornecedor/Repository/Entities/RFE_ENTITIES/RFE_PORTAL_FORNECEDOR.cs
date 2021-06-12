using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Shared.Enums.Enums;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_PORTAL_FORNECEDOR : BASE_TABLE
    {
        [StringLength(18)]
        public string CNPJ { get; set; }
        [StringLength(500)]
        public string RAZAO { get; set; }
        public OpcaoTributaria OPCAO_TRIBUTARIA { get; set; }
        [StringLength(2)]
        public string ESTADO { get; set; }
        [StringLength(200)]
        public string MUNICIPIO { get; set; }
        [StringLength(200)]
        public string SENHA { get; set; }
        [NotMapped]
        [StringLength(200)]
        public virtual string CONFIRMAR_SENHA { get; set; }
        [NotMapped]
        [StringLength(200)]
        public virtual string SENHA_ATUAL { get; set; }
        public bool BLOQUEADO { get; set; } = false;
        public PF_StatusFornecedor STATUS { get; set; }
        [StringLength(300)]
        public string EMAIL { get; set; }
        [StringLength(300)]
        public string NOVO_EMAIL { get; set; }
        public bool EMAIL_VALIDADO { get; set; } = false;
        public DateTime? DATA_ULTIMO_ACESSO { get; set; }

    }
}
