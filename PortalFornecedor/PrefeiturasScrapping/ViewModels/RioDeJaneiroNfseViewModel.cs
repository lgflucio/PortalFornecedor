using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PrefeiturasScrapping.ViewModels
{
    public class RioDeJaneiroNfseViewModel
    {
        public RioDeJaneiroNfseViewModel()
        {
            ConsultarNfseResposta = new ConsultarNfseResposta();
        }
        public ConsultarNfseResposta ConsultarNfseResposta { get; set; }
    }

    public class InfNfse
    {
        public InfNfse()
        {
            IdentificacaoRps = new IdentificacaoRps();
            Servico = new Servico();
            PrestadorServico = new PrestadorServico();
            TomadorServico = new TomadorServico();
            OrgaoGerador = new OrgaoGerador();
        }
        public string Numero { get; set; }
        public string CodigoVerificacao { get; set; }
        public string DataEmissao { get; set; }
        public string DataEmissaoRps { get; set; }
        public string NaturezaOperacao { get; set; }
        public string OptanteSimplesNacional { get; set; }
        public string IncentivadorCultural { get; set; }
        public string Competencia { get; set; }
        public IdentificacaoRps IdentificacaoRps { get; set; }
        public Servico Servico { get; set; }
        public PrestadorServico PrestadorServico { get; set; }
        public TomadorServico TomadorServico { get; set; }
        public OrgaoGerador OrgaoGerador { get; set; }

    }
    public class IdentificacaoRps
    {
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string Tipo { get; set; }
    }
    public class Servico
    {
        public Servico()
        {
            Valores = new Valores();
        }
        public Valores Valores { get; set; }
        public string ItemListaServico { get; set; }
        public string CodigoTributacaoMunicipio { get; set; }
        public string Discriminacao { get; set; }
        public string CodigoMunicipio { get; set; }
    }
    public class Valores
    {
        public string ValorServicos { get; set; }
        public string ValorPis { get; set; }
        public string ValorCofins { get; set; }
        public string ValorInss { get; set; }
        public string ValorIr { get; set; }
        public string ValorCsll { get; set; }
        public string IssRetido { get; set; }
        public string ValorIss { get; set; }
        public string OutrasRetencoes { get; set; }
        public string Aliquota { get; set; }
        public string DescontoIncondicional { get; set; }
        public string DescontoCondicionado { get; set; }
        public string ValorLiquidoNfse { get; set; }
    }

    public class PrestadorServico
    {
        public PrestadorServico()
        {
            IdentificacaoPrestador = new IdentificacaoPrestador();
            Contato = new Contato();
            Endereco = new Endereco();
        }
        public IdentificacaoPrestador IdentificacaoPrestador { get; set; }
        public Contato Contato { get; set; }
        public Endereco Endereco { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
    }

    public class IdentificacaoPrestador
    {
        public string Cnpj { get; set; }
        public string InscricaoMunicipal { get; set; }
    }
    public class Endereco
    {
        public string endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CodigoMunicipio { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
    }
    public class Contato
    {
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
    public class TomadorServico
    {
        public TomadorServico()
        {
            IdentificacaoTomador = new IdentificacaoTomador();
            Endereco = new Endereco();
            Contato = new Contato();
        }
        public IdentificacaoTomador IdentificacaoTomador { get; set; }
        public Endereco Endereco { get; set; }
        public Contato Contato { get; set; }
        public string RazaoSocial { get; set; }

    }
    public class IdentificacaoTomador
    {
        public IdentificacaoTomador()
        {
            CpfCnpj = new CpfCnpj();
        }
        public CpfCnpj CpfCnpj { get; set; }
    }
    public class CpfCnpj
    {
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
    }

    public class OrgaoGerador
    {
        public string CodigoMunicipio { get; set; }
        public string Uf { get; set; }
    }

    #region headers xml
    public class ConsultarNfseResposta
    {
        public ConsultarNfseResposta()
        {
            ListaNfse = new ListaNfse();
        }
        public ListaNfse ListaNfse { get; set; }
    }
    public class ListaNfse
    {
        public ListaNfse()
        {
            CompNfse = new CompNfse();
        }
        public CompNfse CompNfse { get; set; }
    }
    public class CompNfse
    {
        public CompNfse()
        {
            Nfse = new Nfse();
        }
        public Nfse Nfse { get; set; }
    }
    public class Nfse
    {
        public Nfse()
        {
            InfNfse = new InfNfse();
        }
        public InfNfse InfNfse { get; set; }
    }

    #endregion

}
