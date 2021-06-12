using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_TIPOS_XML : BASE_TABLE
    {
        [StringLength(100)]
        public string DESCRICAO { get; set; }

        public virtual ICollection<RFE_TAGS> Tags { get; set; }
    }
}
