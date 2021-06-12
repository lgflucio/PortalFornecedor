using BreakCaptcha.Interfaces;
using BreakCaptcha.Responses;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Repository.DTOs;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.Interfaces;
using Services.Mapper;
using Services.ViewModels;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebDrivers.Interfaces;
using static Shared.Enums.Enums;
using ICampinasAppService = PrefeiturasScrapping.Interfaces.ICampinasAppService;

namespace PrefeiturasScrapping.Services
{
    public class CampinasAppService : ICampinasAppService
    {
        private readonly IWebDriverFactory _webDriverFactorys;
        private readonly INfseRepository _repositoryNfse;
        private readonly INfseAppService _serviceNfse;
        private readonly ILogRepository _repositoryLog;
        private readonly IHistoricoRepository _repositoryHistorico;
        private readonly IItemNfseRepository _repositoryItemNfse;
        private readonly IBreakCaptchaFactory _factoryBreakCaptcha;
        private readonly IMapperObj _mapperObj;
        public CampinasAppService(IWebDriverFactory webDriverFactorys,
                                  INfseRepository repositoryNfse,
                                  INfseAppService serviceNfse,
                                  IHistoricoRepository repositoryHistorico,
                                  IItemNfseRepository repositoryItemNfse,
                                  ILogRepository repositoryLog,
                                  IBreakCaptchaFactory factoryBreakCaptcha,
                                  IMapperObj mapperObj)
        {
            _webDriverFactorys = webDriverFactorys;
            _repositoryNfse = repositoryNfse;
            _serviceNfse = serviceNfse;
            _repositoryHistorico = repositoryHistorico;
            _repositoryItemNfse = repositoryItemNfse;
            _factoryBreakCaptcha = factoryBreakCaptcha;
            _repositoryLog = repositoryLog;
            _mapperObj = mapperObj;
        }
        public void Get()
        {
            RFE_LOG _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = "Serviço de busca automática no municipio de Campinas iniciado.", Servico = Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao };
            _repositoryLog.Create(_log);

            List<XmlNfseViewModel> _nfses = GetNfsesByUserAndPassword("45985371000108", "66BC66D5");
            List<RFE_NFSE> _nfsesRFE = new List<RFE_NFSE>();
            _nfses.ForEach(nfse =>
            {
                RFE_NFSE _nfseCast = _mapperObj.MapToNfse(nfse);
                _nfseCast.MUNGER = CodigoIbgeDTO.Campinas;
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
                        DESCRICAO = "NFSe inserida através de busca automática WebService Campinas.",
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

        public List<XmlNfseViewModel> GetNfsesByUserAndPassword(string user, string password)
        {
            RFE_LOG _log;
            IWebDriver _driver = null;
            List<XmlNfseViewModel> _newNfses = new List<XmlNfseViewModel>();
            int nroNfseDownloadTrys = 0;

            while (true)
            {
                try
                {
                    _driver = _webDriverFactorys.GetFireFoxDriver(@"C:/");
                    _driver = GoToLoginPage(_driver);
                    _driver = FillCaptcha(_driver);
                    _driver = LogIntoAndBreakCaptcha(_driver, user, password);
                    _driver = GoToNfseList(_driver);
                    List<string> _nfsesLinks = GetNfsesLinks(_driver);
                    _newNfses = ConvertXmlToXmlDefault(_driver, _nfsesLinks);

                    break;
                }
                catch (Exception e)
                {
                    _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = $"{e.Message}", Servico = Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Erro };
                    _repositoryLog.Create(_log);
                    if (_driver != null)
                        _driver.Close();
                    nroNfseDownloadTrys++;
                    if (nroNfseDownloadTrys > 4)
                        break;
                }
            }

            _log = new RFE_LOG() { EDITOR = "GSW", MODIFICADO = DateTime.Now, Mensagem = $"{_newNfses.Count} raspadas com sucesso, iniciar inserção das notas na base de dados do RFE.", Servico = Servico.Busca_Automatica_NFSE_Municipio, TipoLog = TipoLog.Informacao };
            _repositoryLog.Create(_log);
            return _newNfses;

        }

        private IWebDriver GoToLoginPage(IWebDriver _driver)
        {
            _driver.Navigate().GoToUrl("https://nfse.campinas.sp.gov.br/NotaFiscal/index.php");
            _driver.SwitchTo().Frame(_driver.FindElement(By.Id("principal")));
            if (_driver.FindElements(By.Id("mensagem0")).Count > 0)
            {
                _driver.FindElement(By.Id("mensagem0"));
                Thread.Sleep(500);
                _driver.FindElement(By.XPath("//img")).Click();
                Thread.Sleep(1000);
            }
            _driver.FindElements(By.TagName("a")).FirstOrDefault(el => el.GetAttribute("href").Contains("acessoSistema.php")).Click();
            Thread.Sleep(2000);
            return _driver;
        }

        private IWebDriver FillCaptcha(IWebDriver _driver)
        {
            Task<ResponseSolved> _captcha;
            //Bitmap img =  WebDrivers.Utils.GetImgCaptcha(_driver, By.XPath("//div[@id='coluna5B']/form/table/tbody/tr[3]/td[4]/img"));
            //string textCaptcha = ImgProcess(img);
            //_driver.FindElement(By.Id("rSelo")).Click();
            //_driver.FindElement(By.Id("rSelo")).Clear();
            //string imgbase64 = Utils.Util.GetImageCaptcha(_driver, By.XPath("//div[@id='coluna5B']/form/table/tbody/tr[3]/td[4]/img"));
            //string _idInserted = _factoryBreakCaptcha.CaptchaProcessing(imgbase64).Result;
            //do
            //{
            //    Thread.Sleep(10000);
            //    _captcha = _factoryBreakCaptcha.CaptchaSolved(_idInserted);
            //}
            //while (!_captcha.Result.captchaSolved && (string.IsNullOrEmpty(_captcha.Result.captchaText) && _captcha.Result.captchaSolved));

            //_driver.FindElement(By.Id("rSelo")).SendKeys(_captcha.Result.captchaText);
            return _driver;
        }

        private IWebDriver LogIntoAndBreakCaptcha(IWebDriver _driver, string user, string password)
        {
            _driver.FindElement(By.Id("rLogin")).SendKeys(user);
            _driver.FindElement(By.Id("rSenha")).SendKeys(password);
            _driver.FindElement(By.Id("btnEntrar")).Click();

            return _driver;
        }

        private IWebDriver GoToNfseList(IWebDriver _driver)
        {
            _driver.FindElement(By.LinkText("NFSe Recebidas")).Click();
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("rMesCompetenciaCN")).Click();
            new SelectElement(_driver.FindElement(By.Id("rMesCompetenciaCN"))).SelectByText(DateTime.Now.Month.ToString().Length == 1 ? DateTime.Now.Month.ToString().PadLeft(2, '0') : DateTime.Now.Month.ToString());
            _driver.FindElement(By.Id("rMesCompetenciaCN")).Click();
            new SelectElement(_driver.FindElement(By.Id("rAnoCompetenciaCN"))).SelectByText(DateTime.Now.Year.ToString());
            _driver.FindElement(By.Id("rAnoCompetenciaCN")).Click();
            _driver.FindElement(By.Id("rMesCompetenciaCN2")).Click();
            new SelectElement(_driver.FindElement(By.Id("rMesCompetenciaCN2"))).SelectByText(DateTime.Now.Month.ToString().Length == 1 ? DateTime.Now.Month.ToString().PadLeft(2, '0') : DateTime.Now.Month.ToString());
            _driver.FindElement(By.Id("rMesCompetenciaCN2")).Click();
            _driver.FindElement(By.Id("rAnoCompetenciaCN2")).Click();
            new SelectElement(_driver.FindElement(By.Id("rAnoCompetenciaCN2"))).SelectByText(DateTime.Now.Year.ToString());
            _driver.FindElement(By.Id("rAnoCompetenciaCN2")).Click();
            _driver.FindElement(By.Id("rComCancelada")).Click();
            _driver.FindElement(By.Id("btnConsultar")).Click();

            return _driver;
        }

        private List<string> GetNfsesLinks(IWebDriver _driver)
        {
            List<string> listaDeLinksNFSe = new List<string>();
            int nroPagina = 2;
            while (true)
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(120));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(".//tr[@class='gridResultado1']/td/a")));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(".//tr[@class='gridResultado2']/td/a")));

                //List<IWebElement> tabela = _driver.FindElements(By.ClassName("gridResultado1")).ToList();
                
                int totalLinhasTabelas = _driver.FindElements(By.ClassName("gridResultado1")).Count + _driver.FindElements(By.ClassName("gridResultado2")).Count + 3;
                for (int numeroLinha = 3; numeroLinha < totalLinhasTabelas; numeroLinha++)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath($"/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]/tbody/tr[{numeroLinha}]/td[2]")));
                    DateTime dataEmissao = DateTime.Parse(_driver.FindElement(By.XPath($"/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]/tbody/tr[{numeroLinha}]/td[2]"))
                        .GetAttribute("innerText"));
                    DateTime dataOntem = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).AddDays(-1);
                    int test = DateTime.Compare(dataEmissao, dataOntem);
                    int t = DateTime.Compare(dataEmissao, dataOntem);
                    if (DateTime.Compare(dataEmissao, dataOntem) == 0 || DateTime.Compare(dataEmissao, dataOntem) > 0) 
                    {

                        string linha = _driver.FindElement(By.XPath($"/html/body/table/tbody/tr/td/table/tbody/tr[2]/td/table/tbody/tr[3]/td[4]/span/form/table[2]/tbody/tr/td[2]/table[2]/tbody/tr[{numeroLinha}]/td[1]/a"))
                                        .GetAttribute("href");
                        listaDeLinksNFSe.Add(linha);
                    }
                        
                }
                if (_driver.FindElements(By.TagName("a")).ToList().Where(el => el.Text.Contains("Próximo")).Count() > 0)
                {
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(_driver.FindElements(By.TagName("a"))
                        .ToList()
                        .Where(el => el.Text.Contains("Próximo"))
                        .FirstOrDefault()));

                    _driver.FindElements(By.TagName("a"))
                        .ToList()
                        .Where(el => el.Text.Contains("Próximo"))
                        .FirstOrDefault()
                        .Click();

                    int tentativas = 0;
                    while (_driver.FindElements(By.TagName("a")).ToList().Where(el => el.Text.Equals((nroPagina - 1).ToString())).Count() == 0)
                    {
                        var testet = Task.Delay(22000);
                        while (!testet.IsCompleted) { }
                        if (_driver.FindElements(By.TagName("a")).ToList().Where(el => el.Text.Contains("Próximo")).Count() > 0)
                            _driver.FindElements(By.TagName("a"))
                            .ToList()
                            .Where(el => el.Text.Equals("Próximo"))
                            .FirstOrDefault()
                            .Click();
                        if (tentativas++ > 10)
                            throw new Exception($"Erro ao tentar ir para a pagina numero {nroPagina} da listagem NFSe em Campinas.");
                    }
                    nroPagina++;
                }
                else
                {
                    break;
                }
            }
            return listaDeLinksNFSe;
        }
        private List<XmlNfseViewModel> ConvertXmlToXmlDefault(IWebDriver _driver, List<string> _nfsesLinks)
        {
            List<XmlNfseViewModel> _newNfses = new List<XmlNfseViewModel>();
            _nfsesLinks.ForEach(link =>
            {
                _driver.Navigate().GoToUrl(link);
                _driver.SwitchTo().Window(_driver.WindowHandles.Last());
                XmlNfseViewModel xmlNfseViewModel = new XmlNfseViewModel();
                ReadOnlyCollection<IWebElement> listElementsInfo = GetElementsWithNfseInfo(_driver);

                xmlNfseViewModel = FillNfseIdentificationFields(xmlNfseViewModel, listElementsInfo);
                xmlNfseViewModel = FillPrestadorFields(xmlNfseViewModel, listElementsInfo);
                xmlNfseViewModel = FillTomadorFields(xmlNfseViewModel, listElementsInfo);
                xmlNfseViewModel = FillDescricaoFields(xmlNfseViewModel, listElementsInfo);
                xmlNfseViewModel = FillImpostosFields(xmlNfseViewModel, listElementsInfo);
                xmlNfseViewModel = FillOutrasInformacoesFields(xmlNfseViewModel, listElementsInfo);
                xmlNfseViewModel.XmlOriginal = Utils.Util.ConverterObjetoXMlEmXML<XmlNfseViewModel>(xmlNfseViewModel).OuterXml;
                _newNfses.Add(xmlNfseViewModel);

            });
            _driver.Quit();
            return _newNfses;
        }

        private XmlNfseViewModel FillNfseIdentificationFields(XmlNfseViewModel xmlNfseViewModel, ReadOnlyCollection<IWebElement> listElementsInfo)
        {
            List<string> listaCampos = new List<string>();
            listElementsInfo.Where(el => el.GetAttribute("innerText")
                        .Contains("Número da Nota"))
                        .FirstOrDefault()
                        .FindElements(By.ClassName("impressaoTitulo"))
                        .ToList()
                        .ForEach(el => listaCampos.Add(el.Text));

            xmlNfseViewModel.Id = listaCampos[2];
            xmlNfseViewModel.Numero = listaCampos[2];
            xmlNfseViewModel.DtEmissao = listaCampos[3];
            xmlNfseViewModel.DtPrestacaoServico = listaCampos[3];
            xmlNfseViewModel.CodigoVerificacao = listaCampos[4];

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel FillPrestadorFields(XmlNfseViewModel xmlNfseViewModel, ReadOnlyCollection<IWebElement> listElementsInfo)
        {

            List<string> listaCampos = new List<string>();
            listElementsInfo.Where(el => el.GetAttribute("innerText")
                        .Contains("PRESTADOR DE SERVIÇOS"))
                        .FirstOrDefault()
                        .FindElements(By.ClassName("impressaoLabel"))
                        .ToList()
                        .ForEach(el => listaCampos.Add(el.Text));

            xmlNfseViewModel.DadosPrestador = new XmlNfseDadosPrestador();
            string cnpjOuCpf = Utils.Util.DesformatarCNPJCPF(PegarValorCampoPorTitulo(listaCampos, "CPF/CNPJ:"));

            if (cnpjOuCpf.Length > 12)
            {
                xmlNfseViewModel.DadosPrestador.Cnpj = cnpjOuCpf;
                xmlNfseViewModel.DadosPrestador.Cpf = "";
            }
            else
            {
                xmlNfseViewModel.DadosPrestador.Cpf = cnpjOuCpf;
                xmlNfseViewModel.DadosPrestador.Cnpj = "";
            }
            xmlNfseViewModel.DadosPrestador.RazaoSocial = PegarValorCampoPorTitulo(listaCampos, "Nome/Razão Social:");
            xmlNfseViewModel.DadosPrestador.Nome = PegarValorCampoPorTitulo(listaCampos, "Nome/Razão Social:");
            xmlNfseViewModel.DadosPrestador.InscricaoMunicipal = PegarValorCampoPorTitulo(listaCampos, "Inscrição Municipal:");
            xmlNfseViewModel.DadosPrestador.Endereco = new XmlNfseEndereco();
            xmlNfseViewModel.DadosPrestador.Endereco.Endereco = PegarValorCampoPorTitulo(listaCampos, "Endereço:");
            xmlNfseViewModel.DadosPrestador.Endereco.Municipio = PegarValorCampoPorTitulo(listaCampos, "Município:");
            xmlNfseViewModel.DadosPrestador.Endereco.Uf = PegarValorCampoPorTitulo(listaCampos, "UF:").Replace(" ", string.Empty);
            xmlNfseViewModel.DadosPrestador.Contato = new XmlNfseContato();
            xmlNfseViewModel.DadosPrestador.Contato.Email = PegarValorCampoPorTitulo(listaCampos, "E-mail:");
            xmlNfseViewModel.DadosPrestador.Contato.Telefone = PegarValorCampoPorTitulo(listaCampos, "Telefone:");

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel FillTomadorFields(XmlNfseViewModel xmlNfseViewModel, ReadOnlyCollection<IWebElement> listElementsInfo)
        {
            List<string> listaCampos = new List<string>();
            listElementsInfo.Where(el => el.GetAttribute("innerText")
                       .Contains("TOMADOR DE SERVIÇOS"))
                       .FirstOrDefault()
                       .FindElements(By.ClassName("impressaoLabel"))
                       .ToList()
                       .ForEach(el => listaCampos.Add(el.Text));

            string cnpjOuCpf = Utils.Util.DesformatarCNPJCPF(PegarValorCampoPorTitulo(listaCampos, "CPF/CNPJ:"));
            xmlNfseViewModel.DadosTomador = new XmlNfseDadosTomador();
            if (cnpjOuCpf.Length > 11)
            {
                xmlNfseViewModel.DadosTomador.Cnpj = cnpjOuCpf;
                xmlNfseViewModel.DadosTomador.Cpf = "";
            }
            else
            {
                xmlNfseViewModel.DadosTomador.Cpf = cnpjOuCpf;
                xmlNfseViewModel.DadosTomador.Cnpj = "";
            }
            xmlNfseViewModel.DadosTomador.RazaoSocial = PegarValorCampoPorTitulo(listaCampos, "Nome/Razão Social:");
            xmlNfseViewModel.DadosTomador.Nome = PegarValorCampoPorTitulo(listaCampos, "Nome/Razão Social:");
            xmlNfseViewModel.DadosTomador.InscricaoMunicipal = PegarValorCampoPorTitulo(listaCampos, "Inscrição Municipal:");
            xmlNfseViewModel.DadosTomador.Endereco = new XmlNfseEndereco();
            xmlNfseViewModel.DadosTomador.Endereco.Endereco = PegarValorCampoPorTitulo(listaCampos, "Endereço:");
            xmlNfseViewModel.DadosTomador.Endereco.Municipio = PegarValorCampoPorTitulo(listaCampos, "Município:");
            xmlNfseViewModel.DadosTomador.Endereco.Uf = PegarValorCampoPorTitulo(listaCampos, "UF:").Replace(" ", string.Empty);
            xmlNfseViewModel.DadosTomador.Contato = new XmlNfseContato();
            xmlNfseViewModel.DadosTomador.Contato.Email = PegarValorCampoPorTitulo(listaCampos, "E-mail:");
            xmlNfseViewModel.DadosTomador.Contato.Telefone = PegarValorCampoPorTitulo(listaCampos, "Telefone:");

            return xmlNfseViewModel;
        }

        private XmlNfseViewModel FillDescricaoFields(XmlNfseViewModel xmlNfseViewModel, ReadOnlyCollection<IWebElement> listElementsInfo)
        {
            IWebElement tabelaDiscriminacaoServico = listElementsInfo.Where(el => el.GetAttribute("innerText").Contains("DISCRIMINAÇÃO DOS SERVIÇOS")).FirstOrDefault();
            List<string> listaCampos = new List<string>();
            tabelaDiscriminacaoServico.FindElements(By.ClassName("impressaoLabel")).ToList().ForEach(el => listaCampos.Add(el.Text));
            xmlNfseViewModel.DiscriminacaoServico = PegarValorCampoPorTitulo(listaCampos, "Descrição:");
            ReadOnlyCollection<IWebElement> listaDeItens = tabelaDiscriminacaoServico.FindElements(By.ClassName("impressaoCampo"));
            xmlNfseViewModel.OutrasInformacoes = tabelaDiscriminacaoServico.FindElement(By.ClassName("linhaItemNota")).Text;
            xmlNfseViewModel.Itens = new List<XmlNfseServicoItem>();

            foreach (var item in listaDeItens)
            {
                ReadOnlyCollection<IWebElement> valoresDoIten = item.FindElements(By.TagName("td"));
                XmlNfseServicoItem itemServico = new XmlNfseServicoItem();
                itemServico.Tributavel = valoresDoIten[0].GetAttribute("innerText").ToLower() == "sim" ? XmlNfseSimNao.S : XmlNfseSimNao.N;
                itemServico.Descricao = valoresDoIten[1].GetAttribute("innerText");

                string qtd = valoresDoIten[2].GetAttribute("innerText");
                if (decimal.TryParse(qtd, out var result))
                    qtd = Convert.ToInt32(result).ToString();

                itemServico.Qtde = qtd;
                itemServico.VlrUnitario = valoresDoIten[3].GetAttribute("innerText").Replace(".", string.Empty).Replace("*", "0");
                ;
                itemServico.VlrTotal = valoresDoIten[4].GetAttribute("innerText").Replace(".", string.Empty);
                xmlNfseViewModel.Itens.Add(itemServico);
            }
            return xmlNfseViewModel;
        }

        private XmlNfseViewModel FillImpostosFields(XmlNfseViewModel xmlNfseViewModel, ReadOnlyCollection<IWebElement> listElementsInfo)
        {
            List<string> listaCampos = new List<string>();
            listElementsInfo.Where(el => el.GetAttribute("innerText")
                       .Contains("PIS ("))
                       .FirstOrDefault()
                       .FindElements(By.TagName("td"))
                       .ToList()
                       .ForEach(el => listaCampos.Add(el.Text));
            //tabelaImpostosRetidos.FindElement(By.ClassName("impressaoLabel")).FindElements(By.TagName("td")).ToList().ForEach(el => listaCampos.Add(el.Text));
            xmlNfseViewModel.ImpostosRetidos = new XmlNfseImpostosRetidos();

            string campoPIS = listaCampos.Where(campo => campo.Contains("PIS")).FirstOrDefault();
            xmlNfseViewModel.ImpostosRetidos.VlrPisPasep = campoPIS.Split("R$")[1].Replace(".", string.Empty).Replace("*", "0");
            ;
            xmlNfseViewModel.ImpostosRetidos.AlqPisPasep = Utils.Util.PegarValorEntreParenteses(campoPIS, "%").Replace("*", "0");
            ;

            string campoCOFINS = listaCampos.Where(campo => campo.Contains("COFINS")).FirstOrDefault();
            xmlNfseViewModel.ImpostosRetidos.VlrCofins = campoCOFINS.Split("R$")[1].Replace(".", string.Empty).Replace("*", "0");
            ;
            xmlNfseViewModel.ImpostosRetidos.AlqCofins = Utils.Util.PegarValorEntreParenteses(campoCOFINS, "%").Replace("*", "0");
            ;

            string campoINSS = listaCampos.Where(campo => campo.Contains("INSS")).FirstOrDefault();
            xmlNfseViewModel.ImpostosRetidos.VlrInss = campoINSS.Split("R$")[1].Replace(".", string.Empty);
            xmlNfseViewModel.ImpostosRetidos.AlqInss = Utils.Util.PegarValorEntreParenteses(campoINSS, "%").Replace("*", "0");
            ;

            string campoIR = listaCampos.Where(campo => campo.Contains("IR")).FirstOrDefault();
            xmlNfseViewModel.ImpostosRetidos.VlrIrrf = campoIR.Split("R$")[1].Replace(".", string.Empty);
            xmlNfseViewModel.ImpostosRetidos.AlqIrrf = Utils.Util.PegarValorEntreParenteses(campoIR, "%").Replace("*", "0");
            ;

            string campoCSLL = listaCampos.Where(campo => campo.Contains("CSLL")).FirstOrDefault();
            xmlNfseViewModel.ImpostosRetidos.VlrCsll = campoCSLL.Split("R$")[1].Replace(".", string.Empty).Replace("*", "0");

            xmlNfseViewModel.ImpostosRetidos.AlqCsll = Utils.Util.PegarValorEntreParenteses(campoCSLL, "%").Replace("*", "0");
            ;

            ReadOnlyCollection<IWebElement> tabelaISSQN = listElementsInfo.Where(el => el.GetAttribute("innerText")
                                                                      .Contains("Alíquota Efetiva ISSQN"))
                                                                      .FirstOrDefault()
                                                                      .FindElement(By.TagName("tbody"))
                                                                      .FindElements(By.TagName("tbody"));

            xmlNfseViewModel.ImpostosRetidos.AlqIssRetido = tabelaISSQN.Where(el => el.Text.Contains("Alíquota Efetiva ISSQN:"))
                                                                            .FirstOrDefault()
                                                                            .FindElement(By.ClassName("impressaoCampo"))
                                                                            .Text
                                                                            .Replace("%", string.Empty)
                                                                            .Replace("*", "0");


            string campoVlrIssRetido = listaCampos.Where(campo => campo.Contains("ISSQN Devido")).FirstOrDefault();
            xmlNfseViewModel.ImpostosRetidos.VlrIssRetido = tabelaISSQN.Where(el => el.Text.Contains("ISSQN Devido"))
                                                                            .FirstOrDefault()
                                                                            .FindElement(By.ClassName("impressaoCampo"))
                                                                            .Text
                                                                            .Replace("R$", string.Empty)
                                                                            .Replace(".", string.Empty)
                                                                            .Replace("*", "0");
            ;
            xmlNfseViewModel.Iss = new XmlNfseImpostosIss();


            string campoVlrTotal = listElementsInfo.Where(el => el.Text.Contains("VALOR TOTAL DA NOTA")).FirstOrDefault().Text;
            xmlNfseViewModel.VlrTotal = decimal.Parse(campoVlrTotal.Split("R$")[1].Replace(".", string.Empty).Replace("*","0"));
                                                                                  
            xmlNfseViewModel.Iss.Aliquota = tabelaISSQN.Where(el => el.Text.Contains("Alíquota ISSQN"))
                                                                            .FirstOrDefault()
                                                                            .FindElement(By.ClassName("impressaoCampo"))
                                                                            .Text
                                                                            .Replace("%", string.Empty)
                                                                             .Replace("*", "0"); 

            xmlNfseViewModel.Iss.BaseCalculo = tabelaISSQN.Where(el => el.Text.Contains("Base de Cálculo ISSQN:"))
                                                                            .FirstOrDefault()
                                                                            .FindElement(By.ClassName("impressaoCampo"))
                                                                            .Text
                                                                            .Replace("R$", string.Empty)
                                                                            .Replace(".", string.Empty)
                                                                            .Replace("*","0");
                                                                            
            xmlNfseViewModel.Iss.Vlr = tabelaISSQN.Where(el => el.Text.Contains("ISSQN Devido"))
                                                                            .FirstOrDefault()
                                                                            .FindElement(By.ClassName("impressaoCampo"))
                                                                            .Text
                                                                            .Replace("R$", string.Empty)
                                                                            .Replace(".", string.Empty)
                                                                            .Replace("*", "0"); 
            return xmlNfseViewModel;
        }

        private XmlNfseViewModel FillOutrasInformacoesFields(XmlNfseViewModel xmlNfseViewModel, ReadOnlyCollection<IWebElement> listElementsInfo)
        {
            List<string> listaCampos = new List<string>();
            listElementsInfo.Where(el => el.GetAttribute("innerText")
                       .Contains("OUTRAS INFORMAÇÕES"))
                       .FirstOrDefault()
                       .FindElements(By.ClassName("impressaoLabel"))
                       .ToList()
                       .ForEach(el => listaCampos.Add(el.Text));
            //tabelaOutrasInfomacoes.FindElements(By.ClassName("impressaoLabel")).ToList().ForEach(el => listaCampos.Add(el.Text));
            xmlNfseViewModel.CNAE = PegarValorCampoPorTitulo(listaCampos, "CNAE:");
            xmlNfseViewModel.DescricaoAtividade = PegarValorCampoPorTitulo(listaCampos, "Descrição da Atividade:");
            xmlNfseViewModel.DescricaoTipoServico = listaCampos.Where(campo => campo.Contains("Serviço:") && !campo.Contains("Local da Prestação")).FirstOrDefault().Replace("Serviço:", string.Empty);
            xmlNfseViewModel.ItemListaServico = xmlNfseViewModel.DescricaoTipoServico.Split("-")[0];
            xmlNfseViewModel.MesCompetencia = PegarValorCampoPorTitulo(listaCampos, "Mês de Competência da Nota Fiscal:");
            xmlNfseViewModel.LocalPrestacao = PegarValorCampoPorTitulo(listaCampos, "Local da Prestação do Serviço:");

            xmlNfseViewModel.Iss.DtVenc = PegarValorCampoPorTitulo(listaCampos, "Data de vencimento do ISSQN referente a esta NFSe:");

            string stringWithFields = listaCampos.Where(campo => campo.Contains("convertido em NFSe em"))
                                    .FirstOrDefault();

            if (stringWithFields != null && !stringWithFields.Equals(""))//verifica se o campo com informações de RSP existe, para evitar NullPointException
            {
                var camposRSP = stringWithFields.Replace("RPS", string.Empty)
                                                .Replace("SÉRIE", string.Empty)
                                                .Replace(", convertido em NFSe em", string.Empty)
                                                .Split(" ");
                xmlNfseViewModel.Rps = new XmlNfseRps();
                xmlNfseViewModel.Rps.Numero = camposRSP[1];
                xmlNfseViewModel.Rps.Serie = camposRSP[3];
                xmlNfseViewModel.Rps.DtEmissao = camposRSP[4];
            }
            return xmlNfseViewModel;
        }

        private ReadOnlyCollection<IWebElement> GetElementsWithNfseInfo(IWebDriver _driver)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("divPrincipal")));
            Thread.Sleep(2000);
            IWebElement divPrincipal = _driver.FindElement(By.ClassName("divPrincipal"));
            return divPrincipal.FindElements(By.ClassName("impressaoTabela"));
        }
        private bool IsNfseAlreadyInBD(ReadOnlyCollection<IWebElement> _listElementsInfo, List<RFE_NFSE> _nfses)
        {
            List<string> fieldsList = new List<string>();
            _listElementsInfo.Where(el => el.GetAttribute("innerText")
                        .Contains("PRESTADOR DE SERVIÇOS"))
                        .FirstOrDefault()
                        .FindElements(By.ClassName("impressaoLabel"))
                        .ToList()
                        .ForEach(el => fieldsList.Add(el.Text));

            fieldsList = new List<string>();
            _listElementsInfo.Where(el => el.GetAttribute("innerText")
                        .Contains("Número da Nota"))
                        .FirstOrDefault()
                        .FindElements(By.ClassName("impressaoTitulo"))
                        .ToList()
                        .ForEach(el => fieldsList.Add(el.Text));

            string cnpjPrestador = PegarValorCampoPorTitulo(fieldsList, "CPF/CNPJ:").Trim(new char[] { ' ', '/', '.', '-' });
            string dtEmissao = fieldsList[3];
            string nroNfse = fieldsList[2];
            return _nfses.Count != 0
                   && _nfses.Find(nfse => nfse.CHNFSE == Utils.Util.GerarCampoChaveTabelaRFE(dtEmissao, cnpjPrestador, nroNfse)) != null
                   ? true
                   : false;
        }

        private string PegarValorCampoPorTitulo(List<string> fieldsList, string nomeCampo)
        {
            string valorCampo = fieldsList.Where(campo => campo.Contains(nomeCampo)).FirstOrDefault();
            valorCampo = valorCampo == null ? "" : valorCampo.Replace(nomeCampo, string.Empty);
            return valorCampo;
        }
    }
}
