using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities.PREFEITURAS_ENTITIES
{
    public class Entity
    {

        public Entity()
        {
            DataCriacao = DateTime.Now;
            IsDeleted = false;
            Editor = "Serviço De Busca Automatica NFSE";
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Editor { get; set; }
        [Required]
        public DateTime? Modificado { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool IsDeleted { get; set; }

    }
}
