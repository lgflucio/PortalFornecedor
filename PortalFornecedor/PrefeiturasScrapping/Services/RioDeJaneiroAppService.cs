using Newtonsoft.Json;
using PrefeiturasScrapping.Componentes;
using PrefeiturasScrapping.Interfaces;
using PrefeiturasScrapping.ViewModels;
using Repository.DTOs;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.Interfaces;
using Services.Mapper;
using Services.ViewModels;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Utils;
using WebDrivers.Interfaces;
using static Shared.Enums.Enums;
using Servico = Shared.Enums.Enums.Servico;

namespace PrefeiturasScrapping.Services
{
    public class RioDeJaneiroAppService : IRioDeJaneiroAppService
    {
        private readonly ICiaRepository _ciaRepository;
        private readonly IWebDriverFactory _webDriverFactory;
        private readonly IMapperObj _mapperObj;
        private readonly IItemNfseRepository _repositoryItemNfse;
        private readonly INfseRepository _repositoryNfse;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly ILogRepository _repositoryLog;
        private readonly INfseAppService _serviceNfse;

        //private readonly string Caminho = @"C:\Notas Fiscais\RioDeJaneiro\";

        private RioDeJaneiroComponente _pageRJComponente;

        public RioDeJaneiroAppService(ICiaRepository ciaRepository,
                                      IWebDriverFactory webDriverFactory,
                                      IMapperObj mapperObj,
                                      IItemNfseRepository repositoryItemNfse,
                                      INfseRepository repositoryNfse,
                                      IHistoricoRepository repositoryHistorico,
                                      ILogRepository repositoryLog,
                                      INfseAppService serviceNfse)
        {
            _ciaRepository = ciaRepository;
            _webDriverFactory = webDriverFactory;
            _mapperObj = mapperObj;
            _repositoryItemNfse = repositoryItemNfse;
            _repositoryNfse = repositoryNfse;
            _repositoryHistorico = repositoryHistorico;
            _repositoryLog = repositoryLog;
            _serviceNfse = serviceNfse;
        }

        public void Start(int periodoConsulta, string diretorioDownload = "")
        {
            try
            {
                ObterNFSePrefeiturasRioDeJaneiro(periodoConsulta, diretorioDownload);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ObterNFSePrefeiturasRioDeJaneiro(int periodoConsulta, string diretorioDownload = "")
        {
            var _cias = _ciaRepository.Get();

            _cias.ForEach(cia =>
            {
                if (cia.CNPJ == "03255266000173")
                {
                    _pageRJComponente = new RioDeJaneiroComponente(cia, _webDriverFactory, periodoConsulta, diretorioDownload);
                    var lista = MapearNFSeBaixadas(diretorioDownload, cia.CNPJ);
                    InserirNFse(lista);
                }
            });

            if (Directory.Exists(diretorioDownload))
            {
                Directory.Delete(diretorioDownload, true);
            }
        }
        public List<XmlNfseViewModel> MapearNFSeBaixadas(string caminho, string cnpj)
        {
            string[] _arquivos = Directory.GetFiles($@"{caminho}\{cnpj}");
            XmlNfseViewModel _nfse = new XmlNfseViewModel();
            List<XmlNfseViewModel> _listNfse = new List<XmlNfseViewModel>();

            foreach (string arquivo in _arquivos)
            {

                XElement _xml = arquivo.CarregarXML();

                string _jsonXml = JsonConvert.SerializeObject(_xml);

                RioDeJaneiroNfseViewModel _nfseRj = JsonConvert.DeserializeObject<RioDeJaneiroNfseViewModel>(_jsonXml);


                DateTime.TryParse(_nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.DataEmissao, out DateTime _dataHoraEmissao);
                _nfse.DtEmissao = _dataHoraEmissao.ToString();
                _nfse.Numero = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Numero;
                _nfse.Rps.Numero = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.IdentificacaoRps.Numero;
                _nfse.Rps.Serie = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.IdentificacaoRps.Serie;
                _nfse.Rps.Tipo = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.IdentificacaoRps.Tipo;
                _nfse.Rps.DtEmissao = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.DataEmissaoRps;
                _nfse.DadosPrestador.InscricaoMunicipal = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.IdentificacaoPrestador.InscricaoMunicipal;
                _nfse.DadosPrestador.Cnpj = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.IdentificacaoPrestador.Cnpj;
                _nfse.DadosPrestador.RazaoSocial = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.RazaoSocial;
                _nfse.DadosPrestador.Endereco.Endereco = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.Endereco.endereco;
                _nfse.DadosPrestador.Endereco.Numero = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.Endereco.Numero;
                _nfse.DadosPrestador.Endereco.Complemento = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.Endereco.Complemento;
                _nfse.DadosPrestador.Endereco.Bairro = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.Endereco.Bairro;
                _nfse.DadosPrestador.Endereco.Municipio = CodigoIbgeDTO.RioDeJaneiro;
                _nfse.DadosPrestador.Endereco.Uf = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.Endereco.Uf;
                _nfse.DadosPrestador.Endereco.Cep = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.PrestadorServico.Endereco.Cep;
                _nfse.Tributacao = "";
                _nfse.CodigoTributacaoMunicipio = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.CodigoTributacaoMunicipio;
                _nfse.OptanteSimplesNacional = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.OptanteSimplesNacional == "1" ? XmlNfseSimNao.S : XmlNfseSimNao.N;
                _nfse.NumeroGuia = "";
                _nfse.DtPagamento = ""; // não tem
                _nfse.VlrServicos = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Valores.ValorServicos;
                _nfse.CodigoVerificacao = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.CodigoVerificacao;
                _nfse.Iss.Aliquota = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Valores.Aliquota;
                _nfse.Iss.Vlr = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Valores.ValorIss;
                _nfse.VlrCredito = "";
                _nfse.ImpostosRetidos.VlrIssRetido = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Valores.IssRetido;
                _nfse.DadosTomador.Cnpj = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.TomadorServico.IdentificacaoTomador.CpfCnpj.Cnpj;
                _nfse.DadosTomador.InscricaoMunicipal = "";
                _nfse.DadosTomador.RazaoSocial = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.TomadorServico.RazaoSocial;
                _nfse.DadosTomador.Endereco.Endereco = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.TomadorServico.Endereco.endereco;
                _nfse.DadosTomador.Endereco.Numero = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.TomadorServico.Endereco.Numero;
                _nfse.DadosTomador.Endereco.Complemento = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.TomadorServico.Endereco.Complemento;
                _nfse.DadosTomador.Endereco.Bairro = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.TomadorServico.Endereco.Bairro;
                _nfse.DadosTomador.Endereco.Municipio = CodigoIbgeDTO.RioDeJaneiro;
                _nfse.DadosTomador.Endereco.Uf = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.TomadorServico.Endereco.Uf;
                _nfse.DadosTomador.Endereco.Cep = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.TomadorServico.Endereco.Cep;
                _nfse.DiscriminacaoServico = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Discriminacao;
                _nfse.MesCompetencia = _dataHoraEmissao.ToString("MM/yyyy");
                _nfse.ItemListaServico = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.ItemListaServico;
                _nfse.DescricaoTipoServico = _nfse.DiscriminacaoServico = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Discriminacao;
                _nfse.VlrTotal = decimal.Parse(_nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Valores.ValorServicos.Replace(".", ","));

                _nfse.Itens.Add(new XmlNfseServicoItem
                {
                    Descricao = _nfse.DiscriminacaoServico,
                    Qtde = "1",
                    VlrUnitario = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Valores.ValorServicos,
                    VlrTotal = _nfseRj.ConsultarNfseResposta.ListaNfse.CompNfse.Nfse.InfNfse.Servico.Valores.ValorServicos,
                    Tributavel = XmlNfseSimNao.N
                });

                _listNfse.Add(_nfse);
            }

            return _listNfse;
        }
        public void InserirNFse(List<XmlNfseViewModel> _nfses)
        {
            List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();

            _nfses.ForEach(nfse =>
            {
                RFE_NFSE _nfseCast = _mapperObj.MapToNfse(nfse);
                _nfseCast.MUNGER = CodigoIbgeDTO.RioDeJaneiro;
                if (!_repositoryNfse.ExisteNotaNaHeaderNfse(_nfseCast.CHNFSE))
                    _nfsesRFE.Add(_nfseCast);
            });

            if (_nfsesRFE.Any())
            {
                _serviceNfse.InsertWithItensAndRepositorio(_nfsesRFE);
                _nfsesRFE.ForEach(nfse =>
                {
                    RFE_HISTORICO _historico = new RFE_HISTORICO()
                    {
                        DESCRICAO = "NFSe inserida através de busca automática Raspagem de Rio de Janeiro.",
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
                    Mensagem = $"{_nfses.Count} inseridas com sucesso, em {DateTime.Now}.",
                    Servico = Servico.Busca_Automatica_NFSE_Municipio,
                    TipoLog = TipoLog.Informacao
                });
            }
        }
    }
}
