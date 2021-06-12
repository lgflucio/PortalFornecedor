using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
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
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Utils;
using WebDrivers.Interfaces;
using static Shared.Enums.Enums;
using Servico = Shared.Enums.Enums.Servico;

namespace PrefeiturasScrapping.Services
{
    public class BlumenauAppService : IBlumenauAppService
    {
        private readonly IWebDriverFactory _webDriverFactorys;
        private readonly INfseRepository _repositoryNfse;
        private readonly INfseAppService _serviceNfse;
        private readonly ILogRepository _repositoryLog;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly IItemNfseRepository _repositoryItemNfse;
        private readonly IMapperObj _mapperObj;
        private WebDriverWait _wait;
        private readonly string _pathDirectory;



        public BlumenauAppService(IWebDriverFactory webDriverFactorys,
                                    INfseRepository repositoryNfse,
                                    INfseAppService serviceNfse,
                                    ILogRepository repositoryLog,
                                    IHistoricoRepository repositoryHistorico,
                                    IItemNfseRepository repositoryItemNfse,
                                    IMapperObj mapperObj)
        {
            _webDriverFactorys = webDriverFactorys;
            _repositoryNfse = repositoryNfse;
            _serviceNfse = serviceNfse;
            _repositoryLog = repositoryLog;
            _repositoryHistorico = repositoryHistorico;
            _repositoryItemNfse = repositoryItemNfse;
            _mapperObj = mapperObj;
            _pathDirectory = @"C:\Notas Fiscais\Blumenau\45985371000108";
        }
        public void Get(int periodoConsulta, string diretorioDownload = "")
        {
            _repositoryLog.Create(new RFE_LOG
            {
                EDITOR = "GSW",
                MODIFICADO = DateTime.Now,
                Mensagem = "Serviço de busca automática no municipio de Blumenau iniciado.",
                Servico = Servico.Busca_Automatica_NFSE_Municipio,
                TipoLog = TipoLog.Informacao
            });

            List<XmlNfseViewModel> _newNfses = GetNfsesByCertificate(periodoConsulta, diretorioDownload);
            List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();

            _newNfses.ForEach(nfse =>
            {
                RFE_NFSE _nfseRfe = _mapperObj.MapToNfse(nfse);
                _nfseRfe.MUNGER = CodigoIbgeDTO.Blumenau;

                if (!_repositoryNfse.ExisteNotaNaHeaderNfse(_nfseRfe.CHNFSE))
                {
                    _nfsesRFE.Add(_nfseRfe);
                }
            });

            if (_nfsesRFE.Count() > 0)
            {
                _serviceNfse.InsertWithItensAndRepositorio(_nfsesRFE);

                _nfsesRFE.ForEach(nfse =>
                {
                    RFE_HISTORICO _historico = new RFE_HISTORICO()
                    {
                        DESCRICAO = "NFSe inserida através de busca automática Raspagem de Dados de Blumenau.",
                        EDITOR = "GSW Busca Automática.",
                        REPOSITORIO = nfse.RFE_REPOSITORIO,
                        TIPO = EHistorico.Evento
                    };

                    _repositoryHistorico.Create(_historico);
                });

                RFE_LOG _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = $"{_newNfses.Count} inseridas com sucesso, em {DateTime.Now}.", Servico = Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao };
                _repositoryLog.Create(_log);
            }
        }

        private List<XmlNfseViewModel> GetNfsesByCertificate(int periodoConsulta = 0, string diretorioDownload = "")
        {
            RFE_LOG _log;
            IWebDriver _driver = null;
            List<XmlNfseViewModel> _newNfses = new List<XmlNfseViewModel>();
            int _nroNfseDownloadTrys = 0;

            while (true)
            {
                try
                {
                    _driver = _webDriverFactorys.GetChromeDriver("45985371000108", "Blumenau", diretorioDownload);

                    _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 60));

                    GoToLoginPage(_driver);
                    AcessarSistema(_driver);
                    AcessarIntegracaoISSQN(_driver);

                    DateTime _dtInicio = DateTime.Now.AddDays(-periodoConsulta);
                    DateTime _dtFim = DateTime.Now;

                    ConsultaNFSEsRecebidas(_driver, _dtInicio, _dtFim);

                    if (_driver.PageSource.Contains("Não há notas para período selecionado!"))
                    {
                        if (Directory.Exists(diretorioDownload))
                        {
                            Directory.Delete(diretorioDownload, true);
                        }

                        _driver.Dispose();

                        return _newNfses;
                    }

                    _newNfses = ReadXmlsInDirectory(_pathDirectory);

                    break;
                }
                catch (Exception e)
                {
                    _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = $"{e.Message}", Servico = Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Erro };
                    _repositoryLog.Create(_log);

                    if (_driver != null)
                        _driver.Close();

                    _nroNfseDownloadTrys++;

                    if (_nroNfseDownloadTrys > 4)
                        break;
                }
            }

            if (_driver != null)
                _driver.Close();

            _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = $"{_newNfses.Count} raspadas com sucesso, iniciar inserção das notas na base de dados do RFE.", Servico = Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao };
            _repositoryLog.Create(_log);

            return _newNfses;
        }

        private void GoToLoginPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://nfse.blumenau.sc.gov.br/contrib/");
        }

        private void AcessarSistema(IWebDriver driver)
        {
            //Fechar janela de Atenção
            if (driver.PageSource.Contains("ATENÇÃO"))
            {
                _wait.Until(c => c.FindElement(By.XPath("//*[@id=\"modalBannerPaicandu\"]/div/div/div[1]/button/span[1]"))).Click();
            }

            //Selecionar Sistema ISSQ
            var _sistemaAtual = driver.FindElement(By.XPath("//*[@id=\"topo\"]/div/div[2]/div[1]"));
            var _linkISSQN = driver.FindElement(By.XPath("//*[@id=\"itemContainer\"]/li[1]/a"));

            IAction acaoLogout = new Actions(driver)
                                .MoveToElement(_sistemaAtual)
                                .MoveToElement(_linkISSQN)
                                .Click()
                                .Build();

            acaoLogout.Perform();

            //Fechar janela de Atenção
            if (driver.PageSource.Contains("ATENÇÃO"))
            {
                _wait.Until(c => c.FindElement(By.XPath("//*[@id=\"modalBannerPaicandu\"]/div/div/div[1]/button/span[1]"))).Click();
            }

            /*=======================================================================================*/
            // dispara uma nova thread para executar
            Thread _secondTread = new Thread(() => NovaThread(driver));
            _secondTread.Start();
            /*=======================================================================================*/

            Task.Delay(5000);

            //Logar com Certificado
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[4]/div[2]/form/p[2]/a"))).Click();
        }

        private void AcessarIntegracaoISSQN(IWebDriver driver)
        {
            Thread.Sleep(5000);

            //Exportar Notas
            var _integracao = _wait.Until(c => c.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[2]/ul/li[4]/a")));
            var _exportarNotas = _wait.Until(c => c.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[2]/ul/li[4]/ul/li[2]/a")));

            IAction acaoLogout = new Actions(driver)
                                .MoveToElement(_integracao)
                                .MoveToElement(_exportarNotas)
                                .Click()
                                .Build();

            acaoLogout.Perform();
        }

        private void ConsultaNFSEsRecebidas(IWebDriver driver, DateTime dtInicio, DateTime dtFim)
        {
            Thread.Sleep(5000);

            //Acessa o Frame
            IWebElement _framePrincipal = _wait.Until(c => c.FindElement(By.Id("ifrm")));
            driver.SwitchTo().Frame(_framePrincipal);

            Thread.Sleep(3000);

            //Seleciona Livro Tomador
            _wait.Until(c => c.FindElement(By.Id("ctl00_ContentPlaceHolder1_rbl_Tipo_Prestador_1"))).Click();

            Thread.Sleep(10000);

            //Seleciona Formato XML
            var _tipoXML = _wait.Until(c => c.FindElement(By.Id("ctl00_ContentPlaceHolder1_rbl_Exportacao_TxT_Excel_1")));
            ScrollElementIntoView(driver, _tipoXML);

            _tipoXML.Click();

            //Define Data de Início
            _wait.Until(c => c.FindElement(By.Id("ctl00_ContentPlaceHolder1_txt_Data_Inicio"))).SendKeys(dtInicio.ToString("dd/MM/yyyy"));

            //Define Data de Fim
            _wait.Until(c => c.FindElement(By.Id("ctl00_ContentPlaceHolder1_txt_Data_Fim"))).SendKeys(dtFim.ToString("dd/MM/yyyy"));

            //Exportar NFSes
            _wait.Until(c => c.FindElement(By.Id("ctl00_ContentPlaceHolder1_btn_Exportar"))).Click();
        }

        private void ScrollElementIntoView(IWebDriver _driver, IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scroll(" + element.Location.X + "," + (element.Location.Y - 200) + ");");
        }

        public void NovaThread(IWebDriver driver)
        {
            Thread.Sleep(5000);

            WndSO _wndSO = new WndSO();
            var _processesName = driver.GetType().Name.Replace("Driver", "");
            var _tituloJanela = "Acesso ao sistema - SIMPLISS WEB - Google Chrome";
            var _nomeEmpresa = "3M DO BRASIL LTDA";
            _wndSO.AcessaSelecaoDeCertificados(_processesName, _tituloJanela, _nomeEmpresa);
        }

        public List<XmlNfseViewModel> ReadXmlsInDirectory(string pathDirectory)
        {
            string[] _arquivos = Directory.GetFiles(pathDirectory);

            List<XmlNfseViewModel> _nfses = new List<XmlNfseViewModel>();

            foreach (string arquivo in _arquivos)
            {
                XmlDocument _xml = new XmlDocument();
                _xml.Load($@"{arquivo}");

                XmlNodeList xmls = _xml.GetElementsByTagName("NOTA");

                for (int i = 0; i < xmls.Count; i++)
                {
                    XmlNode xml = xmls.Item(i);
                    XmlDocument _reader = new XmlDocument();
                    _reader.LoadXml(xml.OuterXml);

                    XmlNfseViewModel _xmlDefault = ConvertXmlToXmlDefault(_reader.OuterXml);
                    _nfses.Add(_xmlDefault);
                }
            }

            if (Directory.Exists(pathDirectory))
            {
                Directory.Delete(pathDirectory, true);
            }

                return _nfses;
        }

        private XmlNfseViewModel ConvertXmlToXmlDefault(string xml)
        {
            XmlDocument _xml = new XmlDocument();
            _xml.LoadXml(xml);

            string _json = JsonConvert.SerializeObject(_xml);

            BlumenauNfseViewModel _nfseBlumenau = JsonConvert.DeserializeObject<BlumenauNfseViewModel>(_json);

            XmlNfseViewModel _nfse = new XmlNfseViewModel();

            DateTime.TryParse(_nfseBlumenau.Nota.Dt_Competencia, out DateTime _dataHoraEmissao);

            _nfse.XmlOriginal = xml;
            //NAO ENCONTRADO
            _nfse.DtEmissao = _dataHoraEmissao.ToString();
            _nfse.Numero = _nfseBlumenau.Nota.Numero;
            _nfse.Serie = _nfseBlumenau.Nota.Serie;
            _nfse.Rps.Numero = null;
            _nfse.Rps.Serie = null;
            _nfse.Rps.Tipo = null;
            _nfse.Rps.DtEmissao = null;
            _nfse.DadosPrestador.InscricaoEstadual = _nfseBlumenau.Nota.Pre_Inscricao_Estadual;
            _nfse.DadosPrestador.Cnpj = _nfseBlumenau.Nota.Cnpj;
            _nfse.DadosPrestador.Cpf = null;
            _nfse.DadosPrestador.RazaoSocial = _nfseBlumenau.Nota.Pre_Razao_Social;
            _nfse.DadosPrestador.Endereco.Endereco = _nfseBlumenau.Nota.Pre_Endereco;
            _nfse.DadosPrestador.Endereco.Numero = _nfseBlumenau.Nota.Pre_Endereco_Numero;
            _nfse.DadosPrestador.Endereco.Complemento = _nfseBlumenau.Nota.Pre_Endereco_Complemento;
            _nfse.DadosPrestador.Endereco.Bairro = _nfseBlumenau.Nota.Pre_Endereco_Bairro;
            _nfse.DadosPrestador.Endereco.Municipio = CodigoIbgeDTO.Blumenau;
            _nfse.DadosPrestador.Endereco.Uf = _nfseBlumenau.Nota.Pre_Endereco_Uf;
            _nfse.DadosPrestador.Endereco.Cep = _nfseBlumenau.Nota.Pre_Endereco_Cep;
            _nfse.DadosPrestador.Nome = _nfseBlumenau.Nota.Pre_Razao_Social;
            _nfse.Tributacao = _nfseBlumenau.Nota.Cd_Regime_Especial_Tributacao;
            _nfse.CodigoTributacaoMunicipio = _nfseBlumenau.Nota.Cd_Tributacao_Municipio;
            _nfse.OptanteSimplesNacional = _nfseBlumenau.Nota.Sn_Optante_Simples_Nacional != "0" ? XmlNfseSimNao.S : XmlNfseSimNao.N;
            _nfse.NumeroGuia = null; // nao tem;
            _nfse.DtPagamento = null; // nao tem;
            _nfse.VlrServicos = _nfseBlumenau.Nota.Vl_Servico;
            _nfse.CodigoVerificacao = _nfseBlumenau.Nota.Cd_Verificacao;
            _nfse.Iss.Aliquota = _nfseBlumenau.Nota.Vl_Aliquota;
            _nfse.Iss.Vlr = _nfseBlumenau.Nota.Vl_Iss;
            _nfse.MesCompetencia = _dataHoraEmissao.ToString("MM/yyyy");
            _nfse.ItemListaServico = _nfseBlumenau.Nota.Es_Item_Lista_Servico;
            _nfse.DadosTomador.Cnpj = _nfseBlumenau.Nota.Tom_Cpf_Cnpj;
            _nfse.DadosTomador.InscricaoMunicipal = _nfseBlumenau.Nota.Tom_Inscricao_Municipal;
            _nfse.DadosTomador.RazaoSocial = _nfseBlumenau.Nota.Tom_Razao_Social;
            _nfse.DadosTomador.Endereco.Endereco = _nfseBlumenau.Nota.Tom_Endereco;
            _nfse.DadosTomador.Endereco.Numero = _nfseBlumenau.Nota.Tom_Endereco_Numero;
            _nfse.DadosTomador.Endereco.Bairro = _nfseBlumenau.Nota.Tom_Endereco_Bairro;
            _nfse.DadosTomador.Endereco.Municipio = _nfseBlumenau.Nota.Tom_Endereco_Es_Municipio;
            _nfse.DadosTomador.Endereco.Uf = _nfseBlumenau.Nota.Tom_Endereco_Uf;
            _nfse.DadosTomador.Endereco.Cep = _nfseBlumenau.Nota.Tom_Endereco_Cep;
            _nfse.DiscriminacaoServico = _nfseBlumenau.Nota.Discriminacao;
            _nfse.DescricaoTipoServico = _nfseBlumenau.Nota.Discriminacao;
            _nfse.VlrTotal = decimal.Parse(_nfseBlumenau.Nota.Vl_Servico.Replace(".", ","));

            _nfse.Itens.Add(new XmlNfseServicoItem
            {
                Descricao = _nfse.DiscriminacaoServico,
                Qtde = "1",
                VlrUnitario = _nfseBlumenau.Nota.Vl_Servico,
                VlrTotal = _nfseBlumenau.Nota.Vl_Servico,
                Tributavel = XmlNfseSimNao.N
            });

            return _nfse;
        }
    }
}
