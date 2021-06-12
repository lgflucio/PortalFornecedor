using System.ComponentModel.DataAnnotations;
using static Shared.Enums.Enums;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_LOG : BASE_TABLE
    {

        [Required]
        public Servico Servico { get; set; }
        [ClobType]
        [Required]
        public string Mensagem { get; set; }
        public TipoLog TipoLog { get; set; }
    }
}
