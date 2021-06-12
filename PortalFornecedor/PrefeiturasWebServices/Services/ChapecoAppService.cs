using ChapecoWebService;
using PrefeiturasWebServices.Interfaces;
using PrefeiturasWebServices.ViewModels.Chapeco;
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
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Xml;
using Utils;
using static Shared.Enums.Enums;

namespace PrefeiturasWebServices.Services
{
    public class ChapecoAppService : IChapecoAppService
    {
        public readonly ICiaRepository _repositoryCia;
        private readonly ILogRepository _repositoryLog;
        private readonly INfseRepository _repositoryNfse;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly ICertificadosRepository _repositoryCertificado;
        private readonly INfseAppService _serviceNfse;
        private readonly IItemNfseRepository _repositoryItemNfse;
        private readonly IMapperObj _mapperObj;
        public ChapecoAppService(ICiaRepository repositoryCia,
                                 ILogRepository repositoryLog,
                                 INfseRepository repositoryNfse,
                                 ICertificadosRepository repositoryCertificado,
                                 INfseAppService serviceNfse,
                                 IHistoricoRepository repositoryHistorico,
                                 IItemNfseRepository repositoryItemNfse,
                                 IMapperObj mapperObj)
        {
            _repositoryCia = repositoryCia;
            _repositoryLog = repositoryLog;
            _repositoryNfse = repositoryNfse;
            _repositoryCertificado = repositoryCertificado;
            _serviceNfse = serviceNfse;
            _repositoryItemNfse = repositoryItemNfse;
            _repositoryHistorico = repositoryHistorico;
            _mapperObj = mapperObj;
        }
        public void Get()
        {
            RFE_LOG _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = "Serviço de busca automática no municipio de Chapeco iniciado.", Servico = Enums.Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao };
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
                    _nfseCast.MUNGER = CodigoIbgeDTO.Chapeco;
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
                            DESCRICAO = "NFSe inserida através de busca automática WebService Chapeco.",
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
                        Servico = Servico.Busca_Automatica_NFSE_Municipio,
                        TipoLog = TipoLog.Informacao
                    });
                }
            });
        }

        public List<XmlNfseViewModel> GetByCnpj(string cnpj)
        {
            List<XmlNfseViewModel> _newNfses = new List<XmlNfseViewModel>();
            RFE_CERTIFICADOS _rfeCertificado = _repositoryCertificado.GetByCnpj(cnpj);
            X509Certificate2 _certificado;
            try
            {
                _certificado = Util.GetCertificate(@"C:\Certificados\" + _rfeCertificado.CERTIFICADO.Split('\\').Last(), _rfeCertificado.SENHA);
                //_certificado = Util.GetCertificate(@"C:\Certificados\45985371000108_000001010131997.pfx" , "br018726");
            }
            catch (Exception e)
            {
                _repositoryLog.Create(new RFE_LOG
                {
                    EDITOR = "GSW",
                    MODIFICADO = DateTime.Now,
                    Mensagem = $"Certificado não encontrado para o CNPJ {cnpj}.",
                    Servico = Servico.Busca_Automatica_NFSE_Municipio,
                    TipoLog = TipoLog.Erro
                });
                return new List<XmlNfseViewModel>();
            }
           
            ServicesClient _webService = new ServicesClient();
            string _xmlEnvio = $@"<?xml version='1.0' encoding='utf-8'?>
                                <ConsultaNfseRecebidaEnvio xmlns='http://www.publica.inf.br'>
                                    <ConsultaNfseRecebida id='assinar'>
                                    <IdentificacaoTomador>
                                        <CpfCnpj>
                                        <Cpf>{cnpj}</Cpf>
                                        </CpfCnpj>
                                    </IdentificacaoTomador>
                                    <DataNfse>{DateTime.Now.ToString("yyyy-MM-dd")}</DataNfse>
                                    </ConsultaNfseRecebida>
                                </ConsultaNfseRecebidaEnvio>";

            XmlDocument _xmlAssinado = AssinarXmlEnvio(_xmlEnvio, _certificado);
            string _resposta = _webService.ConsultarNfseRecebida(_xmlAssinado.OuterXml);
            try
            {
                ChapecoViewModel _objetoXml = Util.Converterobjeto<ChapecoViewModel>(_resposta);
                _objetoXml.ListaNfse.ToList().ForEach(nfse =>
                {
                    var xmlNfseViewModel = MappingXmlNfseViewModel(nfse.Nfse.InfNfse);
                    _newNfses.Add(xmlNfseViewModel);
                });
            }
            catch (Exception)
            {

                ChapecoMsgErroViewModel _objetoXml = Util.Converterobjeto<ChapecoMsgErroViewModel>(_resposta);
                _repositoryLog.Create(new RFE_LOG
                {
                    EDITOR = "GSW",
                    MODIFICADO = DateTime.Now,
                    Mensagem = _objetoXml.ListaMensagemRetorno.MensagemRetorno.Mensagem + $"Dados Consulta -> CNPJ: {cnpj}, Data: {DateTime.Now}",
                    Servico = Servico.Busca_Automatica_NFSE_Municipio,
                    TipoLog = TipoLog.Erro
                });
            }

            return _newNfses;
        }

        private XmlNfseViewModel MappingXmlNfseViewModel(ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse nfe)
        {
            XmlNfseViewModel _xmlNfseViewModel = new XmlNfseViewModel();
            _xmlNfseViewModel.XmlOriginal = Util.ConverterObjetoXMlEmXML<ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse>(nfe).OuterXml;
            _xmlNfseViewModel = PegarEPreencherNumeroNotaECodigoVerificacao(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDePrestador(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDeTomador(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDeDescricao(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDeImpostos(_xmlNfseViewModel, nfe);
            _xmlNfseViewModel = PreencherDadosDeOutrasInformacoes(_xmlNfseViewModel, nfe);
            return _xmlNfseViewModel;
        }

        private XmlNfseViewModel PegarEPreencherNumeroNotaECodigoVerificacao(XmlNfseViewModel xmlNfseViewModel, ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse nfe)
        {
            xmlNfseViewModel.CodigoVerificacao = nfe.CodigoVerificacao;
            xmlNfseViewModel.Numero = nfe.Numero;
            xmlNfseViewModel.Id = nfe.id;
            xmlNfseViewModel.DtEmissao = nfe.DataEmissao.ToString();
            string copetencia = nfe.Competencia; 
            xmlNfseViewModel.MesCompetencia = DateTime.Parse(copetencia).ToString("MM/yyyy");
            xmlNfseViewModel.DtPrestacaoServico = nfe.DataEmissao.ToString();
           
            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDePrestador(XmlNfseViewModel xmlNfseViewModel, ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse nfe)
        {
            xmlNfseViewModel.DadosPrestador = new XmlNfseDadosPrestador();
            xmlNfseViewModel.DadosPrestador.NomeFantasia = nfe.PrestadorServico.NomeFantasia != null ? nfe.PrestadorServico.NomeFantasia : "";
            xmlNfseViewModel.DadosPrestador.RazaoSocial = nfe.PrestadorServico.RazaoSocial != null ? nfe.PrestadorServico.RazaoSocial : "";
            xmlNfseViewModel.DadosPrestador.Nome = nfe.PrestadorServico.RazaoSocial != null ? nfe.PrestadorServico.RazaoSocial : "";
            xmlNfseViewModel.DadosPrestador.Cnpj = nfe.PrestadorServico.IdentificacaoPrestador.Cnpj != null ? nfe.PrestadorServico.IdentificacaoPrestador.Cnpj : "";
            xmlNfseViewModel.DadosPrestador.Cpf = nfe.PrestadorServico.IdentificacaoPrestador.Cpf != null ? nfe.PrestadorServico.IdentificacaoPrestador.Cpf : "";
            xmlNfseViewModel.DadosPrestador.InscricaoMunicipal = nfe.PrestadorServico.IdentificacaoPrestador.InscricaoMunicipal != null ? nfe.PrestadorServico.IdentificacaoPrestador.InscricaoMunicipal : "";
            xmlNfseViewModel.DadosPrestador.Endereco = new XmlNfseEndereco();
            xmlNfseViewModel.DadosPrestador.Endereco.Pais = nfe.PrestadorServico.Endereco.CodigoPais != null ? nfe.PrestadorServico.Endereco.CodigoPais : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Uf = nfe.PrestadorServico.Endereco.Uf != null ? nfe.PrestadorServico.Endereco.Uf : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Municipio = nfe.PrestadorServico.Endereco.Municipio != null ? nfe.PrestadorServico.Endereco.Municipio : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Bairro = nfe.PrestadorServico.Endereco.Bairro != null ? nfe.PrestadorServico.Endereco.Bairro : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Numero = nfe.PrestadorServico.Endereco.Numero != null ? nfe.PrestadorServico.Endereco.Numero : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Complemento = nfe.PrestadorServico.Endereco.Complemento != null ? nfe.PrestadorServico.Endereco.Complemento : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Cep = nfe.PrestadorServico.Endereco.Cep != null ? nfe.PrestadorServico.Endereco.Cep : "";
            xmlNfseViewModel.DadosPrestador.Endereco.Endereco = nfe.PrestadorServico.Endereco.Endereco != null ? nfe.PrestadorServico.Endereco.Endereco : "";
            xmlNfseViewModel.DadosPrestador.Contato = new XmlNfseContato();
            xmlNfseViewModel.DadosPrestador.Contato.Email = nfe.PrestadorServico.Contato.Email != null ? nfe.PrestadorServico.Contato.Email : "";
            xmlNfseViewModel.DadosPrestador.Contato.Telefone = nfe.PrestadorServico.Contato.Telefone != null ? nfe.PrestadorServico.Contato.Telefone : "";

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDeTomador(XmlNfseViewModel xmlNfseViewModel, ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse nfe)
        {
            xmlNfseViewModel.DadosTomador = new XmlNfseDadosTomador();
            xmlNfseViewModel.DadosTomador.RazaoSocial = nfe.TomadorServico.RazaoSocial != null ? nfe.TomadorServico.RazaoSocial : "";
            xmlNfseViewModel.DadosTomador.Nome = nfe.TomadorServico.RazaoSocial != null ? nfe.TomadorServico.RazaoSocial : "";
            xmlNfseViewModel.DadosTomador.Cnpj = nfe.TomadorServico.IdentificacaoTomador.CpfCnpj.Cnpj != null ? nfe.TomadorServico.IdentificacaoTomador.CpfCnpj.Cnpj : "";
            xmlNfseViewModel.DadosTomador.Cpf = nfe.TomadorServico.IdentificacaoTomador.CpfCnpj.Cpf != null ? nfe.TomadorServico.IdentificacaoTomador.CpfCnpj.Cpf : "";
            xmlNfseViewModel.DadosTomador.InscricaoMunicipal = nfe.TomadorServico.IdentificacaoTomador.InscricaoMunicipal != null ? nfe.TomadorServico.IdentificacaoTomador.InscricaoMunicipal : "";
            xmlNfseViewModel.DadosTomador.Endereco = new XmlNfseEndereco();
            xmlNfseViewModel.DadosTomador.Endereco.Pais = nfe.TomadorServico.Endereco.CodigoPais != null ? nfe.TomadorServico.Endereco.CodigoPais : "";
            xmlNfseViewModel.DadosTomador.Endereco.Uf = nfe.TomadorServico.Endereco.Uf != null ? nfe.TomadorServico.Endereco.Uf : "";
            xmlNfseViewModel.DadosTomador.Endereco.Municipio = nfe.TomadorServico.Endereco.CodigoMunicipio != null ? nfe.TomadorServico.Endereco.CodigoMunicipio : "";
            xmlNfseViewModel.DadosTomador.Endereco.Bairro = nfe.TomadorServico.Endereco.Bairro != null ? nfe.TomadorServico.Endereco.Bairro : "";
            xmlNfseViewModel.DadosTomador.Endereco.Numero = nfe.TomadorServico.Endereco.Numero != null ? nfe.TomadorServico.Endereco.Numero : "";
            xmlNfseViewModel.DadosTomador.Endereco.Complemento = nfe.TomadorServico.Endereco.Complemento != null ? nfe.TomadorServico.Endereco.Complemento : "";
            xmlNfseViewModel.DadosTomador.Endereco.Cep = nfe.TomadorServico.Endereco.Cep != null ? nfe.TomadorServico.Endereco.Cep : "";
            xmlNfseViewModel.DadosTomador.Endereco.Endereco = nfe.TomadorServico.Endereco.Endereco != null ? nfe.TomadorServico.Endereco.Endereco : "";
            xmlNfseViewModel.DadosTomador.Contato = new XmlNfseContato();
            xmlNfseViewModel.DadosTomador.Contato.Email = nfe.TomadorServico.Contato.Email != null ? nfe.TomadorServico.Contato.Email : "";
            xmlNfseViewModel.DadosTomador.Contato.Telefone = nfe.TomadorServico.Contato.Telefone != null ? nfe.TomadorServico.Contato.Telefone : "";

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDeDescricao(XmlNfseViewModel xmlNfseViewModel, ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse nfe)
        {
            xmlNfseViewModel.DiscriminacaoServico = nfe.Servico.Discriminacao != null ? nfe.Servico.Discriminacao : "";
            xmlNfseViewModel.DescricaoTipoServico = nfe.Servico.ItemListaServico;
            xmlNfseViewModel.ItemListaServico = nfe.Servico.ItemListaServico;
            xmlNfseViewModel.ImpostosRetidos = new XmlNfseImpostosRetidos();
            xmlNfseViewModel.Itens = new List<XmlNfseServicoItem>();
            XmlNfseServicoItem itemServico = new XmlNfseServicoItem();
            itemServico.Descricao = $"{nfe.Servico.ItemListaServico} - Descrição Serviço";
            itemServico.Qtde = "1";
            itemServico.VlrTotal = nfe.Servico.Valores.ValorServicos.ToString();
            xmlNfseViewModel.VlrTotal = Decimal.Parse(nfe.Servico.Valores.ValorServicos.ToString());
            itemServico.VlrUnitario = nfe.Servico.Valores.ValorServicos.ToString();


            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDeImpostos(XmlNfseViewModel xmlNfseViewModel, ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse nfe)
        {
            xmlNfseViewModel.ImpostosRetidos = new XmlNfseImpostosRetidos();
            xmlNfseViewModel.ImpostosRetidos.VlrCofins = nfe.Servico.Valores.ValorCofins.ToString();
            xmlNfseViewModel.ImpostosRetidos.VlrCsll = nfe.Servico.Valores.ValorCsll.ToString();
            xmlNfseViewModel.ImpostosRetidos.VlrInss = nfe.Servico.Valores.ValorInss.ToString();
            xmlNfseViewModel.ImpostosRetidos.VlrIssRetido = nfe.Servico.Valores.ValorIssRetido.ToString();
            xmlNfseViewModel.ImpostosRetidos.VlrIrrf = nfe.Servico.Valores.ValorIr.ToString();
            xmlNfseViewModel.ImpostosRetidos.VlrPisPasep = nfe.Servico.Valores.ValorPis.ToString();
            xmlNfseViewModel.ImpostosRetidos.AlqIssRetido = nfe.Servico.Valores.Aliquota.ToString();
            xmlNfseViewModel.ImpostosRetidos.AlqCofins = "0";
            xmlNfseViewModel.ImpostosRetidos.AlqCsll = "0";
            xmlNfseViewModel.ImpostosRetidos.AlqInss = "0";
            xmlNfseViewModel.ImpostosRetidos.AlqIrrf = "0";
            xmlNfseViewModel.ImpostosRetidos.AlqPisPasep = "0";
            xmlNfseViewModel.VlrOutrasRetencoes = nfe.Servico.Valores.OutrasRetencoes.ToString();
            xmlNfseViewModel.DescontoCondicionado = nfe.Servico.Valores.DescontoCondicionado.ToString();
            xmlNfseViewModel.DescontoIncondicionado = nfe.Servico.Valores.DescontoIncondicionado.ToString();
            xmlNfseViewModel.Iss = new XmlNfseImpostosIss();
            xmlNfseViewModel.Iss.BaseCalculo = nfe.Servico.Valores.BaseCalculo.ToString();
            xmlNfseViewModel.Iss.Aliquota = nfe.Servico.Valores.Aliquota.ToString();
            xmlNfseViewModel.Iss.Vlr = nfe.Servico.Valores.ValorIss.ToString();
            xmlNfseViewModel.VlrLiquido = nfe.Servico.Valores.ValorLiquidoNfse.ToString();
            xmlNfseViewModel.VlrTotal = nfe.Servico.Valores.ValorLiquidoNfse;
            xmlNfseViewModel.VlrServicos = nfe.Servico.Valores.ValorServicos.ToString();
            xmlNfseViewModel.VlrDeducoes = nfe.Servico.Valores.ValorDeducoes.ToString();




            return xmlNfseViewModel;
        }

        private XmlNfseViewModel PreencherDadosDeOutrasInformacoes(XmlNfseViewModel xmlNfseViewModel, ConsultaNfseRecebidaRespostaCompNfseNfseInfNfse nfe)
        {
            xmlNfseViewModel.LocalPrestacao = CodigoIbgeDTO.GetMunicipioGerador(CodigoIbgeDTO.Chapeco);
            xmlNfseViewModel.MunicipioIncidencia = nfe.OrgaoGerador.CodigoMunicipio.ToString();
            xmlNfseViewModel.IncentivadorCultural = nfe.IncentivadorCultural.Contains("0") ? XmlNfseSimNao.S : XmlNfseSimNao.N;
            xmlNfseViewModel.OptanteSimplesNacional = nfe.OptanteSimplesNacional.Contains("0") ? XmlNfseSimNao.S : XmlNfseSimNao.N;
            xmlNfseViewModel.OutrasInformacoes = nfe.OutrasInformacoes;
            xmlNfseViewModel.Serie = nfe.Serie;
            xmlNfseViewModel.Rps = new XmlNfseRps();
            if (nfe.IdentificacaoRps != null)
            {
                xmlNfseViewModel.Rps.Numero = nfe.IdentificacaoRps.Numero.ToString();
                xmlNfseViewModel.Rps.Serie = nfe.IdentificacaoRps.Serie;
                xmlNfseViewModel.Rps.Tipo = nfe.IdentificacaoRps.Tipo.ToString();
            }

            return xmlNfseViewModel;
        }

        public XmlDocument AssinarXmlEnvio(string mensagemXML, X509Certificate2 certificado)
        {

            var xmlDoc = new XmlDocument();

            SignedXml SignedDocument;
            var keyInfo = new KeyInfo();

            xmlDoc.LoadXml(mensagemXML);

            //'Adiciona Certificado ao Key Info
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            SignedDocument = new SignedXml(xmlDoc)
            {
                //Permite exportar a PrivateKey do certificado digital
                SigningKey = certificado.GetRSAPrivateKey(),
                KeyInfo = keyInfo
            };
            //'Seta chaves
            //' Cria referencia
            var reference = new Reference { Uri = string.Empty };
            //' Adiciona transformacao a referencia
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform(false));
            //' Adiciona referencia ao xml
            SignedDocument.AddReference(reference);
            //' Calcula Assinatura
            SignedDocument.ComputeSignature();
            //' Pega representação da assinatura
            var xmlDigitalSignature = SignedDocument.GetXml();
            //' Adiciona ao doc XML
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            return xmlDoc;
        }
    }
}
