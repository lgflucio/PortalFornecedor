using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_TAGS : BASE_TABLE
    {
        [StringLength(50)]
        public string NOME { get; set; }
        [StringLength(50)]
        public string TAG { get; set; }
        [StringLength(50)]
        public string TAG_PAI { get; set; }
        [ForeignKey("TIPO")]
        public int? ID_TIPO { get; set; }
        public virtual RFE_TIPOS_XML TIPO { get; set; }
    }
}
