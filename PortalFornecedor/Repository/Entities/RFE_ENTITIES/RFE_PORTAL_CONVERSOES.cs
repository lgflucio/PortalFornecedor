using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Shared.Enums.Enums;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_PORTAL_CONVERSOES : BASE_TABLE
    {
        public string PROTOCOLO { get; set; }

        [ForeignKey("HEADER_NOTA")]
        public int ID_NFSE { get; set; }
        public RFE_PORTAL_HEADER_NOTA HEADER_NOTA { get; set; }
        public StatusConversao STATUS { get; set; }

    }
}
