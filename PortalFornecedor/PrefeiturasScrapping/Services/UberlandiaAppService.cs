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
using System.Linq;
using System.Threading;
using Utils;
using WebDrivers.Interfaces;
using WebDrivers.Model;
using static Shared.Enums.Enums;

namespace PrefeiturasScrapping.Services
{
    public class UberlandiaAppService : IUberlandiaAppService
    {
        private readonly IWebDriverFactory _webDriverFactorys;
        private readonly INfseRepository _repositoryNfse;
        private readonly INfseAppService _serviceNfse;
        private readonly ILogRepository _repositoryLog;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly IItemNfseRepository _repositoryItemNfse;
        private readonly IMapperObj _mapperObj;
        private WebDriverWait _wait;

        public UberlandiaAppService(IWebDriverFactory webDriverFactorys,
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
        }

        public void Get(int periodoConsulta)
        {
            _repositoryLog.Create(new RFE_LOG
            {
                EDITOR = "GSW",
                MODIFICADO = DateTime.Now,
                Mensagem = "Serviço de busca automática no municipio de Uberlândia iniciado.",
                Servico = Servico.Busca_Automatica_NFSE_Municipio,
                TipoLog = TipoLog.Informacao
            });

            List<XmlNfseViewModel> _newNfses = GetNfsesByCertificate(periodoConsulta);
            List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();

            _newNfses.ForEach(nfse =>
            {
                RFE_NFSE _nfseRfe = _mapperObj.MapToNfse(nfse);
                _nfseRfe.MUNGER = CodigoIbgeDTO.Uberlandia;

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
                        DESCRICAO = "NFSe inserida através de busca automática Raspagem de Dados de Uberlândia.",
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

        public List<XmlNfseViewModel> GetNfsesByCertificate(int periodoConsulta)
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
                    AcessarSistema(_driver);
                    AcessarViaCertificado(_driver);

                    DateTime _dtIniCompetencia = DateTime.Now.AddDays(-periodoConsulta);
                    DateTime _dtFimCompetencia = DateTime.Now;

                    ConsultaNFSEsRecebidas(_driver, _dtIniCompetencia, _dtFimCompetencia);

                    List<LinkNFSe> _nfsesLinks = ObtemListaDeNFSEsRecebidas(_driver);

                    _newNfses = ProcessaListaNFSEs(_driver, _nfsesLinks);

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
            driver.Navigate().GoToUrl("https://udigital.uberlandia.mg.gov.br/NotaFiscal/");
        }

        private void AcessarSistema(IWebDriver driver)
        {
            IWebElement _framePrincipal = _wait.Until(c => c.FindElement(By.Id("principal")));
            driver.SwitchTo().Frame(_framePrincipal);

            _wait.Until(c => c.FindElement(By.XPath("//*[@id=\"coluna1\"]/div/div[2]/ul/li[2]/a"))).Click();
        }

        private void AcessarViaCertificado(IWebDriver driver)
        {
            var _btnAbrirAcessoCertificado = _wait.Until(c => c.FindElement(By.XPath("//*[@id=\"divCertificadoDigital\"]/input")));
            ScrollElementIntoView(driver, _btnAbrirAcessoCertificado);
            _btnAbrirAcessoCertificado.Click();

            // Seleciona Certificado
            Thread.Sleep(3000);

            var _wndSO = new WndSO();
            var _processesName = driver.GetType().Name.Replace("Driver", "");
            var _tituloJanela = "Nota Fiscal de Serviços Eletrônica - Google Chrome";
            var _nomeEmpresa = "3M DO BRASIL LTDA";

            _wndSO.AcessaSelecaoDeCertificados(_processesName, _tituloJanela, _nomeEmpresa);

            Thread.Sleep(1000);

            _wndSO.FechaDialogCertificados(_processesName, _tituloJanela, _nomeEmpresa);
        }

        private void ConsultaNFSEsRecebidas(IWebDriver driver, DateTime dtIniCompetencia, DateTime dtFimCompetencia)
        {
            Thread.Sleep(3000);

            _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[2]/div[9]/ul/li/a"))).Click();

            string _mesIniCompetencia = dtIniCompetencia.ToString("MM");
            SelecItemCombobox("rMesCompetenciaCN", _mesIniCompetencia);

            string _anoIniCompetencia = dtIniCompetencia.ToString("yyyy");
            SelecItemCombobox("rAnoCompetenciaCN", _anoIniCompetencia);

            string _mesFimCompetencia = dtFimCompetencia.ToString("MM");
            SelecItemCombobox("rMesCompetenciaCN2", _mesFimCompetencia);

            string _anoFimCompetencia = dtFimCompetencia.ToString("yyyy");
            SelecItemCombobox("rAnoCompetenciaCN2", _anoFimCompetencia);

            _wait.Until(c => c.FindElement(By.Id("btnConsultar"))).Click();
        }

        private List<LinkNFSe> ObtemListaDeNFSEsRecebidas(IWebDriver driver)
        {
            Thread.Sleep(1000);

            //Localiza Grid de NFSEs
            int count = 0;
            IWebElement _table = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]"))).FirstOrDefault();
            while (_table == null && count < 50)
            {
                _table = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]"))).FirstOrDefault();
                ++count;
                Thread.Sleep(100);
            }

            ScrollElementIntoView(driver, _table);

            //Localiza Paginação do Grid
            IWebElement _paginacao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/table")));

            //Próximo
            IEnumerable<IWebElement> _proximo = _paginacao.FindElements(By.TagName("a")).Where(o => o.Text.Contains("Próximo"));

            List<LinkNFSe> _nfsesLinks = new List<LinkNFSe>();

            //Enquanto botão "Próximo" estiver visível, converte tabela e avança para próxima
            count = 0;
            while (_proximo.Count() > 0 && count < 50)
            {
                Thread.Sleep(1000);

                //Localiza Grid de Mensagens
                _table = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]"))).FirstOrDefault();

                _nfsesLinks.AddRange(ConvertTableHtmlToList(_table));

                if (_proximo.Count() > 0)
                {
                    ScrollElementIntoView(driver, _proximo.First());
                    _proximo.First().Click();

                    Thread.Sleep(1000);

                    _paginacao = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/table"))).FirstOrDefault();
                    _proximo = _paginacao.FindElements(By.TagName("a")).Where(e => e.Text.Contains("Próximo"));
                }

                ++count;
            }

            //Localiza Grid de Mensagens
            _table = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]"))).FirstOrDefault();

            _nfsesLinks.AddRange(ConvertTableHtmlToList(_table));

            //Vai para Primeira Página
            _paginacao = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/table"))).FirstOrDefault();
            IEnumerable<IWebElement> _primeiraPagina = _paginacao.FindElements(By.TagName("a")).Where(o => o.Text.Contains("1"));

            if (_primeiraPagina.Count() > 0)
            {
                ScrollElementIntoView(driver, _primeiraPagina.First());
                _primeiraPagina.First().Click();
            }

            return _nfsesLinks;
        }

        private List<LinkNFSe> ConvertTableHtmlToList(IWebElement table)
        {
            List<LinkNFSe> _nfsesLinks = new List<LinkNFSe>();

            //Obtêm linhas da tabela
            IEnumerable<IWebElement> _linhasTabela = table.FindElements(By.TagName("tr"));

            _linhasTabela.ToList().ForEach(l =>
            {
                var _nfse = new LinkNFSe();

                //Obtêm colunas de cada linha
                IEnumerable<IWebElement> _colunasTabela = l.FindElements(By.TagName("td"));
                if (_colunasTabela.Count() > 8 && !_colunasTabela.ElementAt(0).Text.Equals("NFSe"))
                {
                    IWebElement _link = _colunasTabela.ElementAt(0).FindElement(By.TagName("a"));
                    _nfse.Id = _link.Text;
                }

                if ((_nfse.Id != null) && (_nfse.Id != " "))
                {
                    _nfsesLinks.Add(_nfse);
                }
            });

            return _nfsesLinks;
        }

        private List<XmlNfseViewModel> ProcessaListaNFSEs(IWebDriver driver, List<LinkNFSe> nfsesLinks)
        {
            List<XmlNfseViewModel> _nfses = new List<XmlNfseViewModel>();
            var availableWindows = driver.WindowHandles;

            nfsesLinks.ForEach(l =>
            {
                Thread.Sleep(1000);

                //Localiza Grid de NFSEs
                int count = 0;
                IWebElement _table = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]"))).FirstOrDefault();
                while (_table == null && count < 50)
                {
                    _table = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]"))).FirstOrDefault();
                    ++count;
                    Thread.Sleep(100);
                }

                ScrollElementIntoView(driver, _table);

                //Link da NFSE
                IEnumerable<IWebElement> _links = _table.FindElements(By.TagName("a")).Where(o => o.Text.Contains(l.Id));

                //Localiza Paginação do Grid
                IWebElement _paginacao = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/table"))).FirstOrDefault();

                //Próximo
                IEnumerable<IWebElement> _proximo = _paginacao.FindElements(By.TagName("a")).Where(o => o.Text.Contains("Próximo"));

                while (_links.Count() == 0 && _proximo.Count() > 0)
                {
                    Thread.Sleep(1000);

                    if (_proximo.Count() > 0)
                    {
                        ScrollElementIntoView(driver, _proximo.First());
                        _proximo.First().Click();

                        Thread.Sleep(1000);

                        _paginacao = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/table"))).FirstOrDefault();
                        _proximo = _paginacao.FindElements(By.TagName("a")).Where(e => e.Text.Contains("Próximo"));

                        //Localiza Grid de Mensagens
                        count = 0;
                        _table = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]"))).FirstOrDefault();
                        while (_table == null && count < 50)
                        {
                            _table = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/div/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]"))).FirstOrDefault();
                            ++count;
                            Thread.Sleep(100);
                        }

                        _links = _table.FindElements(By.TagName("a")).Where(o => o.Text.Contains(l.Id));
                    }
                }

                ScrollElementIntoView(driver, _links.FirstOrDefault());
                _links.FirstOrDefault().Click();

                _nfses.Add(ConvertPageObjectsToXmlDefault(driver));

                driver.Close();
                driver.SwitchTo().Window(availableWindows[0]);

                IWebElement _framePrincipal = _wait.Until(c => c.FindElement(By.Id("principal")));
                driver.SwitchTo().Frame(_framePrincipal);
            });

            return _nfses;
        }

        private XmlNfseViewModel ConvertPageObjectsToXmlDefault(IWebDriver driver)
        {
            Thread.Sleep(1000);

            driver.SwitchTo().Window(driver.WindowHandles.Last());

            XmlNfseViewModel _nfse = new XmlNfseViewModel();

            // DADOS DA NOTA
            _nfse.Numero = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[1]/tbody/tr/td[2]/table/tbody/tr[2]/td"))).Text;
            _nfse.DtEmissao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[1]/tbody/tr/td[2]/table/tbody/tr[4]/td"))).Text;
            _nfse.CodigoVerificacao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[1]/tbody/tr/td[2]/table/tbody/tr[6]/td"))).Text;

            // PRESTADOR DE SERVIÇOS
            _nfse.DadosPrestador.Cnpj = Util.DesformatarCNPJCPF(_wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[2]/tbody/tr[3]/td[1]/span"))).Text);
            _nfse.DadosPrestador.RazaoSocial = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[2]/tbody/tr[2]/td[2]/span"))).Text;
            _nfse.DadosPrestador.InscricaoMunicipal = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[2]/tbody/tr[3]/td[2]/span"))).Text.Replace(".", "").Replace("-", "");
            _nfse.DadosPrestador.Endereco.Endereco = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[2]/tbody/tr[4]/td/span"))).Text;
            _nfse.DadosPrestador.Endereco.Municipio = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[2]/tbody/tr[5]/td[1]/span"))).Text;
            _nfse.DadosPrestador.Endereco.Uf = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[2]/tbody/tr[5]/td[2]/span"))).Text;

            // TOMADOR DE SERVIÇOS
            _nfse.DadosTomador.Cnpj = Util.DesformatarCNPJCPF(_wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[3]/tbody/tr[3]/td/span"))).Text);
            _nfse.DadosTomador.RazaoSocial = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[3]/tbody/tr[2]/td/span"))).Text;
            _nfse.DadosTomador.Endereco.Endereco = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[3]/tbody/tr[4]/td/span"))).Text;
            _nfse.DadosTomador.Endereco.Municipio = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[3]/tbody/tr[5]/td[1]/span"))).Text;
            _nfse.DadosTomador.Endereco.Uf = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[3]/tbody/tr[5]/td[2]/span"))).Text;

            // SERVIÇO
            if (!_wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[5]"))).Text.Equals(" "))
            {
                _nfse.VlrServicos = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[5]"))).Text.Replace(".", "");
                _nfse.VlrTotal = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[5]"))).Text.Replace(".", "").ToDecimal();
            }
            else if (!_wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[5]"))).Text.Equals(" "))
            {
                _nfse.VlrServicos = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[5]"))).Text.Replace(".", "");
                _nfse.VlrTotal = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[5]"))).Text.Replace(".", "").ToDecimal();
            }

            //TRIBUTAÇÃO
            if (_wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr[2]/td[1]/span"))).Count > 0)
            {
                _nfse.ImpostosRetidos.VlrPisPasep = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr[2]/td[1]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.ImpostosRetidos.VlrCofins = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr[2]/td[2]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.ImpostosRetidos.VlrInss = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr[2]/td[3]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.ImpostosRetidos.VlrIrrf = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr[2]/td[4]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.ImpostosRetidos.VlrCsll = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr[2]/td[5]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
            }
            else if (_wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr/td[1]/span"))).Text.Equals("R$ "))
            {
                _nfse.ImpostosRetidos.VlrPisPasep = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr/td[1]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.ImpostosRetidos.VlrCofins = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr/td[2]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.ImpostosRetidos.VlrInss = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr/td[3]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.ImpostosRetidos.VlrIrrf = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr/td[4]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.ImpostosRetidos.VlrCsll = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[5]/tbody/tr/td[5]/span"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
            }

            if (_wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[7]/tbody/tr/td[2]/table/tbody/tr[2]/td"))).Count() > 0)
            {
                _nfse.VlrDeducoes = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[7]/tbody/tr/td[1]/table/tbody/tr[2]/td"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.Iss.BaseCalculo = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[7]/tbody/tr/td[2]/table/tbody/tr[2]/td"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.Iss.Aliquota = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[7]/tbody/tr/td[3]/table/tbody/tr[2]/td"))).Text.Replace("%", "").Replace("*", "").Replace(".", "").Replace(" ", "");
                _nfse.Iss.Vlr = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[7]/tbody/tr/td[4]/table/tbody/tr[2]/td"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
            }
            else if (_wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr/td[2]/table/tbody/tr[2]/td"))).Count() > 0)
            {
                _nfse.VlrDeducoes = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr/td[1]/table/tbody/tr[2]/td"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.Iss.BaseCalculo = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr/td[2]/table/tbody/tr[2]/td"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
                _nfse.Iss.Aliquota = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr/td[3]/table/tbody/tr[2]/td"))).Text.Replace("%", "").Replace("*", "").Replace(".", "").Replace(" ", "");
                _nfse.Iss.Vlr = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr/td[4]/table/tbody/tr[2]/td"))).Text.Replace("R$ *", "").Replace("R$ ", "").Replace(".", "");
            }

            _nfse.Iss.BaseCalculo = _nfse.Iss.BaseCalculo.Equals("") ? "0" : _nfse.Iss.BaseCalculo;
            _nfse.Iss.Aliquota = _nfse.Iss.Aliquota.Equals("") ? "0" : _nfse.Iss.Aliquota;
            _nfse.Iss.Vlr = _nfse.Iss.Vlr.Equals("") ? "0" : _nfse.Iss.Vlr;

            // DESCRIÇÃO GERAL DO SERVIÇO
            if (_wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[2]/td/div/font"))).Count() > 0)
            {
                _nfse.DiscriminacaoServico = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[2]/td/div/font"))).Text;
            }

            // OUTRAS INFORMAÇÕES
            if (_wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[2]/td[1]/span"))).Count() > 0)
            {
                _nfse.MesCompetencia = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[2]/td[1]/span"))).Text;
                _nfse.Tributacao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[2]/td[2]/span"))).Text;
                _nfse.LocalPrestacao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[3]/td[1]/span"))).Text;
                _nfse.MunicipioIncidencia = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[3]/td[2]/span"))).Text;
                _nfse.Recolhimento = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[4]/td[2]/span"))).Text;
                _nfse.CNAE = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[5]/td[1]/span"))).Text;
                _nfse.DescricaoTipoServico = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[6]/td/span"))).Text;
            }
            else if (_wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[2]/td[1]/span"))).Count() > 0)
            {
                _nfse.MesCompetencia = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[2]/td[1]/span"))).Text.Replace(".", "");
                _nfse.Tributacao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[2]/td[2]/span"))).Text;
                _nfse.LocalPrestacao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[3]/td[1]/span"))).Text;
                _nfse.MunicipioIncidencia = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[3]/td[2]/span"))).Text;
                _nfse.Recolhimento = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[4]/td[2]/span"))).Text;
                _nfse.CNAE = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[5]/td[1]/span"))).Text;
                _nfse.DescricaoTipoServico = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[6]/td/span"))).Text;
            }

            if (_wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[4]/td[1]/span"))).Count() > 0)
            {
                _nfse.Iss.DtVenc = _wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[8]/tbody/tr[4]/td[1]/span"))).FirstOrDefault().Text;
            }
            else if (_wait.Until(c => c.FindElements(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[4]/td[1]/span"))).Count() > 0)
            {
                _nfse.Iss.DtVenc = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[9]/tbody/tr[4]/td[1]/span"))).Text.Replace(".", "");
            }

            // ITEM DO SERVIÇO
            if (!_wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[1]"))).Text.Equals(" "))
            {
                string _tributavel = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[1]"))).Text;

                _nfse.Itens.Add(new XmlNfseServicoItem
                {
                    Tributavel = _tributavel.Equals("SIM") ? XmlNfseSimNao.S : XmlNfseSimNao.N,
                    Descricao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[2]"))).Text,
                    Qtde = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[3]"))).Text,
                    VlrUnitario = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[4]"))).Text.Replace(".", ""),
                    VlrTotal = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[4]/td[5]"))).Text.Replace(".", "")
                });
            }
            else if (!_wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[1]"))).Text.Equals(" "))
            {
                string _tributavel = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[1]"))).Text;

                _nfse.Itens.Add(new XmlNfseServicoItem
                {
                    Tributavel = _tributavel.Equals("SIM") ? XmlNfseSimNao.S : XmlNfseSimNao.N,
                    Descricao = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[2]"))).Text,
                    Qtde = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[3]"))).Text,
                    VlrUnitario = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[4]"))).Text.Replace(".", ""),
                    VlrTotal = _wait.Until(c => c.FindElement(By.XPath("/html/body/table/tbody/tr/td/table/tbody/tr/td/div/table/tbody/tr[3]/td/span/div[2]/div[1]/table[4]/tbody/tr[3]/td[5]"))).Text.Replace(".", "")
                });
            }

            var xml = Util.ConverterObjetoXMlEmXML<XmlNfseViewModel>(_nfse);
            _nfse.XmlOriginal = xml.OuterXml;

            return _nfse;
        }

        private void ScrollElementIntoView(IWebDriver _driver, IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scroll(" + element.Location.X + "," + (element.Location.Y - 200) + ");");
        }

        private void SelecItemCombobox(string id, string option)
        {
            IWebElement _combobox = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(id)));

            IEnumerable<IWebElement> _opcoes = _combobox.FindElements(By.CssSelector("option"));

            _opcoes.Where(o => o.Text.Contains(option)).First().Click();
        }
    }
}
