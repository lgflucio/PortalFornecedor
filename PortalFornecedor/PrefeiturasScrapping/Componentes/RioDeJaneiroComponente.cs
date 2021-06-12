using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PrefeiturasScrapping.Abstracts;
using PrefeiturasScrapping.Interfaces;
using Repository.Entities.RFE_ENTITIES;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils;
using WebDrivers.Interfaces;

namespace PrefeiturasScrapping.Componentes
{
    public class RioDeJaneiroComponente : AbDriver, IRioDeJaneiroComponente
    {
        protected override string BtnGerarXml { get; set; }
        protected override string LinkAcessoNFE { get; set; }
        protected override string Caminho { get; set; }
        protected override string UrlNFSE { get; set; }
        private string BtnAcessoAoSistema { get; set; }
        private string BtnPossuiCertificadoDigital { get; set; }
        private string BtnAcessarOSistema { get; set; }
        private string BtnConsultasDeNotas { get; set; }
        private string BtnNotasRecebidas { get; set; }
        private string BtnConsultar { get; set; }
        private string BtnFiltrar { get; set; }
        private string ComboTomadorDeServiços { get; set; }
        private string BtnVoltarParaConsulta { get; set; }
        private string GridDeNotasVazia { get; set; }
        private List<long?> NFSEs { get; set; }
        private string PathArquivoBaixado { get; set; }
        private string DataInicial { get; set; }
        private string DataFinal { get; set; }
        private string CampoDataInicial { get; set; }
        private string CampoDataFinal { get; set; }
        private string Paginacao { get; set; }


        public RioDeJaneiroComponente()
        {

        }

        public RioDeJaneiroComponente(RFE_CIA cia, IWebDriverFactory webDriverFactory, int periodoConsulta, string diretorioDownload = "") : base(cia, webDriverFactory)
        {
            try
            {
                BtnAcessoAoSistema = "//*[@id='ctl00_CAB_controleLogin_hlAcesso']";
                BtnPossuiCertificadoDigital = "//*[@id='ctl00_cphCabMenu_hlLoginICP']";
                BtnAcessarOSistema = "//*[@id='ctl00_cphCabMenu_btAcesso']";
                BtnConsultasDeNotas = "#ctl00_Vcab_mnuRotinas tr td table tbody tr td a[href='/contribuinte/Consulta.aspx']";
                BtnNotasRecebidas = "//*[@id='ctl00_cphCabMenu_btnAbaNFRecebida']";
                BtnConsultar = "//*[@id='ctl00_cphCabMenu_btConsultarNFRecebida']";
                BtnGerarXml = "//*[@id='ctl00_cphBase_lkbWSNacional']";
                BtnFiltrar = "//*[@id='ctl00_cphCabMenu_ctrlFiltros_btFiltrar']";
                LinkAcessoNFE = "#ctl00_cphCabMenu_pnResultadoConteudo table tbody tr td a[target][href]";
                UrlNFSE = "https://notacarioca.rio.gov.br/capa.aspx";
                Caminho = diretorioDownload;
                ComboTomadorDeServiços = ".conteudoAba .wlegend .grid-11-12 #ctl00_cphCabMenu_ddlContribuinte";
                BtnVoltarParaConsulta = @"//*[@id='ctl00_cphCabMenu_btVoltar']";
                GridDeNotasVazia = "//*[@id='ctl00_cphCabMenu_gvNotas']/tbody/tr/td/div";
                CampoDataInicial = "//*[@id='ctl00_cphCabMenu_ctrlFiltros_ctrPeriodo_tbInicio']";
                CampoDataFinal = "//*[@id='ctl00_cphCabMenu_ctrlFiltros_ctrPeriodo_tbFim']";
                DataInicial = DateTime.Now.AddDays(-periodoConsulta).ToString("dd/MM/yyyy");
                DataFinal = DateTime.Now.ToString("dd/MM/yyyy");
                Paginacao = "//*[@id='ctl00_cphCabMenu_gvNotas']/tbody/tr[103]/td/table/tbody/tr/td/a";

                CarregarDriver();
                RealizarPassosDeClicks(Driver, cia.CNPJ);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                Driver.Quit();
            }
        }

        public void NovaThread(IWebDriver driver)
        {
            Thread.Sleep(10000);

            var _wndSO = new WndSO();
            var _processesName = driver.GetType().Name.Replace("Driver", "");
            var _tituloJanela = "NOTA CARIOCA - Nota Fiscal de Serviços Eletrônica - NFS-e - Prefeitura da Cidade do Rio de Janeiro - Google Chrome";
            var _nomeEmpresa = "PETRO RIO OEG EXPLORACAO E PRODUCAO DE PETROLEO L:11058804000168";
            _wndSO.AcessaSelecaoDeCertificados(_processesName, _tituloJanela, _nomeEmpresa);
        }
        public override void RealizarPassosDeClicks(IWebDriver driver, string cnpj)
        {
            driver.FindElement(By.XPath(BtnAcessoAoSistema)).Click();

            /*=======================================================================================*/
            // dispara uma nova thread para executar
            Thread _secondTread = new Thread(() => NovaThread(driver));
            _secondTread.Start();
            /*=======================================================================================*/

            Task.Delay(5000);

            //Logar com Certificado
            driver.FindElement(By.XPath(BtnPossuiCertificadoDigital)).Click();

            driver.FindElement(By.XPath(BtnAcessarOSistema)).Click();
            driver.SwitchTo().Window(driver.CurrentWindowHandle);
            driver.FindElement(By.CssSelector(BtnConsultasDeNotas)).Click();
            driver.FindElement(By.XPath(BtnNotasRecebidas)).Click();

            string[] removerDoCombo =
            {
                "Selecione o contribuinte desejado...",
                "Sem Inscrição"
            };

            List<string> tomadoresParaSelecionar = new List<string>();

            List<IWebElement> comboTomadores = driver.FindElements(By.CssSelector(ComboTomadorDeServiços + " option")).ToList();

            foreach (var combo in comboTomadores)
            {
                if (!removerDoCombo.Contains(combo.Text))
                    tomadoresParaSelecionar.Add(combo.Text);
            }

            foreach (var tomador in tomadoresParaSelecionar)
            {
                SelectElement selectTomadores = new SelectElement(driver.FindElement(By.CssSelector(ComboTomadorDeServiços)));
                selectTomadores.SelectByText(tomador);
                driver.FindElement(By.XPath(BtnConsultar)).Click();
                driver.FindElement(By.XPath(CampoDataInicial)).Clear();
                driver.FindElement(By.XPath(CampoDataFinal)).Clear();
                driver.FindElement(By.XPath(CampoDataInicial)).SendKeys(DataInicial);
                driver.FindElement(By.XPath(CampoDataFinal)).SendKeys(DataFinal);
                driver.FindElement(By.XPath(BtnFiltrar)).Click();
                bool existeNFse = driver.FindElements(By.XPath(GridDeNotasVazia)).Count() == 0;
                if (existeNFse)
                {
                    var paginas = driver.FindElements(By.XPath(Paginacao));
                    ProcessarRaspagemDeNotas(driver, LinkAcessoNFE, BtnGerarXml, cnpj);

                    for (int i = 0; i < paginas.Count; i++)
                    {
                        try
                        {
                            paginas[i].Click();
                        }
                        catch (StaleElementReferenceException e)
                        {
                            paginas = driver.FindElements(By.XPath(Paginacao));
                            paginas[i].Click();
                        }

                        ProcessarRaspagemDeNotas(driver, LinkAcessoNFE, BtnGerarXml, cnpj);
                    }
                }
                driver.FindElement(By.XPath(BtnVoltarParaConsulta)).Click();
            }
        }

        public void ProcessarRaspagemDeNotas(IWebDriver driver, string linkAcessoNFE, string btnGerarXml, string cnpj)
        {
            IReadOnlyList<IWebElement> links = driver
              .FindElements(By
              .CssSelector(linkAcessoNFE));

            string pastaCliente = $@"{Caminho}\{cnpj}";
            if (!Directory.Exists(pastaCliente))
                Directory.CreateDirectory(pastaCliente);

            using var watcher = new FileSystemWatcher(pastaCliente);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Filter = "*.xml";
            watcher.EnableRaisingEvents = true;

            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            foreach (var link in links)
            {
                long? nf = long.Parse(link.Text);

                bool ErroNoLink = false;
                try
                {
                    link.Click();
                    ErroNoLink = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"NF {nf.ToString()}: Não foi realizar o click para está NF. Exception: {e.Message}");
                }

                if (ErroNoLink)
                {
                    var homeWindow = driver.WindowHandles;
                    driver.SwitchTo().Window(homeWindow[1]);

                    bool xmlDisponivel = false;
                    try
                    {
                        driver.FindElement(By.XPath(btnGerarXml)).Click();
                        xmlDisponivel = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"NF {nf.ToString()}: O XML não está disponivel para download. Exception: {e.Message}");
                    }

                    driver.Close();
                    driver.SwitchTo().Window(homeWindow[0]);

                    if (xmlDisponivel)
                    {
                        bool arquivoEstaBaixado = false;
                        string pathArquivoBaixado = "";
                        int contador = 0;
                        do
                        {
                            contador += 1;
                            arquivoEstaBaixado = !PathArquivoBaixado.IsNullOrEmpty();
                            if (arquivoEstaBaixado)
                            {
                                pathArquivoBaixado = PathArquivoBaixado;
                                PathArquivoBaixado = "";
                            }

                        } while (!arquivoEstaBaixado || contador == 10);

                        if (arquivoEstaBaixado)
                        {
                            Console.WriteLine("NF: " + nf + " - CONFIRMADA!");
                        }
                        else
                        {
                            Console.WriteLine($"Error: Não foi possivel confirmar o download da NF: {nf}");
                        }
                    }
                }
            }
        }

        private static void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e) => PathArquivoBaixado = e.FullPath;
    }
}
