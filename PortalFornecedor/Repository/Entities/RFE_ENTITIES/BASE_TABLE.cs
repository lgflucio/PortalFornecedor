using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities.RFE_ENTITIES
{
    public class BASE_TABLE
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string EDITOR { get; set; }
        [Required]
        public DateTime MODIFICADO { get; set; }
    }
}
