using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Shared.Enums.Enums;

namespace Repository.Entities.RFE_ENTITIES
{
    public class RFE_HISTORICO : BASE_TABLE
    {
        [Required]
        [StringLength(500)]
        public string DESCRICAO { get; set; }
        [ForeignKey("REPOSITORIO")]
        public int? ID_REPOSITORIO { get; set; }
        public virtual RFE_REPOSITORIO REPOSITORIO { get; set; }
        public EHistorico? TIPO { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is RFE_HISTORICO) && (((RFE_HISTORICO)obj).DESCRICAO == DESCRICAO && ((RFE_HISTORICO)obj).ID_REPOSITORIO == ID_REPOSITORIO);
        }

        public override int GetHashCode()
        {
            return ID_REPOSITORIO.GetHashCode() + DESCRICAO.GetHashCode();
        }
    }
}
