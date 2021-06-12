using BreakCaptcha.Interfaces;
using BreakCaptcha.Responses;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PrefeiturasScrapping.Interfaces;
using Repository.DTOs;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.Interfaces;
using Services.Mapper;
using Services.ViewModels;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using WebDrivers.Interfaces;
using WebDrivers.Model;
using static Shared.Enums.Enums;

namespace PrefeiturasScrapping.Services
{
    public class ManausAppService : IManausAppService
    {
        private readonly IWebDriverFactory _webDriverFactorys;
        private readonly INfseRepository _repositoryNfse;
        private readonly INfseAppService _serviceNfse;
        private readonly ILogRepository _repositoryLog;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly IItemNfseRepository _repositoryItemNfse;
        private readonly IMapperObj _mapperObj;
        private readonly IBreakCaptchaFactory _factoryBreakCaptcha;
        private readonly IFirefoxSetup _firefoxSetup;
        private WebDriverWait _wait;

        public ManausAppService(IWebDriverFactory webDriverFactorys,
                                INfseRepository repositoryNfse,
                                INfseAppService serviceNfse,
                                ILogRepository repositoryLog,
                                IHistoricoRepository repositoryHistorico,
                                IItemNfseRepository repositoryItemNfse,
                                IFirefoxSetup firefoxSetup,
                                IMapperObj mapperObj,
                                IBreakCaptchaFactory factoryBreakCaptcha)
        {
            _webDriverFactorys = webDriverFactorys;
            _repositoryNfse = repositoryNfse;
            _serviceNfse = serviceNfse;
            _repositoryLog = repositoryLog;
            _repositoryHistorico = repositoryHistorico;
            _repositoryItemNfse = repositoryItemNfse;
            _mapperObj = mapperObj;
            _firefoxSetup = firefoxSetup;
            _factoryBreakCaptcha = factoryBreakCaptcha;
        }

        public void Get(int periodoConsulta)
        {
            _repositoryLog.Create(new RFE_LOG
            {
                EDITOR = "GSW",
                MODIFICADO = DateTime.Now,
                Mensagem = "Serviço de busca automática no municipio de Manaus iniciado.",
                Servico = Servico.Busca_Automatica_NFSE_Municipio,
                TipoLog = TipoLog.Informacao
            });

            List<RFE_NFSE> _nfsesBD = _repositoryNfse.GetByMunicipio(CodigoIbgeDTO.Manaus);
            List<XmlNfseViewModel> _newNfses = GetNfsesByUserAndPassword("45985371000108", "3msourcing", periodoConsulta);
            List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();

            _newNfses.ForEach(nfse =>
            {
                RFE_NFSE _nfseRfe = _mapperObj.MapToNfse(nfse);
                _nfseRfe.MUNGER = CodigoIbgeDTO.Manaus;

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
                        DESCRICAO = "NFSe inserida através de busca automática Raspagem de Dados Manaus.",
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

        public List<XmlNfseViewModel> GetNfsesByUserAndPassword(string cnpj, string password, int periodoConsulta)
        {
            RFE_LOG _log;
            IWebDriver _driver = null;
            List<XmlNfseViewModel> _newNfses = new List<XmlNfseViewModel>();
            int _nroNfseDownloadTrys = 0;

            while (true)
            {
                try
                {
                    _driver = _webDriverFactorys.GetChromeDriver();

                    _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 60));

                    GoToLoginPage(_driver);
                    SelecItemCombobox("vTPOPESSOA", "Jurídica");
                    LogIntoAndBreakCaptcha(_driver, cnpj, password);
                    GoToNfseList(_driver);

                    string dtIni = DateTime.Now.AddDays(-periodoConsulta).ToString("dd/MM/yyyy");
                    string dtFim = DateTime.Now.ToString("dd/MM/yyyy");

                    List<LinkNFSe> _nfsesLinks = GetNfsesLinks(_driver, dtIni, dtFim);

                    if (_nfsesLinks.Count > 0)
                    {
                        _newNfses = ProcessaListaNFSEs(_driver, _nfsesLinks);
                    }

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

        private void GoToLoginPage(IWebDriver _driver)
        {
            _driver.Navigate().GoToUrl("https://nfse-prd.manaus.am.gov.br/nfse/servlet/hlogin");
        }

        private void SelecItemCombobox(string id, string option)
        {
            IWebElement _combobox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(id)));

            IEnumerable<IWebElement> _opcoes = _combobox.FindElements(By.CssSelector("option"));

            _opcoes.Where(o => o.Text.Contains(option)).First().Click();
        }

        private void LogIntoAndBreakCaptcha(IWebDriver driver, string cnpj, string password)
        {
            Task<ResponseSolved> _captcha;
            _wait.Until(c => c.FindElement(By.Id("vUSULOGIN"))).SendKeys(cnpj);
            _wait.Until(c => c.FindElement(By.Id("vSENHA"))).SendKeys(password);

            string _imageBase64 = Util.GetImageCaptcha(driver, By.XPath("//*[@id=\"vIMAGEM_0001\"]"));

            string _idInserted = _factoryBreakCaptcha.CaptchaProcessing(_imageBase64).Result;

            do
            {
                Thread.Sleep(20000);
                _captcha = _factoryBreakCaptcha.CaptchaSolved(_idInserted);
            }
            while (!_captcha.Result.captchaSolved);

            if (string.IsNullOrEmpty(_captcha.Result.captchaText))
            {
                GoToLoginPage(driver);
                SelecItemCombobox("vTPOPESSOA", "Jurídica");
                LogIntoAndBreakCaptcha(driver, cnpj, password);

                return;
            }

            //Inseriu o captcha quebrado.
            _wait.Until(c => c.FindElement(By.Id("vVALORIMAGEM"))).SendKeys(_captcha.Result.captchaText);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("BUTTON1"))).Click();
        }

        private void GoToNfseList(IWebDriver _driver)
        {
            Thread.Sleep(2000);
            _wait.Until(c => c.FindElement(By.Id("apy0m0i4"))).Click();

            Thread.Sleep(300);
            _wait.Until(c => c.FindElement(By.Id("apy0m9i1TR"))).Click();
        }

        private List<LinkNFSe> GetNfsesLinks(IWebDriver driver, string dataInicio, string dataFim)
        {
            _wait.Until(c => c.FindElement(By.Id("vDTAINI"))).SendKeys(dataInicio);
            _wait.Until(c => c.FindElement(By.Id("vDTAFIM"))).SendKeys(dataFim);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("BUTTON3"))).Click();

            return GetNfsesLinks(driver);
        }

        private List<LinkNFSe> GetNfsesLinks(IWebDriver driver)
        {
            Thread.Sleep(2000);
            List<LinkNFSe> _listaDeLinksNFSe = new List<LinkNFSe>();


            IWebElement table = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("GridContainerTbl")));
            ScrollElementIntoView(driver, table);

            int _quantidadeDePaginas = GetQuantidadeDePaginas();

            for (int i = 0; i < _quantidadeDePaginas; i++)
            {
                int pagina = i + 1;
                SelecItemCombobox("vPAGINAS", pagina.ToString());

                table = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("GridContainerTbl")));
                _listaDeLinksNFSe.AddRange(ConvertTableHtmlToList(table, pagina));
            }

            return _listaDeLinksNFSe;
        }

        private List<LinkNFSe> ConvertTableHtmlToList(IWebElement table, int pagina)
        {
            List<LinkNFSe> _listaDeLinksNFSe = new List<LinkNFSe>();

            //Obtêm linhas da tabela
            IEnumerable<IWebElement> _linhasTabela = table.FindElements(By.TagName("tr"));

            _linhasTabela.ToList().ForEach(l =>
            {
                LinkNFSe _linkNFSe = new LinkNFSe();

                //Obtêm colunas de cada linha
                IEnumerable<IWebElement> _colunasTabela = l.FindElements(By.TagName("td"));

                // colindex="9"
                if (_colunasTabela.Count() > 0)
                {
                    IWebElement _colunaVisualizar = _colunasTabela.Where(o => o.GetAttribute("colindex").ToString().Equals("9")).First();
                    IWebElement _input = _colunaVisualizar.FindElements(By.TagName("input")).FirstOrDefault();

                    _linkNFSe.Pagina = pagina;
                    _linkNFSe.Id = _input.GetAttribute("id");

                    _listaDeLinksNFSe.Add(_linkNFSe);
                }
            });

            return _listaDeLinksNFSe;
        }

        private void ScrollElementIntoView(IWebDriver _driver, IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scroll(" + element.Location.X + "," + (element.Location.Y - 200) + ");");
        }

        private int GetQuantidadeDePaginas()
        {
            IWebElement _paginador = _wait.Until(c => c.FindElement(By.Id("vPAGINAS")));

            IEnumerable<IWebElement> _opcoes = _paginador.FindElements(By.CssSelector("option"));

            return _opcoes.Count();
        }

        private List<XmlNfseViewModel> ProcessaListaNFSEs(IWebDriver driver, List<LinkNFSe> nfsesLinks)
        {
            List<XmlNfseViewModel> _nfses = new List<XmlNfseViewModel>();
            nfsesLinks.ForEach(nfseLink =>
            {
                SelecItemCombobox("vPAGINAS", nfseLink.Pagina.ToString());

                Thread.Sleep(1000);

                _wait.Until(c => c.FindElement(By.Id(nfseLink.Id))).Click();
                XmlNfseViewModel xmlNfseViewModel = ConvertPageObjectsToXmlDefault(driver);
                _nfses.Add(xmlNfseViewModel);
            });

            return _nfses;
        }

        private XmlNfseViewModel ConvertPageObjectsToXmlDefault(IWebDriver driver)
        {
            Thread.Sleep(5000);

            XmlNfseViewModel _nfse = new XmlNfseViewModel();

            // DADOS DA NOTA
            _nfse.Numero = _wait.Until(c => c.FindElement(By.Id("span_vNFSNUMERO"))).Text;

            var _dataEmissao = _wait.Until(c => c.FindElement(By.Id("span_NFSDATAGERA"))).Text;

            _nfse.DtEmissao = _dataEmissao;

            _nfse.MesCompetencia = _dataEmissao.Substring(3, 7);

            _nfse.CodigoVerificacao = _wait.Until(c => c.FindElement(By.Id("span_vCODVER"))).Text;

            // PRESTADOR DE SERVIÇOS
            _nfse.DadosPrestador.Cnpj = Util.DesformatarCNPJCPF(_wait.Until(c => c.FindElement(By.Id("span_NFSPRTCPFCNPJMASC"))).Text);
            _nfse.DadosPrestador.RazaoSocial = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTNOME"))).Text;
            _nfse.DadosPrestador.InscricaoMunicipal = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTCTCCMC"))).Text;
            _nfse.DadosPrestador.InscricaoEstadual = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTCTBINSCEST"))).Text;
            _nfse.DadosPrestador.Endereco.Endereco = _wait.Until(c => c.FindElement(By.Id("span_vNFSPRTNOMLOG"))).Text;
            _nfse.DadosPrestador.Endereco.Numero = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTCTCNMR"))).Text;
            _nfse.DadosPrestador.Endereco.Cep = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTCTCCEP"))).Text;
            _nfse.DadosPrestador.Endereco.Bairro = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTNOMBAIRRO"))).Text;
            _nfse.DadosPrestador.Endereco.Municipio = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTCTCMUNICIPIO"))).Text;
            _nfse.DadosPrestador.Endereco.Uf = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTCTBUF"))).Text;
            _nfse.DadosPrestador.Endereco.Pais = _wait.Until(c => c.FindElement(By.Id("span_NFSPRTCTBPAIS"))).Text;

            // TOMADOR DE SERVIÇOS
            _nfse.DadosTomador.Cnpj = Util.DesformatarCNPJCPF(_wait.Until(c => c.FindElement(By.Id("span_NFSTOMCPFCNPJMASCFRM"))).Text);
            _nfse.DadosTomador.RazaoSocial = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMNOMEFRM"))).Text;
            _nfse.DadosTomador.InscricaoMunicipal = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMCTCCMCFRM"))).Text;
            _nfse.DadosTomador.InscricaoEstadual = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMCTBINSCESTFRM"))).Text;
            _nfse.DadosTomador.Endereco.Endereco = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMNOMLOGFRM"))).Text;
            _nfse.DadosTomador.Endereco.Numero = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMCTCNMRFRM"))).Text;
            _nfse.DadosTomador.Endereco.Cep = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMCTCCEPFRM"))).Text;
            _nfse.DadosTomador.Endereco.Bairro = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMNOMBAIRROFRM"))).Text;
            _nfse.DadosTomador.Endereco.Municipio = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMCTCMUNICIPIOFRM"))).Text;
            _nfse.DadosTomador.Endereco.Uf = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMENDUFFRM"))).Text;
            _nfse.DadosTomador.Endereco.Pais = _wait.Until(c => c.FindElement(By.Id("span_NFSTOMENDPAISFRM"))).Text;

            // TRIBUTAÇÃO
            string _localPrestacaoServico = _wait.Until(c => c.FindElement(By.Id("span_NFSLOCPRESTSRV"))).Text;
            _nfse.LocalPrestacao = _localPrestacaoServico.Equals("No Município") ? "MANAUS/AM" : _localPrestacaoServico;
            _nfse.Tributacao = _wait.Until(c => c.FindElement(By.Id("span_NFSOPERACAO"))).Text;

            // SERVIÇO
            _nfse.DescricaoTipoServico = _wait.Until(c => c.FindElement(By.Id("span_vSRVDESCRICAO_0001"))).Text;
            _nfse.VlrServicos = _wait.Until(c => c.FindElement(By.Id("span_NFIVLRSRV_0001"))).Text.Replace(".", "");
            _nfse.VlrTotal = _wait.Until(c => c.FindElement(By.Id("span_NFIVLRTOTAL_0001"))).Text.ToDecimal();
            _nfse.VlrDeducoes = _wait.Until(c => c.FindElement(By.Id("span_NFIVLRDED_0001"))).Text.Replace(".", "");
            _nfse.Iss.BaseCalculo = _wait.Until(c => c.FindElement(By.Id("span_NFIBASCLC_0001"))).Text.Replace(".", "");
            _nfse.Iss.Aliquota = _wait.Until(c => c.FindElement(By.Id("span_NFIALIQUOTA_0001"))).Text;
            _nfse.Iss.Vlr = _wait.Until(c => c.FindElement(By.Id("span_NFIVLRISS_0001"))).Text.Replace(".", "");

            // DESCRIÇÃO GERAL DO SERVIÇO
            _nfse.DiscriminacaoServico = _wait.Until(c => c.FindElement(By.Id("span_vNFSDSCGERAL"))).Text;

            // OUTRAS INFORMAÇÕES
            _nfse.OutrasInformacoes = _wait.Until(c => c.FindElement(By.Id("OUTRASINFORMACOES"))).Text;

            var xml = Util.ConverterObjetoXMlEmXML<XmlNfseViewModel>(_nfse);
            _nfse.XmlOriginal = xml.OuterXml;

            _nfse.Itens.Add(new XmlNfseServicoItem
            {
                Descricao = _nfse.DiscriminacaoServico,
                Qtde = "1",
                Tributavel = _localPrestacaoServico.Equals("No Município") ? XmlNfseSimNao.S : XmlNfseSimNao.N,
                VlrTotal = _nfse.VlrTotal.ToString(),
                VlrUnitario = _nfse.VlrTotal.ToString()
            });

            IWebElement _btnVoltar = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("BUTTON3")));
            ScrollElementIntoView(driver, _btnVoltar);
            _btnVoltar.Click();

            return _nfse;
        }
    }
}
