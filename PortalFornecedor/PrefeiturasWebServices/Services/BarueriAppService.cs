using BarueriWebService;
using Repository.DTOs;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.Interfaces;
using Services.Mapper;
using Services.ViewModels;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Utils;
using static Shared.Enums.Enums;
using IBarueriAppService = PrefeiturasWebServices.Interfaces.IBarueriAppService;

namespace PrefeiturasWebServices.Services
{
    public class BarueriAppService : IBarueriAppService
    {
        //
        public readonly ICiaRepository _repositoryCia;
        private readonly ILogRepository _repositoryLog;
        private readonly INfseRepository _repositoryNfse;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly ICertificadosRepository _repositoryCertificado;
        private readonly INfseAppService _serviceNfse;
        private readonly IMapperObj _mapperObj;
        public BarueriAppService(ICiaRepository repositoryCia,
                                 ILogRepository repositoryLog,
                                 INfseRepository repositoryNfse,
                                 ICertificadosRepository repositoryCertificado,
                                 INfseAppService serviceNfse,
                                 IHistoricoRepository repositoryHistorico,
                                 IMapperObj mapperObj)
        {
            _repositoryCia = repositoryCia;
            _repositoryLog = repositoryLog;
            _repositoryNfse = repositoryNfse;
            _repositoryCertificado = repositoryCertificado;
            _serviceNfse = serviceNfse;
            _repositoryHistorico = repositoryHistorico;
            _mapperObj = mapperObj;
        }
        public void Get()
        {
            RFE_LOG _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = "Serviço de busca automática no municipio de Barueri iniciado.", Servico = Enums.Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao };
            _repositoryLog.Create(_log);

            List<RFE_CIA> _cias = _repositoryCia.Get();

            List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();
            _cias.ForEach(cia =>
            {
                List<XmlNfseViewModel> _nfses = GetByCnpj(cia.CNPJ);
                List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();
                _nfses.ForEach(nfse =>
                {
                    RFE_NFSE _nfseCast = _mapperObj.MapToNfse(nfse);
                    _nfseCast.MUNGER = CodigoIbgeDTO.Barueri;
                    if (!_repositoryNfse.ExisteNotaNaHeaderNfse(_nfseCast.CHNFSE))
                        _nfsesRFE.Add(_nfseCast);
                });

                if (_nfsesRFE.Count() > 0)
                {
                    _serviceNfse.InsertWithItensAndRepositorio(_nfsesRFE);
                    _nfsesRFE.ForEach(nfse =>
                    {
                        RFE_HISTORICO _historico = new RFE_HISTORICO()
                        {
                            DESCRICAO = "NFSe inserida através de busca automática WebService Barueri.",
                            EDITOR = "GSW Busca Automática.",
                            REPOSITORIO = nfse.RFE_REPOSITORIO,
                            TIPO = EHistorico.Evento
                        };
                        _repositoryHistorico.Create(_historico);
                    });
                    _repositoryLog.Create(new RFE_LOG
                    {
                        EDITOR = "GSW",
                        MODIFICADO = DateTime.Now,
                        Mensagem = $"{_nfsesRFE.Count} inseridas com sucesso, em {DateTime.Now}.",
                        Servico = Enums.Servico.Busca_Automatica_NFSE_Municipio,
                        TipoLog = TipoLog.Informacao
                    });
                }
            });
        }

        //alteracao retorno do método GetByCnpj
        public List<XmlNfseViewModel> GetByCnpj(string cnpj)
        {

            RFE_CERTIFICADOS _certificado = _repositoryCertificado.GetByCnpj(cnpj);

            if (_certificado == null)
            {
                RFE_LOG log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = $"O CNPJ {cnpj} não foi encontrado na tabela de Certificados.", Servico = Enums.Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao };
                _repositoryLog.Create(log);
                return new List<XmlNfseViewModel>();
            }

            ConsultarNfeResposta resposta;
            List<XmlNfseViewModel> _newNfses = new List<XmlNfseViewModel>();
            do
            {
                try
                {
                    var certificado2 = Util.GetCertificate(@"C:\Certificados\" + _certificado.CERTIFICADO.Split('\\').Last(), _certificado.SENHA);
                    //X509Certificate2 certificado2 = Util.GetCertificate(@"C:\Users\rodrigo.peres\Desktop\Documentos Municipios NFSe\Certificados\45985371000108_000001010131997.pfx", "br018726");
                    if(certificado2 == null)
                        throw new Exception($"Não foi encontrado um certificado para o CNPJ {cnpj}.");
                    if(certificado2.NotAfter < DateTime.Now)
                        throw new Exception($"O certificado {_certificado.CERTIFICADO.Split('\\').Last()} está vencido.");

                    wsGeraXmlSoapClient _webService = new wsGeraXmlSoapClient(wsGeraXmlSoapClient.EndpointConfiguration.wsGeraXmlSoap12);
                    _webService.ClientCredentials.ClientCertificate.Certificate = certificado2;
                    do {
                        int pagina = 1;
                        string xmlEnvio = $@"<?xml version='1.0' encoding='utf-16'?>
                                        <NFeRecebidaPeriodo xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://www.barueri.sp.gov.br/nfe'>
                                          <CPFCNPJTomador>{cnpj}</CPFCNPJTomador>
                                          <DataInicial>{DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd")}</DataInicial>
                                          <DataFinal>{DateTime.Now.ToString("yyyy-MM-dd")}</DataFinal>
                                          <Pagina>{pagina}</Pagina>
                                        </NFeRecebidaPeriodo>";
                        pagina++;
                        resposta = _webService.ConsultaNFeRecebidaPeriodo(1, xmlEnvio);
                    } while (resposta.ListaNfe.CompNfe1.Length >= 50);
                    

                }
                catch (Exception e)
                {
                    RFE_LOG log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = "Erro ao carregar o certificado.  " + e.Message, Servico = Enums.Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Erro };
                   // _repositoryLog.Create(log);
                    return new List<XmlNfseViewModel>();
                }

                if (resposta.ListaMensagemRetorno == null)
                {
                    resposta.ListaNfe.CompNfe1.ToList().ForEach( _nfse => {
                        XmlNfseViewModel _xmlNfseViewModel = MappingXmlNfseViewModel(_nfse);
                        _newNfses.Add(_xmlNfseViewModel);
                    });
                }

                else
                    TratarErros(resposta.ListaMensagemRetorno, cnpj);

            } while (resposta.ListaNfe.CompNfe1 != null && resposta.ListaNfe.CompNfe1.Length >= 50);

            return _newNfses;
        }

        private void TratarErros(MensagemRetorno[] listaMensagemRetorno, string cnpj)
        {
            listaMensagemRetorno.ToList().ForEach(erro => {
                RFE_LOG log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = $"CNPJ: {cnpj} - " + erro.Mensagem, Servico = Enums.Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao };
                _repositoryLog.Create(log);
            });
        }

        private XmlNfseViewModel MappingXmlNfseViewModel(Nfe nfe)
        {
            XmlNfseViewModel _xmlNfseViewModel = new XmlNfseViewModel();
            _xmlNfseViewModel.XmlOriginal = Util.ConverterObjetoXMlEmXML<Nfe>(nfe).OuterXml;
            _xmlNfseViewModel = PegarEPreencherNumeroNotaECodigoVerificacao(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDePrestador(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDeTomador(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDeDescricao(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDeImpostos(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDeOutrasInformacoes(_xmlNfseViewModel, nfe);
            return _xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDeOutrasInformacoes(XmlNfseViewModel xmlNfseViewModel, Nfe nfe)
        {
            xmlNfseViewModel.LocalPrestacao = CodigoIbgeDTO.GetMunicipioGerador(CodigoIbgeDTO.Barueri);
            xmlNfseViewModel.MunicipioIncidencia = CodigoIbgeDTO.Barueri;
            xmlNfseViewModel.Rps = new XmlNfseRps();
            xmlNfseViewModel.Rps.Numero = nfe.Nfe1.InfNFe1.DeclaracaoPrestacaoServico.InfDeclaracaoPrestacaoServico.Rps.IdentificacaoRps.Numero;
            xmlNfseViewModel.Rps.Serie = nfe.Nfe1.InfNFe1.DeclaracaoPrestacaoServico.InfDeclaracaoPrestacaoServico.Rps.IdentificacaoRps.Serie;
            xmlNfseViewModel.Rps.DtEmissao = nfe.Nfe1.InfNFe1.DeclaracaoPrestacaoServico.InfDeclaracaoPrestacaoServico.Rps.DataEmissao;
            string regimeTributacaoEspecial = nfe.Nfe1.InfNFe1.DeclaracaoPrestacaoServico.InfDeclaracaoPrestacaoServico.RegimeEspecialTributacao;
            //xmlNfseViewModel.RegimeEspecialTributacao = ;
            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDeImpostos(XmlNfseViewModel xmlNfseViewModel, Nfe nfe)
        {
            var impostos = nfe.Nfe1.InfNFe1.DeclaracaoPrestacaoServico.InfDeclaracaoPrestacaoServico.Servico.Valores;
            xmlNfseViewModel.ImpostosRetidos = new XmlNfseImpostosRetidos();
            xmlNfseViewModel.ImpostosRetidos.VlrCofins = impostos.ValorCofins;
            xmlNfseViewModel.ImpostosRetidos.VlrCsll = impostos.ValorCsll;
            xmlNfseViewModel.ImpostosRetidos.VlrInss = "0";
            xmlNfseViewModel.ImpostosRetidos.VlrIrrf = impostos.ValorIr;
            xmlNfseViewModel.ImpostosRetidos.VlrPisPasep = impostos.ValorPis;
            xmlNfseViewModel.ImpostosRetidos.AlqIssRetido = impostos.Aliquota;
            xmlNfseViewModel.ImpostosRetidos.AlqCofins = "0";
            xmlNfseViewModel.ImpostosRetidos.AlqCsll = "0";
            xmlNfseViewModel.ImpostosRetidos.AlqInss = "0";
            xmlNfseViewModel.ImpostosRetidos.AlqIrrf = "0";
            xmlNfseViewModel.ImpostosRetidos.AlqPisPasep = "0";
            xmlNfseViewModel.Iss = new XmlNfseImpostosIss();
            xmlNfseViewModel.Iss.BaseCalculo = nfe.Nfe1.InfNFe1.ValoresNfe.BaseCalculo;
            xmlNfseViewModel.Iss.Aliquota = nfe.Nfe1.InfNFe1.ValoresNfe.Aliquota;
            xmlNfseViewModel.Iss.Vlr = nfe.Nfe1.InfNFe1.ValoresNfe.ValorIss;
            xmlNfseViewModel.VlrLiquido = nfe.Nfe1.InfNFe1.ValoresNfe.ValorLiquidoNfe;

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDeDescricao(XmlNfseViewModel xmlNfseViewModel, Nfe nfe)
        {
            BarueriWebService.Servico servico = nfe.Nfe1.InfNFe1.DeclaracaoPrestacaoServico.InfDeclaracaoPrestacaoServico.Servico;
            xmlNfseViewModel.DiscriminacaoServico = servico.Discriminacao != null ? servico.Discriminacao : "";
            xmlNfseViewModel.DescricaoTipoServico = servico.Discriminacao != null ? servico.Discriminacao : "";
            xmlNfseViewModel.ItemListaServico = servico.CodigoServico;
            xmlNfseViewModel.ImpostosRetidos = new XmlNfseImpostosRetidos();
            xmlNfseViewModel.ImpostosRetidos.VlrIssRetido = servico.IssRetido != null ? servico.IssRetido : "";
            xmlNfseViewModel.Itens = new List<XmlNfseServicoItem>();
            XmlNfseServicoItem itemServico = new XmlNfseServicoItem();
            itemServico.Descricao = servico.Discriminacao != null ? servico.Discriminacao : ""; 
            itemServico.Qtde = "1";
            itemServico.VlrTotal = servico.Valores.ValorServicos;
            xmlNfseViewModel.VlrTotal = Decimal.Parse(servico.Valores.ValorServicos);
            itemServico.VlrUnitario = servico.Valores.ValorServicos;
            xmlNfseViewModel.Itens.Add(itemServico);

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDeTomador(XmlNfseViewModel xmlNfseViewModel, Nfe nfe)
        {
            TomadorServico tomador = nfe.Nfe1.InfNFe1.DeclaracaoPrestacaoServico.InfDeclaracaoPrestacaoServico.TomadorServico;
            xmlNfseViewModel.DadosTomador = new XmlNfseDadosTomador();
            xmlNfseViewModel.DadosTomador.RazaoSocial = tomador.RazaoSocial != null ? tomador.RazaoSocial : "";
            xmlNfseViewModel.DadosTomador.Nome = tomador.RazaoSocial != null ? tomador.RazaoSocial : "";
            xmlNfseViewModel.DadosTomador.Cnpj = tomador.IdentificacaoTomador.CpfCnpj.Cnpj != null ? tomador.IdentificacaoTomador.CpfCnpj.Cnpj : "";
            xmlNfseViewModel.DadosTomador.Cpf = tomador.IdentificacaoTomador.CpfCnpj.Cpf != null ? tomador.IdentificacaoTomador.CpfCnpj.Cpf : "";
            xmlNfseViewModel.DadosTomador.InscricaoMunicipal = tomador.IdentificacaoTomador.InscricaoMunicipal != null ? tomador.IdentificacaoTomador.InscricaoMunicipal : "";
            xmlNfseViewModel.DadosTomador.Endereco = new XmlNfseEndereco();
            xmlNfseViewModel.DadosTomador.Endereco.Pais = tomador.Endereco.Pais != null ? tomador.Endereco.Pais : "";
            xmlNfseViewModel.DadosTomador.Endereco.Uf = tomador.Endereco.Uf != null ? tomador.Endereco.Uf : "";
            xmlNfseViewModel.DadosTomador.Endereco.Municipio = tomador.Endereco.Cidade != null ? tomador.Endereco.Cidade : "";
            xmlNfseViewModel.DadosTomador.Endereco.Bairro = tomador.Endereco.Bairro != null ? tomador.Endereco.Bairro : "";
            xmlNfseViewModel.DadosTomador.Endereco.Numero = tomador.Endereco.NumeroEndereco != null ? tomador.Endereco.NumeroEndereco : "";
            xmlNfseViewModel.DadosTomador.Endereco.Complemento = tomador.Endereco.ComplementoEndereco != null ? tomador.Endereco.ComplementoEndereco : "";
            xmlNfseViewModel.DadosTomador.Endereco.Cep = tomador.Endereco.Cep != null ? tomador.Endereco.Cep : "";
            xmlNfseViewModel.DadosTomador.Endereco.Endereco = tomador.Endereco.Endereco1 != null ? tomador.Endereco.Endereco1 : "";
            xmlNfseViewModel.DadosTomador.Contato = new XmlNfseContato();
            xmlNfseViewModel.DadosTomador.Contato.Email = tomador.Contato.Email != null ? tomador.Contato.Email : "";
            xmlNfseViewModel.DadosTomador.Contato.Telefone = tomador.Contato.Telefone != null ? tomador.Contato.Telefone : "";

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDePrestador(XmlNfseViewModel xmlNfseViewModel, Nfe nfe)
        {
            PrestadorServico prestador = nfe.Nfe1.InfNFe1.PrestadorServico;
            xmlNfseViewModel.DadosPrestador = new XmlNfseDadosPrestador();
            xmlNfseViewModel.DadosPrestador.NomeFantasia = prestador.NomeFantasia != null ? prestador.NomeFantasia : "";
            xmlNfseViewModel.DadosPrestador.RazaoSocial = prestador.RazaoSocial != null ? prestador.RazaoSocial : "";
            xmlNfseViewModel.DadosPrestador.Nome = prestador.RazaoSocial != null ? prestador.RazaoSocial : "";
            xmlNfseViewModel.DadosPrestador.Cnpj = prestador.IdentificacaoPrestador.CpfCnpj.Cnpj != null ? prestador.IdentificacaoPrestador.CpfCnpj.Cnpj : "";
            xmlNfseViewModel.DadosPrestador.Cpf = prestador.IdentificacaoPrestador.CpfCnpj.Cpf != null ? prestador.IdentificacaoPrestador.CpfCnpj.Cpf : "";
            xmlNfseViewModel.DadosPrestador.InscricaoMunicipal = prestador.IdentificacaoPrestador.InscricaoMunicipal != null ? prestador.IdentificacaoPrestador.InscricaoMunicipal : "";
            xmlNfseViewModel.DadosPrestador.Endereco = new XmlNfseEndereco();
            xmlNfseViewModel.DadosPrestador.Endereco.Pais = prestador.Endereco.Pais != null ? prestador.Endereco.Pais : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Uf = prestador.Endereco.Uf != null ? prestador.Endereco.Uf : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Municipio = prestador.Endereco.Cidade != null ? prestador.Endereco.Cidade : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Bairro = prestador.Endereco.Bairro != null ? prestador.Endereco.Bairro : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Numero = prestador.Endereco.NumeroEndereco != null ? prestador.Endereco.NumeroEndereco : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Complemento = prestador.Endereco.ComplementoEndereco != null ? prestador.Endereco.ComplementoEndereco : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Cep = prestador.Endereco.Cep != null ? prestador.Endereco.Cep : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Endereco = prestador.Endereco.Endereco1 != null ? prestador.Endereco.Endereco1 : "";
            xmlNfseViewModel.DadosPrestador.Contato = new XmlNfseContato();
            xmlNfseViewModel.DadosPrestador.Contato.Email = prestador.Contato.Email != null ? prestador.Contato.Email : "";
            xmlNfseViewModel.DadosPrestador.Contato.Telefone = prestador.Contato.Telefone != null ? prestador.Contato.Telefone : "";

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PegarEPreencherNumeroNotaECodigoVerificacao(XmlNfseViewModel XmlNfseViewModel, Nfe nfe)
        {
            XmlNfseViewModel.CodigoVerificacao = nfe.Nfe1.InfNFe1.CodigoVerificacao;
            XmlNfseViewModel.Numero = nfe.Nfe1.InfNFe1.NumeroNfe.ToString();
            XmlNfseViewModel.Id = nfe.Nfe1.InfNFe1.NumeroNfe.ToString();
            XmlNfseViewModel.DtEmissao = nfe.Nfe1.InfNFe1.DataEmissao;
            string copetencia = nfe.Nfe1.InfNFe1.DeclaracaoPrestacaoServico.InfDeclaracaoPrestacaoServico.Competencia;
            XmlNfseViewModel.MesCompetencia = DateTime.Parse(copetencia).ToString("MM/yyyy");
            XmlNfseViewModel.DtPrestacaoServico = nfe.Nfe1.InfNFe1.DataEmissao;
            return XmlNfseViewModel;
        }

    }
}

