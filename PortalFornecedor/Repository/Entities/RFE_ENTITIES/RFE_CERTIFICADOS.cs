using System.ComponentModel.DataAnnotations;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_CERTIFICADOS : BASE_TABLE
    {
        [Required]
        [StringLength(14)]
        public string CNPJ { get; set; }
        [Required]
        [StringLength(200)]
        public string CERTIFICADO { get; set; }
        [Required]
        [StringLength(100)]
        public string SENHA { get; set; }
    }
}
