using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_PORTAL_ITEM_NOTA : BASE_TABLE
    {
        [ForeignKey("HEADER_NOTA")]
        public int ID_HEADER { get; set; }
        public int NUMERO_ITEM { get; set; }
        [StringLength(50)]
        public string NUMERO_OC { get; set; }
        public int? LINHA_OC { get; set; }
        [StringLength(50)]
        public string NUMERO_FOLHA_MEDICAO { get; set; }
        public virtual RFE_PORTAL_HEADER_NOTA HEADER_NOTA { get; set; }
    }
}
