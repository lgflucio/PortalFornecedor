using Repository.DTOs;
using Repository.Entities.PREFEITURAS_ENTITIES;
using Repository.Entities.RFE_ENTITIES;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Utils;
using static Shared.Enums.Enums;

namespace Services.Mapper
{
    public class MapperObj : IMapperObj
    {
        //public RFE_NFSE MapToNfse(XmlNfseViewModel xml)
        //{
        //    RFE_NFSE _nfse = new RFE_NFSE();
        //    _nfse.EDITOR = "Serviço De Busca Automatica NFSE";
        //    _nfse = PegarEPreencherNumeroNotaECodigoVerificacao(xml, _nfse);
        //    _nfse = PreencherDadosDePrestador(xml, _nfse);
        //    _nfse = PreencherDadosDeTomador(xml, _nfse);
        //    _nfse = PreencherDadosDeDescricao(xml, _nfse);
        //    _nfse = PreencherDadosDeImpostos(xml, _nfse);
        //    _nfse = PreencherDadosDeOutrasInformacoes(xml, _nfse);
        //    _nfse = PreencherDadosTabelaRepositorio(xml, _nfse);
        //    _nfse.DT_ENTRADA_REPOSIT = DateTime.Now;
        //    _nfse.MODIFICADO = DateTime.Now;
        //    return _nfse;
        //}
        //private RFE_NFSE PreencherDadosTabelaRepositorio(XmlNfseViewModel xmlNfseViewModel, RFE_NFSE nfse)
        //{
        //    RFE_REPOSITORIO _repositorio = new RFE_REPOSITORIO();
        //    string cpfOuCnpj = Util.DesformatarCNPJCPF(!string.IsNullOrEmpty(xmlNfseViewModel.DadosPrestador.Cnpj) ? xmlNfseViewModel.DadosPrestador.Cpf : xmlNfseViewModel.DadosPrestador.Cnpj);
        //    _repositorio.Chave = nfse.CHNFSE;
        //    _repositorio.CNPJ_DESTINATARIO = xmlNfseViewModel.DadosTomador.Cnpj != "" ? xmlNfseViewModel.DadosTomador.Cnpj : xmlNfseViewModel.DadosTomador.Cpf;
        //    _repositorio.CNPJ_Fornecedor = xmlNfseViewModel.DadosPrestador.Cnpj != "" ? xmlNfseViewModel.DadosPrestador.Cnpj : xmlNfseViewModel.DadosPrestador.Cpf;
        //    _repositorio.TipoXml = ETipoXML.NFSe;
        //    _repositorio.OrigemXML = OrigemXML.Upload_Via_WebService;
        //    _repositorio.Processado = true;
        //    _repositorio.NNF = long.Parse(xmlNfseViewModel.Numero);
        //    XDocument xDoc = XDocument.Parse(Util.ConverterObjetoXMlEmXML<XmlNfseViewModel>(xmlNfseViewModel).InnerXml);
        //    _repositorio.Cabecalho = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        //    xDoc.Declaration = null;
        //    _repositorio.XML = xmlNfseViewModel.XmlOriginal;
        //    _repositorio.flag_servico = FlagServicos.Nenhum;
        //    _repositorio.FLAG_REGRA_NEGOCIO = FlgExecRegraNegocio.NaoProcessado;
        //    _repositorio.DHEMI = DateTime.Parse(xmlNfseViewModel.DtEmissao);
        //    _repositorio.DHEMI_STRING = xmlNfseViewModel.DtEmissao;
        //    _repositorio.EDITOR = "Serviço De Busca Automatica NFSE";
        //    _repositorio.MODIFICADO = DateTime.Now;
        //    nfse.RFE_REPOSITORIO = _repositorio;
        //    return nfse;

        //}

        //private RFE_NFSE PreencherDadosDeOutrasInformacoes(XmlNfseViewModel xmlNfseViewModel, RFE_NFSE nfse)
        //{
        //    nfse.LOCAL_PRESTACAO = xmlNfseViewModel.LocalPrestacao;
        //    nfse.MUNGER = string.IsNullOrEmpty(xmlNfseViewModel.DadosPrestador.Endereco.Municipio) ? CodigoIbgeDTO.GetMunicipioGerador(xmlNfseViewModel.DadosPrestador.Endereco.Municipio) : "";
        //    nfse.COMPETENCIA = xmlNfseViewModel.MesCompetencia.Replace("/", string.Empty);
        //    nfse.ATIVIDADE = xmlNfseViewModel.CNAE;
        //    nfse.NATUREZAOPERACAO = xmlNfseViewModel.NaturezaOperacao;
        //    nfse.OPTANTESIMPLES = xmlNfseViewModel.OptanteSimplesNacional.ToString();
        //    nfse.REGIMEESPECIALTRIBUTACAO = xmlNfseViewModel.RegimeEspecialTributacao.ToString();
        //    nfse.OUTRASINFORMACOES = xmlNfseViewModel.OutrasInformacoes;
        //    return nfse;
        //}

        //private RFE_NFSE PreencherDadosDeImpostos(XmlNfseViewModel xmlNfseViewModel, RFE_NFSE nfse)
        //{
        //    nfse.VALORTOTAL = xmlNfseViewModel.VlrTotal;
        //    nfse.VALORCOFINS = xmlNfseViewModel.ImpostosRetidos.VlrCofins != null ? decimal.Parse(xmlNfseViewModel.ImpostosRetidos.VlrCofins.Replace(".", ",")) : 0;
        //    nfse.VALORCSLL = xmlNfseViewModel.ImpostosRetidos.VlrCsll != null ? decimal.Parse(xmlNfseViewModel.ImpostosRetidos.VlrCsll.Replace(".", ",")) : 0;
        //    nfse.VALORISS = xmlNfseViewModel.Iss.Vlr != null ? decimal.Parse(xmlNfseViewModel.Iss.Vlr.Replace(".", ",")) : 0;
        //    nfse.VALORIR = xmlNfseViewModel.ImpostosRetidos.VlrIrrf != null ? decimal.Parse(xmlNfseViewModel.ImpostosRetidos.VlrIrrf.Replace(".", ",")) : 0;
        //    nfse.VALORPIS = xmlNfseViewModel.ImpostosRetidos.VlrPisPasep != null ? decimal.Parse(xmlNfseViewModel.ImpostosRetidos.VlrPisPasep.Replace(".", ",")) : 0;
        //    nfse.ISSRETIDO = xmlNfseViewModel.ImpostosRetidos.AlqIssRetido != null ? decimal.Parse(xmlNfseViewModel.ImpostosRetidos.AlqIssRetido.Replace(".", ",")) : 0;
        //    nfse.ALIQUOTA = xmlNfseViewModel.Iss.Aliquota != null ? decimal.Parse(xmlNfseViewModel.Iss.Aliquota.Replace(".", ",")) : 0;
        //    nfse.BASECALCULO = xmlNfseViewModel.Iss.BaseCalculo != null ? decimal.Parse(xmlNfseViewModel.Iss.BaseCalculo.Replace(".", ",")) : 0;
        //    nfse.CODSERVNFSE = xmlNfseViewModel.ItemListaServico;
        //    nfse.CODSERV_CODTRIB_NFSE = xmlNfseViewModel.ItemListaServico;
        //    nfse.VALORSERVICO = xmlNfseViewModel.VlrServicos != null ? decimal.Parse(xmlNfseViewModel.VlrServicos.Replace(".", ",")) : 0;
        //    nfse.VALORLIQUIDO = xmlNfseViewModel.VlrLiquido != null ? decimal.Parse(xmlNfseViewModel.VlrLiquido.Replace(".", ",")) : 0;
        //    nfse.VALORDEDUCOES = xmlNfseViewModel.VlrDeducoes != null ? decimal.Parse(xmlNfseViewModel.VlrDeducoes.Replace(".", ",")) : 0;
        //    nfse.VALOR_DESCONTO = xmlNfseViewModel.VlrDesconto != null ? decimal.Parse(xmlNfseViewModel.VlrDesconto.Replace(".", ",")) : 0;
        //    nfse.OUTRASRETENCOES = xmlNfseViewModel.VlrOutrasRetencoes != null ? decimal.Parse(xmlNfseViewModel.VlrOutrasRetencoes.Replace(".", ",")) : 0;

        //    return nfse;
        //}

        //private RFE_NFSE PreencherDadosDeDescricao(XmlNfseViewModel xmlNfseViewModel, RFE_NFSE nfse)
        //{
        //    nfse.DISCRIMINACAO = xmlNfseViewModel.DiscriminacaoServico;
        //    nfse.DESCRICAO_TIPO_SERVICO = xmlNfseViewModel.DescricaoTipoServico;
        //    nfse.Itens = new List<RFE_ITEM_NFSE>();
        //    int numeroItem = 0;
        //    xmlNfseViewModel.Itens.ForEach(item =>
        //    {
        //        RFE_ITEM_NFSE _itemNfse = new RFE_ITEM_NFSE();
        //        _itemNfse.DESCRICAO_SERVICO = xmlNfseViewModel.DiscriminacaoServico;
        //        _itemNfse.QTD_ITEM_SERVICO = int.Parse(item.Qtde.Replace(".", string.Empty));
        //        string cpfOuCnpj = Util.DesformatarCNPJCPF(!string.IsNullOrEmpty(xmlNfseViewModel.DadosPrestador.Cnpj) ? xmlNfseViewModel.DadosPrestador.Cpf : xmlNfseViewModel.DadosPrestador.Cnpj);
        //        _itemNfse.CHNFSE = Util.GerarCampoChaveTabelaRFE(xmlNfseViewModel.MesCompetencia, cpfOuCnpj, xmlNfseViewModel.Numero);
        //        _itemNfse.CODIGO_SERVICO = xmlNfseViewModel.ItemListaServico;
        //        numeroItem++;
        //        _itemNfse.NITEM = numeroItem.ToString();
        //        _itemNfse.VALOR_UNITARIO = decimal.Parse(item.VlrUnitario.Replace(".",","));
        //        _itemNfse.VALOR_TOTAL = decimal.Parse(item.VlrTotal.Replace(".",","));
        //        _itemNfse.VALOR_DESCONTO = xmlNfseViewModel.VlrDesconto != null ? decimal.Parse(xmlNfseViewModel.VlrDesconto.Replace(".", ",")) : 0;
        //        _itemNfse.TRIBUTADO = item.Tributavel == XmlNfseSimNao.S ? true : false;
        //        _itemNfse.STATUS = "1";
        //        nfse.Itens.Add(_itemNfse);

        //    });

        //    return nfse;
        //}

        //private RFE_NFSE PreencherDadosDeTomador(XmlNfseViewModel xmlNfseViewModel, RFE_NFSE nfse)
        //{
        //    nfse.TOMADORRAZAO = xmlNfseViewModel.DadosTomador.RazaoSocial;
        //    nfse.TOMADORCNPJ = xmlNfseViewModel.DadosTomador.Cnpj;
        //    nfse.TOMADORCPF = xmlNfseViewModel.DadosTomador.Cpf;
        //    return nfse;
        //}

        //private RFE_NFSE PreencherDadosDePrestador(XmlNfseViewModel xmlNfseViewModel, RFE_NFSE nfse)
        //{
        //    nfse.PRESTADORNOME = xmlNfseViewModel.DadosPrestador.Nome;
        //    nfse.PRESTADORRAZAO = xmlNfseViewModel.DadosPrestador.RazaoSocial;
        //    nfse.PRESTADORCNPJ = xmlNfseViewModel.DadosPrestador.Cnpj;
        //    nfse.PRESTADORCPF = xmlNfseViewModel.DadosPrestador.Cpf;
        //    nfse.INSCRICAO = xmlNfseViewModel.DadosPrestador.InscricaoMunicipal;
        //    nfse.PRESTADORUF = xmlNfseViewModel.DadosPrestador.Endereco.Uf;
        //    nfse.UF_PRESTACAO = xmlNfseViewModel.LocalPrestacao;

        //    return nfse;
        //}

        //private RFE_NFSE PegarEPreencherNumeroNotaECodigoVerificacao(XmlNfseViewModel xmlNfseViewModel, RFE_NFSE nfse)
        //{
        //    xmlNfseViewModel.Id = xmlNfseViewModel.Numero;
        //    nfse.CODVERIFICACAO = xmlNfseViewModel.CodigoVerificacao;
        //    nfse.NUNFSE = long.Parse(xmlNfseViewModel.Numero);
        //    nfse.DHEMI = DateTime.Parse(xmlNfseViewModel.DtEmissao);
        //    nfse.STATUS = EStatusRFeNFSe.NaoClassificado;
        //    string cpfOuCnpjPrestador = Util.DesformatarCNPJCPF(string.IsNullOrEmpty(xmlNfseViewModel.DadosPrestador.Cnpj) ? xmlNfseViewModel.DadosPrestador.Cpf : xmlNfseViewModel.DadosPrestador.Cnpj);
        //    nfse.CHNFSE = Util.GerarCampoChaveTabelaRFE(xmlNfseViewModel.DtEmissao, cpfOuCnpjPrestador, xmlNfseViewModel.Numero);

        //    return nfse;
        //}

        //public NotasServicos MapingToNfse(XmlNfseViewModel xml)
        //{
        //    DateTime.TryParse(xml.DtEmissao, out DateTime _dataEmissao);
        //    string _CnpjCfp = Util.DesformatarCNPJCPF(string.IsNullOrEmpty(xml.DadosPrestador.Cnpj) ? xml.DadosPrestador.Cpf : xml.DadosPrestador.Cnpj);

        //    NotasServicos _nfse = new NotasServicos
        //    {
        //        CodigoVerificacao = xml.CodigoVerificacao,
        //        Numero = long.Parse(xml.Numero),
        //        DataEmissao = _dataEmissao,
        //        Chave = Util.GerarCampoChaveTabelaRFE(_dataEmissao.ToString(), _CnpjCfp, xml.Numero),
        //        Serie = xml.Serie,
        //        MesCompetencia = xml.MesCompetencia.Replace("/", string.Empty)
        //    };

        //    //Tributações
        //    _nfse.Tributacao = xml.Tributacao;
        //    _nfse.CodigoTributacao = xml.CodigoTributacaoMunicipio;
        //    _nfse.CodigoServico = xml.ItemListaServico;
        //    _nfse.OptanteSimplesNacional = xml.OptanteSimplesNacional.ToString();

        //    //Valores Servicos
        //    _nfse.ValorServicos = xml.VlrServicos != null ? decimal.Parse(xml.VlrServicos.Replace(".",",")) : 0;
        //    _nfse.Valortotal              = xml.VlrTotal;
        //    _nfse.ValorCredito            = xml.VlrCredito != null ?decimal.Parse(xml.VlrCredito.Replace(".",",")) : 0;
        //    _nfse.ValorCofins             = xml.ImpostosRetidos.VlrCofins != null ? decimal.Parse(xml.ImpostosRetidos.VlrCofins.Replace(".", ",")) : 0;
        //    _nfse.ValorCsll               = xml.ImpostosRetidos.VlrCsll != null ? decimal.Parse(xml.ImpostosRetidos.VlrCsll.Replace(".", ",")) : 0;
        //    _nfse.ValorIss                = xml.Iss.Vlr != null ? decimal.Parse(xml.Iss.Vlr.Replace(".", ",")) : 0;
        //    _nfse.ValorIr                 = xml.ImpostosRetidos.VlrIrrf != null ? decimal.Parse(xml.ImpostosRetidos.VlrIrrf.Replace(".", ",")) : 0;
        //    _nfse.ValorPis                = xml.ImpostosRetidos.VlrPisPasep != null ? decimal.Parse(xml.ImpostosRetidos.VlrPisPasep.Replace(".", ",")) : 0;
        //    _nfse.ValorIssRetido          = xml.ImpostosRetidos.AlqIssRetido != null ? decimal.Parse(xml.ImpostosRetidos.AlqIssRetido.Replace(".", ",")) : 0;
        //    _nfse.BaseCalculo             = xml.Iss.BaseCalculo != null ? decimal.Parse(xml.Iss.BaseCalculo.Replace(".", ",")) : 0;
        //    _nfse.ValorLiquido            = xml.VlrLiquido != null ? decimal.Parse(xml.VlrLiquido.Replace(".", ",")) : 0;
        //    _nfse.ValorDeducoes           = xml.VlrDeducoes != null ? decimal.Parse(xml.VlrDeducoes.Replace(".", ",")) : 0;
        //    _nfse.ValorDesconto           = xml.VlrDesconto != null ? decimal.Parse(xml.VlrDesconto.Replace(".", ",")) : 0;
        //    _nfse.ValorOutrasRetencoes    = xml.VlrOutrasRetencoes != null ? decimal.Parse(xml.VlrOutrasRetencoes.Replace(".", ",")) : 0; 
        //    _nfse.ValorAliquota           = xml.Iss.Aliquota != null ? decimal.Parse(xml.Iss.Aliquota.Replace(".", ",")) : 0;
            
        //    //OUTRAS INFORMAÇÕES
        //    _nfse.Cnae = xml.CNAE;
        //    _nfse.NaturezaOperacao = xml.NaturezaOperacao;
        //    _nfse.OutrasInformacoes = xml.OutrasInformacoes;

        //    _nfse.DiscriminacaoServico = xml.DiscriminacaoServico;
        //    _nfse.DescricaoTipoServico = xml.DescricaoTipoServico;
        //    _nfse.Itens = new List<ItensNfse>();
        //    int numeroItem = 0;

        //    xml.Itens.ForEach(item =>
        //    {
        //        ItensNfse _itemNfse = new ItensNfse();
        //        _itemNfse.Descricao = xml.DiscriminacaoServico;
        //        _itemNfse.Quantidade = int.Parse(item.Qtde.Replace(".", string.Empty));
        //        _itemNfse.CodigoServico = xml.ItemListaServico;
        //        numeroItem++;
        //        _itemNfse.NumeroItem = numeroItem;
        //        _itemNfse.ValorUnitario = decimal.Parse(item.VlrUnitario.Replace(".", ","));
        //        _itemNfse.ValorTotal = decimal.Parse(item.VlrTotal.Replace(".", ","));
        //        _itemNfse.ValorDesconto = xml.VlrDesconto != null ? decimal.Parse(xml.VlrDesconto.Replace(".", ",")) : 0;
        //        _itemNfse.Tributavel = item.Tributavel == XmlNfseSimNao.S ? true : false;
        //        _nfse.Itens.Add(_itemNfse);

        //    });

        //    ///DADOS TOMADOR
        //    _nfse.TomadorBairro = xml.DadosTomador.Endereco.Bairro;
        //    _nfse.TomadorCep = xml.DadosTomador.Endereco.Cep;
        //    _nfse.TomadorUf = xml.DadosTomador.Endereco.Uf;
        //    _nfse.TomadorMunicipio = xml.DadosTomador.Endereco.Municipio;
        //    _nfse.TomadorNumero = xml.DadosTomador.Endereco.Numero;
        //    _nfse.TomadorRazaoSocial = xml.DadosTomador.RazaoSocial;
        //    _nfse.TomadorInscricaoEstadual = xml.DadosTomador.InscricaoEstadual;
        //    _nfse.TomadorInscricaoMunicipal = xml.DadosTomador.InscricaoMunicipal;
        //    _nfse.TomadorCnpj = xml.DadosTomador.Cnpj;
        //    _nfse.TomadorCpf = xml.DadosTomador.Cpf;

        //    //DADOS PRESTADOR
        //    _nfse.PrestadorBairro = xml.DadosPrestador.Endereco.Bairro;
        //    _nfse.PrestadorCep = xml.DadosPrestador.Endereco.Cep;
        //    _nfse.PrestadorUf = xml.DadosPrestador.Endereco.Uf;
        //    _nfse.PrestadorMunicipio = xml.DadosPrestador.Endereco.Municipio;
        //    _nfse.PrestadorNumero = xml.DadosPrestador.Endereco.Numero;
        //    _nfse.PrestadorRazaoSocial = xml.DadosPrestador.RazaoSocial;
        //    _nfse.PrestadorInscricaoEstadual = xml.DadosPrestador.InscricaoEstadual;
        //    _nfse.PrestadorInscricaoMunicipal = xml.DadosPrestador.InscricaoMunicipal;
        //    _nfse.PrestadorCnpj = xml.DadosPrestador.Cnpj;
        //    _nfse.PrestadorCpf = xml.DadosPrestador.Cpf;

        //    //DADOS CONTAINER
        //    ContainerNotas _container = new ContainerNotas();
        //    _container.Chave = _nfse.Chave;
        //    _container.Numero = _nfse.Numero;
        //    _container.Origem = "Busca Automática";
        //    _container.DataEmissao = _nfse.DataEmissao;
        //    _container.Cabecalho = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        //    _container.Xml = xml.XmlOriginal;
        //    _container.PrestadorCnpj = _nfse.PrestadorCnpj != null ? _nfse.PrestadorCnpj : _nfse.PrestadorCpf;
        //    _container.TomadorCnpj = _nfse.TomadorCnpj != null ? _nfse.TomadorCnpj : _nfse.TomadorCpf;
        //    _nfse.Container = _container;


        //    return _nfse;
        //}


    }
}
