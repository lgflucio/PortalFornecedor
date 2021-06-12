using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities.PREFEITURAS_ENTITIES
{
    public class NotasServicos : Entity
    {
        public NotasServicos()
        {
            MesCompetencia = DataEmissao.ToString("MM/yyyy");
            Itens = new List<ItensNfse>();
        }
        public string MunicipioGerador { get; set; }
        public string Chave { get; set; }
        public DateTime DataEmissao { get; set; }
        public long Numero { get; set; }
        public string Cnae { get; set; }
        public string CodigoVerificacao { get; set; }
        public string Serie { get; set; }
        public string RpsNumero { get; set; }
        public string RpsSerie { get; set; }
        public string RpsTipo { get; set; }
        public DateTime? RpsDataEmissao { get; set; }
        public string NaturezaOperacao { get; set; }
        public string RegimeEspecialTributacao { get; set; }
        public string OutrasInformacoes { get; set; }
        public string PrestadorInscricaoMunicipal { get; set; }
        public string PrestadorInscricaoEstadual { get; set; }
        public string PrestadorCnpj { get; set; }
        public string PrestadorCpf { get; set; }
        public string PrestadorRazaoSocial { get; set; }
        public string PrestadorEndereco { get; set; }
        public string PrestadorNumero { get; set; }
        public string PrestadorComplemento { get; set; }
        public string PrestadorBairro { get; set; }
        public string PrestadorMunicipio { get; set; }
        public string PrestadorUf { get; set; }
        public string PrestadorCep { get; set; }
        public string PrestadorNome { get; set; }
        public string Tributacao { get; set; }
        public string CodigoTributacao { get; set; }
        public string OptanteSimplesNacional { get; set; }
        public string NumeroGuia { get; set; }
        public DateTime? DataPagamento { get; set; }
        public decimal? ValorServicos { get; set; }
        public decimal? ValorAliquota { get; set; }
        public decimal? ValorCredito { get; set; }
        public decimal? Valortotal { get; set; }
        public decimal? ValorCofins { get; set; }
        public decimal? ValorCsll { get; set; }
        public decimal? ValorIss { get; set; }
        public decimal? ValorIr { get; set; }
        public decimal? ValorPis { get; set; }
        public decimal? ValorIssRetido { get; set; }
        public decimal? ValorLiquido { get; set; }
        public decimal? ValorOutrasRetencoes { get; set; }
        public decimal? ValorRetencoesFederais { get; set; }
        public decimal? ValorDeducoes { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? BaseCalculo { get; set; }          
        public string MesCompetencia { get; set; }
        public string CodigoServico { get; set; }
        public string TomadorCnpj { get; set; }
        public string TomadorCpf { get; set; }
        public string TomadorInscricaoMunicipal { get; set; }
        public string TomadorInscricaoEstadual { get; set; }
        public string TomadorRazaoSocial { get; set; }
        public string TomadorEndereco { get; set; }
        public string TomadorNumero { get; set; }
        public string TomadorBairro { get; set; }
        public string TomadorMunicipio { get; set; }
        public string TomadorUf { get; set; }
        public string TomadorCep { get; set; }
        public string DiscriminacaoServico { get; set; }
        public string DescricaoTipoServico { get; set; }
        public List<ItensNfse> Itens { get; set; }

        [ForeignKey("Container")]
        public int ContainerId { get; set; }
        public virtual ContainerNotas Container { get; set; }
    }
}
