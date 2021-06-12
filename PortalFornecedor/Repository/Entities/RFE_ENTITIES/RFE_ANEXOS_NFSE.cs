using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_ANEXOS_NFSE : BASE_TABLE
    {
        [StringLength(255)]
        public string FILE_NAME { get; set; }
        [StringLength(7)]
        public string FILE_EXTENSION { get; set; }
        [StringLength(40)]
        public string CONTENT_TYPE { get; set; }
        public long? FILE_SIZE { get; set; }
        [FileType]
        public string FILE_BASE64 { get; set; }

    }
}
