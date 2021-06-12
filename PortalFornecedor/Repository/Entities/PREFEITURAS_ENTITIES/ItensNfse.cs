namespace Repository.Entities.PREFEITURAS_ENTITIES
{
    public class ItensNfse : Entity
    {
        public ItensNfse()
        {
            Tributavel = false;
        }

        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? ValorDesconto { get; set; }
        public bool Tributavel { get; set; }
        public int NumeroItem { get; set; }
        public string CodigoServico { get; set; }
        public int NfseId { get; set; }
        public virtual NotasServicos Nfse { get; set; }
    }
}