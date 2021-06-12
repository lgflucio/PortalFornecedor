using System;
using System.Collections.Generic;
using System.Text;

namespace PrefeiturasWebServices.ViewModels.Chapeco
{

    // OBSERVAÇÃO: o código gerado pode exigir pelo menos .NET Framework 4.5 ou .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.publica.inf.br", IsNullable = false, ElementName = "ConsultaNfseRecebidaResposta")]
    public partial class ChapecoViewModel
    {

        private CompNfse[] listaNfseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(ElementName = "CompNfse", IsNullable = false)]
        public CompNfse[] ListaNfse
        {
            get
            {
                return this.listaNfseField;
            }
            set
            {
                this.listaNfseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class CompNfse
    {

        private CompNfseNfse nfseField;

        private CompNfseNfseCancelamento nfseCancelamentoField;

        private CompNfseNfseSubstituicao nfseSubstituicaoField;

        private CompNfseCartaCorrecao[] listaCorrecaoField;

        /// <remarks/>
        public CompNfseNfse Nfse
        {
            get
            {
                return this.nfseField;
            }
            set
            {
                this.nfseField = value;
            }
        }

        /// <remarks/>
        public CompNfseNfseCancelamento NfseCancelamento
        {
            get
            {
                return this.nfseCancelamentoField;
            }
            set
            {
                this.nfseCancelamentoField = value;
            }
        }

        /// <remarks/>
        public CompNfseNfseSubstituicao NfseSubstituicao
        {
            get
            {
                return this.nfseSubstituicaoField;
            }
            set
            {
                this.nfseSubstituicaoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CartaCorrecao", IsNullable = false)]
        public CompNfseCartaCorrecao[] ListaCorrecao
        {
            get
            {
                return this.listaCorrecaoField;
            }
            set
            {
                this.listaCorrecaoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class CompNfseNfse
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse infNfseField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse InfNfse
        {
            get
            {
                return this.infNfseField;
            }
            set
            {
                this.infNfseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse
    {

        private string numeroField;

        private string serieField;

        private string codigoVerificacaoField;

        private System.DateTime dataEmissaoField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIdentificacaoRps identificacaoRpsField;

        private System.DateTime dataEmissaoRpsField;

        private int naturezaOperacaoField;

        private string optanteSimplesNacionalField;

        private string incentivadorCulturalField;

        private string competenciaField;

        private string nfseSubstituidaField;

        private string outrasInformacoesField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseServico servicoField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServico prestadorServicoField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServico tomadorServicoField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIntermediarioServico intermediarioServicoField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseOrgaoGerador orgaoGeradorField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseParcelas[] condicaoPagamentoField;

        private string linkVisualizacaoNfseField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "string")]
        public string Numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        public string Serie
        {
            get
            {
                return this.serieField;
            }
            set
            {
                this.serieField = value;
            }
        }

        /// <remarks/>
        public string CodigoVerificacao
        {
            get
            {
                return this.codigoVerificacaoField;
            }
            set
            {
                this.codigoVerificacaoField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DataEmissao
        {
            get
            {
                return this.dataEmissaoField;
            }
            set
            {
                this.dataEmissaoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIdentificacaoRps IdentificacaoRps
        {
            get
            {
                return this.identificacaoRpsField;
            }
            set
            {
                this.identificacaoRpsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime DataEmissaoRps
        {
            get
            {
                return this.dataEmissaoRpsField;
            }
            set
            {
                this.dataEmissaoRpsField = value;
            }
        }

        /// <remarks/>
        public int NaturezaOperacao
        {
            get
            {
                return this.naturezaOperacaoField;
            }
            set
            {
                this.naturezaOperacaoField = value;
            }
        }

        /// <remarks/>
        public string OptanteSimplesNacional
        {
            get
            {
                return this.optanteSimplesNacionalField;
            }
            set
            {
                this.optanteSimplesNacionalField = value;
            }
        }

        /// <remarks/>
        public string IncentivadorCultural
        {
            get
            {
                return this.incentivadorCulturalField;
            }
            set
            {
                this.incentivadorCulturalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "gYearMonth")]
        public string Competencia
        {
            get
            {
                return this.competenciaField;
            }
            set
            {
                this.competenciaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "string")]
        public string NfseSubstituida
        {
            get
            {
                return this.nfseSubstituidaField;
            }
            set
            {
                this.nfseSubstituidaField = value;
            }
        }

        /// <remarks/>
        public string OutrasInformacoes
        {
            get
            {
                return this.outrasInformacoesField;
            }
            set
            {
                this.outrasInformacoesField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseServico Servico
        {
            get
            {
                return this.servicoField;
            }
            set
            {
                this.servicoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServico PrestadorServico
        {
            get
            {
                return this.prestadorServicoField;
            }
            set
            {
                this.prestadorServicoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServico TomadorServico
        {
            get
            {
                return this.tomadorServicoField;
            }
            set
            {
                this.tomadorServicoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIntermediarioServico IntermediarioServico
        {
            get
            {
                return this.intermediarioServicoField;
            }
            set
            {
                this.intermediarioServicoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseOrgaoGerador OrgaoGerador
        {
            get
            {
                return this.orgaoGeradorField;
            }
            set
            {
                this.orgaoGeradorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Parcelas", IsNullable = false)]
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseParcelas[] CondicaoPagamento
        {
            get
            {
                return this.condicaoPagamentoField;
            }
            set
            {
                this.condicaoPagamentoField = value;
            }
        }

        /// <remarks/>
        public string LinkVisualizacaoNfse
        {
            get
            {
                return this.linkVisualizacaoNfseField;
            }
            set
            {
                this.linkVisualizacaoNfseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIdentificacaoRps
    {

        private long numeroField;

        private string serieField;

        private int tipoField;

        /// <remarks/>
        public long Numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        public string Serie
        {
            get
            {
                return this.serieField;
            }
            set
            {
                this.serieField = value;
            }
        }

        /// <remarks/>
        public int Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseServico
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseServicoValores valoresField;

        private string itemListaServicoField;

        private string discriminacaoField;

        private string informacoesComplementaresField;

        private int codigoMunicipioField;

        private string codigoPaisField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseServicoValores Valores
        {
            get
            {
                return this.valoresField;
            }
            set
            {
                this.valoresField = value;
            }
        }

        /// <remarks/>
        public string ItemListaServico
        {
            get
            {
                return this.itemListaServicoField;
            }
            set
            {
                this.itemListaServicoField = value;
            }
        }

        /// <remarks/>
        public string Discriminacao
        {
            get
            {
                return this.discriminacaoField;
            }
            set
            {
                this.discriminacaoField = value;
            }
        }

        /// <remarks/>
        public string InformacoesComplementares
        {
            get
            {
                return this.informacoesComplementaresField;
            }
            set
            {
                this.informacoesComplementaresField = value;
            }
        }

        /// <remarks/>
        public int CodigoMunicipio
        {
            get
            {
                return this.codigoMunicipioField;
            }
            set
            {
                this.codigoMunicipioField = value;
            }
        }

        /// <remarks/>
        public string CodigoPais
        {
            get
            {
                return this.codigoPaisField;
            }
            set
            {
                this.codigoPaisField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseServicoValores
    {

        private decimal valorServicosField;

        private decimal valorDeducoesField;

        private decimal valorPisField;

        private decimal valorCofinsField;

        private decimal valorInssField;

        private decimal valorIrField;

        private decimal valorCsllField;

        private decimal issRetidoField;

        private decimal valorIssField;

        private decimal valorIssRetidoField;

        private decimal outrasRetencoesField;

        private decimal baseCalculoField;

        private decimal aliquotaField;

        private decimal valorLiquidoNfseField;

        private decimal descontoIncondicionadoField;

        private decimal descontoCondicionadoField;

        /// <remarks/>
        public decimal ValorServicos
        {
            get
            {
                return this.valorServicosField;
            }
            set
            {
                this.valorServicosField = value;
            }
        }

        /// <remarks/>
        public decimal ValorDeducoes
        {
            get
            {
                return this.valorDeducoesField;
            }
            set
            {
                this.valorDeducoesField = value;
            }
        }

        /// <remarks/>
        public decimal ValorPis
        {
            get
            {
                return this.valorPisField;
            }
            set
            {
                this.valorPisField = value;
            }
        }

        /// <remarks/>
        public decimal ValorCofins
        {
            get
            {
                return this.valorCofinsField;
            }
            set
            {
                this.valorCofinsField = value;
            }
        }

        /// <remarks/>
        public decimal ValorInss
        {
            get
            {
                return this.valorInssField;
            }
            set
            {
                this.valorInssField = value;
            }
        }

        /// <remarks/>
        public decimal ValorIr
        {
            get
            {
                return this.valorIrField;
            }
            set
            {
                this.valorIrField = value;
            }
        }

        /// <remarks/>
        public decimal ValorCsll
        {
            get
            {
                return this.valorCsllField;
            }
            set
            {
                this.valorCsllField = value;
            }
        }

        /// <remarks/>
        public decimal IssRetido
        {
            get
            {
                return this.issRetidoField;
            }
            set
            {
                this.issRetidoField = value;
            }
        }

        /// <remarks/>
        public decimal ValorIss
        {
            get
            {
                return this.valorIssField;
            }
            set
            {
                this.valorIssField = value;
            }
        }

        /// <remarks/>
        public decimal ValorIssRetido
        {
            get
            {
                return this.valorIssRetidoField;
            }
            set
            {
                this.valorIssRetidoField = value;
            }
        }

        /// <remarks/>
        public decimal OutrasRetencoes
        {
            get
            {
                return this.outrasRetencoesField;
            }
            set
            {
                this.outrasRetencoesField = value;
            }
        }

        /// <remarks/>
        public decimal BaseCalculo
        {
            get
            {
                return this.baseCalculoField;
            }
            set
            {
                this.baseCalculoField = value;
            }
        }

        /// <remarks/>
        public decimal Aliquota
        {
            get
            {
                return this.aliquotaField;
            }
            set
            {
                this.aliquotaField = value;
            }
        }

        /// <remarks/>
        public decimal ValorLiquidoNfse
        {
            get
            {
                return this.valorLiquidoNfseField;
            }
            set
            {
                this.valorLiquidoNfseField = value;
            }
        }

        /// <remarks/>
        public decimal DescontoIncondicionado
        {
            get
            {
                return this.descontoIncondicionadoField;
            }
            set
            {
                this.descontoIncondicionadoField = value;
            }
        }

        /// <remarks/>
        public decimal DescontoCondicionado
        {
            get
            {
                return this.descontoCondicionadoField;
            }
            set
            {
                this.descontoCondicionadoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServico
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoIdentificacaoPrestador identificacaoPrestadorField;

        private string razaoSocialField;

        private string nomeFantasiaField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoEndereco enderecoField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoContato contatoField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoIdentificacaoPrestador IdentificacaoPrestador
        {
            get
            {
                return this.identificacaoPrestadorField;
            }
            set
            {
                this.identificacaoPrestadorField = value;
            }
        }

        /// <remarks/>
        public string RazaoSocial
        {
            get
            {
                return this.razaoSocialField;
            }
            set
            {
                this.razaoSocialField = value;
            }
        }

        /// <remarks/>
        public string NomeFantasia
        {
            get
            {
                return this.nomeFantasiaField;
            }
            set
            {
                this.nomeFantasiaField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoEndereco Endereco
        {
            get
            {
                return this.enderecoField;
            }
            set
            {
                this.enderecoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoContato Contato
        {
            get
            {
                return this.contatoField;
            }
            set
            {
                this.contatoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoIdentificacaoPrestador
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoIdentificacaoPrestadorCpfCnpj cpfCnpjField;

        private string cpfField;

        private string cnpjField;

        private string inscricaoMunicipalField;

        private string idField;

        public string Cpf
        {
            get
            {
                return this.cpfField;
            }
            set
            {
                this.cpfField = value;
            }
        }
        public string Cnpj
        {
            get
            {
                return this.cnpjField;
            }
            set
            {
                this.cnpjField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoIdentificacaoPrestadorCpfCnpj CpfCnpj
        {
            get
            {
                return this.cpfCnpjField;
            }
            set
            {
                this.cpfCnpjField = value;
            }
        }

        /// <remarks/>
        public string InscricaoMunicipal
        {
            get
            {
                return this.inscricaoMunicipalField;
            }
            set
            {
                this.inscricaoMunicipalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoIdentificacaoPrestadorCpfCnpj
    {
        private string cpfField;
        private string cnpjField;
        private string inscricaoMunicipalField;
        /// <remarks/>
        public string Cpf
        {
            get
            {
                return this.cpfField;
            }
            set
            {
                this.cpfField = value;
            }
        }
        public string Cnpj
        {
            get
            {
                return this.cnpjField;
            }
            set
            {
                this.cnpjField = value;
            }
        }
        public string InscricaoMunicipal
        {
            get
            {
                return this.inscricaoMunicipalField;
            }
            set
            {
                this.inscricaoMunicipalField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoEndereco
    {

        private string enderecoField;

        private string numeroField;

        private string complementoField;

        private string bairroField;

        private long codigoMunicipioField;

        private string ufField;

        private string cepField;

        private string codigoPaisField;

        private string municipioField;

        /// <remarks/>
        public string Endereco
        {
            get
            {
                return this.enderecoField;
            }
            set
            {
                this.enderecoField = value;
            }
        }

        /// <remarks/>
        public string Numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        public string Complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;
            }
        }

        /// <remarks/>
        public string Bairro
        {
            get
            {
                return this.bairroField;
            }
            set
            {
                this.bairroField = value;
            }
        }

        /// <remarks/>
        public long CodigoMunicipio
        {
            get
            {
                return this.codigoMunicipioField;
            }
            set
            {
                this.codigoMunicipioField = value;
            }
        }

        /// <remarks/>
        public string Uf
        {
            get
            {
                return this.ufField;
            }
            set
            {
                this.ufField = value;
            }
        }

        /// <remarks/>
        public string Cep
        {
            get
            {
                return this.cepField;
            }
            set
            {
                this.cepField = value;
            }
        }

        /// <remarks/>
        public string CodigoPais
        {
            get
            {
                return this.codigoPaisField;
            }
            set
            {
                this.codigoPaisField = value;
            }
        }

        /// <remarks/>
        public string Municipio
        {
            get
            {
                return this.municipioField;
            }
            set
            {
                this.municipioField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfsePrestadorServicoContato
    {

        private string telefoneField;

        private string emailField;

        /// <remarks/>
        public string Telefone
        {
            get
            {
                return this.telefoneField;
            }
            set
            {
                this.telefoneField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServico
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoIdentificacaoTomador identificacaoTomadorField;

        private string razaoSocialField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoEndereco enderecoField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoContato contatoField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoIdentificacaoTomador IdentificacaoTomador
        {
            get
            {
                return this.identificacaoTomadorField;
            }
            set
            {
                this.identificacaoTomadorField = value;
            }
        }

        /// <remarks/>
        public string RazaoSocial
        {
            get
            {
                return this.razaoSocialField;
            }
            set
            {
                this.razaoSocialField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoEndereco Endereco
        {
            get
            {
                return this.enderecoField;
            }
            set
            {
                this.enderecoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoContato Contato
        {
            get
            {
                return this.contatoField;
            }
            set
            {
                this.contatoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoIdentificacaoTomador
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoIdentificacaoTomadorCpfCnpj cpfCnpjField;

        private string inscricaoMunicipalField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoIdentificacaoTomadorCpfCnpj CpfCnpj
        {
            get
            {
                return this.cpfCnpjField;
            }
            set
            {
                this.cpfCnpjField = value;
            }
        }

        /// <remarks/>
        public string InscricaoMunicipal
        {
            get
            {
                return this.inscricaoMunicipalField;
            }
            set
            {
                this.inscricaoMunicipalField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoIdentificacaoTomadorCpfCnpj
    {

        private string cpfField;
        private string cnpjField;
        private string inscricaoMunicipalField;
        /// <remarks/>
        public string Cpf
        {
            get
            {
                return this.cpfField;
            }
            set
            {
                this.cpfField = value;
            }
        }
        public string Cnpj
        {
            get
            {
                return this.cnpjField;
            }
            set
            {
                this.cnpjField = value;
            }
        }
        public string InscricaoMunicipal
        {
            get
            {
                return this.inscricaoMunicipalField;
            }
            set
            {
                this.inscricaoMunicipalField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoEndereco
    {

        private string enderecoField;

        private string numeroField;

        private string complementoField;

        private string bairroField;

        private string codigoMunicipioField;

        private string ufField;

        private string cepField;

        private string codigoPaisField;

        private string municipioField;

        /// <remarks/>
        public string Endereco
        {
            get
            {
                return this.enderecoField;
            }
            set
            {
                this.enderecoField = value;
            }
        }

        /// <remarks/>
        public string Numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        public string Complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;
            }
        }

        /// <remarks/>
        public string Bairro
        {
            get
            {
                return this.bairroField;
            }
            set
            {
                this.bairroField = value;
            }
        }

        /// <remarks/>
        public string CodigoMunicipio
        {
            get
            {
                return this.codigoMunicipioField;
            }
            set
            {
                this.codigoMunicipioField = value;
            }
        }

        /// <remarks/>
        public string Uf
        {
            get
            {
                return this.ufField;
            }
            set
            {
                this.ufField = value;
            }
        }

        /// <remarks/>
        public string Cep
        {
            get
            {
                return this.cepField;
            }
            set
            {
                this.cepField = value;
            }
        }

        /// <remarks/>
        public string CodigoPais
        {
            get
            {
                return this.codigoPaisField;
            }
            set
            {
                this.codigoPaisField = value;
            }
        }

        /// <remarks/>
        public string Municipio
        {
            get
            {
                return this.municipioField;
            }
            set
            {
                this.municipioField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseTomadorServicoContato
    {

        private string telefoneField;

        private string emailField;

        /// <remarks/>
        public string Telefone
        {
            get
            {
                return this.telefoneField;
            }
            set
            {
                this.telefoneField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIntermediarioServico
    {

        private string razaoSocialField;

        private ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIntermediarioServicoCpfCnpj cpfCnpjField;

        private string inscricaoMunicipalField;

        /// <remarks/>
        public string RazaoSocial
        {
            get
            {
                return this.razaoSocialField;
            }
            set
            {
                this.razaoSocialField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIntermediarioServicoCpfCnpj CpfCnpj
        {
            get
            {
                return this.cpfCnpjField;
            }
            set
            {
                this.cpfCnpjField = value;
            }
        }

        /// <remarks/>
        public string InscricaoMunicipal
        {
            get
            {
                return this.inscricaoMunicipalField;
            }
            set
            {
                this.inscricaoMunicipalField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseIntermediarioServicoCpfCnpj
    {

        private string cpfField;

        /// <remarks/>
        public string Cpf
        {
            get
            {
                return this.cpfField;
            }
            set
            {
                this.cpfField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseOrgaoGerador
    {

        private int codigoMunicipioField;

        private string ufField;

        /// <remarks/>
        public int CodigoMunicipio
        {
            get
            {
                return this.codigoMunicipioField;
            }
            set
            {
                this.codigoMunicipioField = value;
            }
        }

        /// <remarks/>
        public string Uf
        {
            get
            {
                return this.ufField;
            }
            set
            {
                this.ufField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseInfNfseParcelas
    {

        private int condicaoField;

        private int parcelaField;

        private decimal valorField;

        private System.DateTime dataVencimentoField;

        /// <remarks/>
        public int Condicao
        {
            get
            {
                return this.condicaoField;
            }
            set
            {
                this.condicaoField = value;
            }
        }

        /// <remarks/>
        public int Parcela
        {
            get
            {
                return this.parcelaField;
            }
            set
            {
                this.parcelaField = value;
            }
        }

        /// <remarks/>
        public decimal Valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime DataVencimento
        {
            get
            {
                return this.dataVencimentoField;
            }
            set
            {
                this.dataVencimentoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class CompNfseNfseCancelamento
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacao confirmacaoField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacao Confirmacao
        {
            get
            {
                return this.confirmacaoField;
            }
            set
            {
                this.confirmacaoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacao
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedido pedidoField;

        private System.DateTime dataHoraCancelamentoField;

        private string idField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedido Pedido
        {
            get
            {
                return this.pedidoField;
            }
            set
            {
                this.pedidoField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DataHoraCancelamento
        {
            get
            {
                return this.dataHoraCancelamentoField;
            }
            set
            {
                this.dataHoraCancelamentoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedido
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedidoInfPedidoCancelamento infPedidoCancelamentoField;

        private Signature signatureField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedidoInfPedidoCancelamento InfPedidoCancelamento
        {
            get
            {
                return this.infPedidoCancelamentoField;
            }
            set
            {
                this.infPedidoCancelamentoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Signature Signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedidoInfPedidoCancelamento
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedidoInfPedidoCancelamentoIdentificacaoNfse identificacaoNfseField;

        private string codigoCancelamentoField;

        private string motivoCancelamentoField;

        private string idField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedidoInfPedidoCancelamentoIdentificacaoNfse IdentificacaoNfse
        {
            get
            {
                return this.identificacaoNfseField;
            }
            set
            {
                this.identificacaoNfseField = value;
            }
        }

        /// <remarks/>
        public string CodigoCancelamento
        {
            get
            {
                return this.codigoCancelamentoField;
            }
            set
            {
                this.codigoCancelamentoField = value;
            }
        }

        /// <remarks/>
        public string MotivoCancelamento
        {
            get
            {
                return this.motivoCancelamentoField;
            }
            set
            {
                this.motivoCancelamentoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseCancelamentoConfirmacaoPedidoInfPedidoCancelamentoIdentificacaoNfse
    {

        private string numeroField;

        private string cnpjField;

        private string inscricaoMunicipalField;

        private int codigoMunicipioField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "string")]
        public string Numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        public string Cnpj
        {
            get
            {
                return this.cnpjField;
            }
            set
            {
                this.cnpjField = value;
            }
        }

        /// <remarks/>
        public string InscricaoMunicipal
        {
            get
            {
                return this.inscricaoMunicipalField;
            }
            set
            {
                this.inscricaoMunicipalField = value;
            }
        }

        /// <remarks/>
        public int CodigoMunicipio
        {
            get
            {
                return this.codigoMunicipioField;
            }
            set
            {
                this.codigoMunicipioField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2000/09/xmldsig#", IsNullable = false)]
    public partial class Signature
    {

        private SignatureSignedInfo signedInfoField;

        private SignatureSignatureValue signatureValueField;

        private SignatureKeyInfo keyInfoField;

        private SignatureObject[] objectField;

        private string idField;

        /// <remarks/>
        public SignatureSignedInfo SignedInfo
        {
            get
            {
                return this.signedInfoField;
            }
            set
            {
                this.signedInfoField = value;
            }
        }

        /// <remarks/>
        public SignatureSignatureValue SignatureValue
        {
            get
            {
                return this.signatureValueField;
            }
            set
            {
                this.signatureValueField = value;
            }
        }

        /// <remarks/>
        public SignatureKeyInfo KeyInfo
        {
            get
            {
                return this.keyInfoField;
            }
            set
            {
                this.keyInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Object")]
        public SignatureObject[] Object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureSignedInfo
    {

        private SignatureSignedInfoCanonicalizationMethod canonicalizationMethodField;

        private SignatureSignedInfoSignatureMethod signatureMethodField;

        private SignatureSignedInfoReference[] referenceField;

        private string idField;

        /// <remarks/>
        public SignatureSignedInfoCanonicalizationMethod CanonicalizationMethod
        {
            get
            {
                return this.canonicalizationMethodField;
            }
            set
            {
                this.canonicalizationMethodField = value;
            }
        }

        /// <remarks/>
        public SignatureSignedInfoSignatureMethod SignatureMethod
        {
            get
            {
                return this.signatureMethodField;
            }
            set
            {
                this.signatureMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Reference")]
        public SignatureSignedInfoReference[] Reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureSignedInfoCanonicalizationMethod
    {

        private string algorithmField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Algorithm
        {
            get
            {
                return this.algorithmField;
            }
            set
            {
                this.algorithmField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureSignedInfoSignatureMethod
    {

        private long hMACOutputLengthField;

        private string[] textField;

        private string algorithmField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "long")]
        public long HMACOutputLength
        {
            get
            {
                return this.hMACOutputLengthField;
            }
            set
            {
                this.hMACOutputLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Algorithm
        {
            get
            {
                return this.algorithmField;
            }
            set
            {
                this.algorithmField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureSignedInfoReference
    {

        private SignatureSignedInfoReferenceTransform[] transformsField;

        private SignatureSignedInfoReferenceDigestMethod digestMethodField;

        private string digestValueField;

        private string idField;

        private string uRIField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Transform", IsNullable = false)]
        public SignatureSignedInfoReferenceTransform[] Transforms
        {
            get
            {
                return this.transformsField;
            }
            set
            {
                this.transformsField = value;
            }
        }

        /// <remarks/>
        public SignatureSignedInfoReferenceDigestMethod DigestMethod
        {
            get
            {
                return this.digestMethodField;
            }
            set
            {
                this.digestMethodField = value;
            }
        }

        /// <remarks/>
        public string DigestValue
        {
            get
            {
                return this.digestValueField;
            }
            set
            {
                this.digestValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string URI
        {
            get
            {
                return this.uRIField;
            }
            set
            {
                this.uRIField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureSignedInfoReferenceTransform
    {

        private string[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        private string[] textField;

        private string algorithmField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("XPath", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("any_element", typeof(string), Namespace = "otherNS")]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public string[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Algorithm
        {
            get
            {
                return this.algorithmField;
            }
            set
            {
                this.algorithmField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2000/09/xmldsig#", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        XPath,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("otherNS:any_element")]
        any_element,
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureSignedInfoReferenceDigestMethod
    {

        private string algorithmField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Algorithm
        {
            get
            {
                return this.algorithmField;
            }
            set
            {
                this.algorithmField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureSignatureValue
    {

        private string idField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureKeyInfo
    {

        private string keyNameField;

        private SignatureKeyInfoKeyValue keyValueField;

        private SignatureKeyInfoRetrievalMethod retrievalMethodField;

        private string[] textField;

        private string idField;

        /// <remarks/>
        public string KeyName
        {
            get
            {
                return this.keyNameField;
            }
            set
            {
                this.keyNameField = value;
            }
        }

        /// <remarks/>
        public SignatureKeyInfoKeyValue KeyValue
        {
            get
            {
                return this.keyValueField;
            }
            set
            {
                this.keyValueField = value;
            }
        }

        /// <remarks/>
        public SignatureKeyInfoRetrievalMethod RetrievalMethod
        {
            get
            {
                return this.retrievalMethodField;
            }
            set
            {
                this.retrievalMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureKeyInfoKeyValue
    {

        private SignatureKeyInfoKeyValueDSAKeyValue dSAKeyValueField;

        private string[] textField;

        /// <remarks/>
        public SignatureKeyInfoKeyValueDSAKeyValue DSAKeyValue
        {
            get
            {
                return this.dSAKeyValueField;
            }
            set
            {
                this.dSAKeyValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureKeyInfoKeyValueDSAKeyValue
    {

        private string pField;

        private string qField;

        private string gField;

        private string yField;

        private string jField;

        private string seedField;

        private string pgenCounterField;

        /// <remarks/>
        public string P
        {
            get
            {
                return this.pField;
            }
            set
            {
                this.pField = value;
            }
        }

        /// <remarks/>
        public string Q
        {
            get
            {
                return this.qField;
            }
            set
            {
                this.qField = value;
            }
        }

        /// <remarks/>
        public string G
        {
            get
            {
                return this.gField;
            }
            set
            {
                this.gField = value;
            }
        }

        /// <remarks/>
        public string Y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        public string J
        {
            get
            {
                return this.jField;
            }
            set
            {
                this.jField = value;
            }
        }

        /// <remarks/>
        public string Seed
        {
            get
            {
                return this.seedField;
            }
            set
            {
                this.seedField = value;
            }
        }

        /// <remarks/>
        public string PgenCounter
        {
            get
            {
                return this.pgenCounterField;
            }
            set
            {
                this.pgenCounterField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureKeyInfoRetrievalMethod
    {

        private string uRIField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string URI
        {
            get
            {
                return this.uRIField;
            }
            set
            {
                this.uRIField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2000/09/xmldsig#")]
    public partial class SignatureObject
    {

        private string[] any_elementField;

        private string[] textField;

        private string idField;

        private string mimeTypeField;

        private string encodingField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("any_element", Namespace = "http://www.publica.inf.br")]
        public string[] any_element
        {
            get
            {
                return this.any_elementField;
            }
            set
            {
                this.any_elementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MimeType
        {
            get
            {
                return this.mimeTypeField;
            }
            set
            {
                this.mimeTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Encoding
        {
            get
            {
                return this.encodingField;
            }
            set
            {
                this.encodingField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class CompNfseNfseSubstituicao
    {

        private ConsultaNfseRecebidaRespostaCompNfseNfseSubstituicaoSubstituicaoNfse substituicaoNfseField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseNfseSubstituicaoSubstituicaoNfse SubstituicaoNfse
        {
            get
            {
                return this.substituicaoNfseField;
            }
            set
            {
                this.substituicaoNfseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseNfseSubstituicaoSubstituicaoNfse
    {

        private string nfseSubstituidoraField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "string")]
        public string NfseSubstituidora
        {
            get
            {
                return this.nfseSubstituidoraField;
            }
            set
            {
                this.nfseSubstituidoraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class CompNfseCartaCorrecao
    {

        private long numeroCartaCorrecaoField;

        private string oficialField;

        private System.DateTime dataDeclaracaoField;

        private ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServico tomadorServicoField;

        private ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoIntermediarioServico intermediarioServicoField;

        private string discriminacaoField;

        private ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoValores valoresField;

        /// <remarks/>
        public long NumeroCartaCorrecao
        {
            get
            {
                return this.numeroCartaCorrecaoField;
            }
            set
            {
                this.numeroCartaCorrecaoField = value;
            }
        }

        /// <remarks/>
        public string Oficial
        {
            get
            {
                return this.oficialField;
            }
            set
            {
                this.oficialField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DataDeclaracao
        {
            get
            {
                return this.dataDeclaracaoField;
            }
            set
            {
                this.dataDeclaracaoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServico TomadorServico
        {
            get
            {
                return this.tomadorServicoField;
            }
            set
            {
                this.tomadorServicoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoIntermediarioServico IntermediarioServico
        {
            get
            {
                return this.intermediarioServicoField;
            }
            set
            {
                this.intermediarioServicoField = value;
            }
        }

        /// <remarks/>
        public string Discriminacao
        {
            get
            {
                return this.discriminacaoField;
            }
            set
            {
                this.discriminacaoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoValores Valores
        {
            get
            {
                return this.valoresField;
            }
            set
            {
                this.valoresField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServico
    {

        private ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoIdentificacaoTomador identificacaoTomadorField;

        private string razaoSocialField;

        private ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoEndereco enderecoField;

        private ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoContato contatoField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoIdentificacaoTomador IdentificacaoTomador
        {
            get
            {
                return this.identificacaoTomadorField;
            }
            set
            {
                this.identificacaoTomadorField = value;
            }
        }

        /// <remarks/>
        public string RazaoSocial
        {
            get
            {
                return this.razaoSocialField;
            }
            set
            {
                this.razaoSocialField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoEndereco Endereco
        {
            get
            {
                return this.enderecoField;
            }
            set
            {
                this.enderecoField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoContato Contato
        {
            get
            {
                return this.contatoField;
            }
            set
            {
                this.contatoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoIdentificacaoTomador
    {

        private ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoIdentificacaoTomadorCpfCnpj cpfCnpjField;

        private string inscricaoMunicipalField;

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoIdentificacaoTomadorCpfCnpj CpfCnpj
        {
            get
            {
                return this.cpfCnpjField;
            }
            set
            {
                this.cpfCnpjField = value;
            }
        }

        /// <remarks/>
        public string InscricaoMunicipal
        {
            get
            {
                return this.inscricaoMunicipalField;
            }
            set
            {
                this.inscricaoMunicipalField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoIdentificacaoTomadorCpfCnpj
    {

        private string cpfField;

        /// <remarks/>
        public string Cpf
        {
            get
            {
                return this.cpfField;
            }
            set
            {
                this.cpfField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoEndereco
    {

        private string enderecoField;

        private string numeroField;

        private string complementoField;

        private string bairroField;

        private long codigoMunicipioField;

        private string ufField;

        private string cepField;

        private string codigoPaisField;

        private string municipioField;

        /// <remarks/>
        public string Endereco
        {
            get
            {
                return this.enderecoField;
            }
            set
            {
                this.enderecoField = value;
            }
        }

        /// <remarks/>
        public string Numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        public string Complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;
            }
        }

        /// <remarks/>
        public string Bairro
        {
            get
            {
                return this.bairroField;
            }
            set
            {
                this.bairroField = value;
            }
        }

        /// <remarks/>
        public long CodigoMunicipio
        {
            get
            {
                return this.codigoMunicipioField;
            }
            set
            {
                this.codigoMunicipioField = value;
            }
        }

        /// <remarks/>
        public string Uf
        {
            get
            {
                return this.ufField;
            }
            set
            {
                this.ufField = value;
            }
        }

        /// <remarks/>
        public string Cep
        {
            get
            {
                return this.cepField;
            }
            set
            {
                this.cepField = value;
            }
        }

        /// <remarks/>
        public string CodigoPais
        {
            get
            {
                return this.codigoPaisField;
            }
            set
            {
                this.codigoPaisField = value;
            }
        }

        /// <remarks/>
        public string Municipio
        {
            get
            {
                return this.municipioField;
            }
            set
            {
                this.municipioField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoTomadorServicoContato
    {

        private string telefoneField;

        private string emailField;

        /// <remarks/>
        public string Telefone
        {
            get
            {
                return this.telefoneField;
            }
            set
            {
                this.telefoneField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoIntermediarioServico
    {

        private string razaoSocialField;

        private ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoIntermediarioServicoCpfCnpj cpfCnpjField;

        private string inscricaoMunicipalField;

        /// <remarks/>
        public string RazaoSocial
        {
            get
            {
                return this.razaoSocialField;
            }
            set
            {
                this.razaoSocialField = value;
            }
        }

        /// <remarks/>
        public ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoIntermediarioServicoCpfCnpj CpfCnpj
        {
            get
            {
                return this.cpfCnpjField;
            }
            set
            {
                this.cpfCnpjField = value;
            }
        }

        /// <remarks/>
        public string InscricaoMunicipal
        {
            get
            {
                return this.inscricaoMunicipalField;
            }
            set
            {
                this.inscricaoMunicipalField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoIntermediarioServicoCpfCnpj
    {

        private string cpfField;

        /// <remarks/>
        public string Cpf
        {
            get
            {
                return this.cpfField;
            }
            set
            {
                this.cpfField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.publica.inf.br")]
    public partial class ConsultaNfseRecebidaRespostaCompNfseCartaCorrecaoValores
    {

        private decimal valorCofinsField;

        private decimal valorInssField;

        private decimal valorIrField;

        private decimal valorCsllField;

        private decimal outrasRetencoesField;

        private decimal valorPisField;

        private decimal descontoCondicionadoField;

        private decimal valorLiquidoNfseField;

        /// <remarks/>
        public decimal ValorCofins
        {
            get
            {
                return this.valorCofinsField;
            }
            set
            {
                this.valorCofinsField = value;
            }
        }

        /// <remarks/>
        public decimal ValorInss
        {
            get
            {
                return this.valorInssField;
            }
            set
            {
                this.valorInssField = value;
            }
        }

        /// <remarks/>
        public decimal ValorIr
        {
            get
            {
                return this.valorIrField;
            }
            set
            {
                this.valorIrField = value;
            }
        }

        /// <remarks/>
        public decimal ValorCsll
        {
            get
            {
                return this.valorCsllField;
            }
            set
            {
                this.valorCsllField = value;
            }
        }

        /// <remarks/>
        public decimal OutrasRetencoes
        {
            get
            {
                return this.outrasRetencoesField;
            }
            set
            {
                this.outrasRetencoesField = value;
            }
        }

        /// <remarks/>
        public decimal ValorPis
        {
            get
            {
                return this.valorPisField;
            }
            set
            {
                this.valorPisField = value;
            }
        }

        /// <remarks/>
        public decimal DescontoCondicionado
        {
            get
            {
                return this.descontoCondicionadoField;
            }
            set
            {
                this.descontoCondicionadoField = value;
            }
        }

        /// <remarks/>
        public decimal ValorLiquidoNfse
        {
            get
            {
                return this.valorLiquidoNfseField;
            }
            set
            {
                this.valorLiquidoNfseField = value;
            }
        }
    }


}
