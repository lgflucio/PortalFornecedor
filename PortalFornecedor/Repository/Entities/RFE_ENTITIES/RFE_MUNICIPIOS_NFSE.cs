using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_MUNICIPIOS_NFSE : BASE_TABLE
    {
        [Required]
        [StringLength(14)]
        public string MUNICIPIO { get; set; }
        [ForeignKey("TIPO")]
        public int ID_TIPO { get; set; }
        public virtual RFE_TIPOS_XML TIPO { get; set; }
    }
}
