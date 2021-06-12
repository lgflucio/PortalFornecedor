using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace Utils
{
    public class WndSO
    {
        private InputSimulator inp = new InputSimulator();

        /// <summary>
        /// Anexa uma janela do Firefox conforme o Título da Janela
        /// </summary>
        /// <param name="tituloJanela"></param>
        /// <returns></returns>
        public Application AttachToProcess(string processesName, string tituloJanela)
        {
            try
            {
                Thread.Sleep(2000);

                IEnumerable<Process> processes = Process.GetProcessesByName(processesName);
                int count = 0;
                var procImprimir = processes.Where(p => p.MainWindowTitle == tituloJanela).FirstOrDefault();
                while (procImprimir == null && count < 50)
                {
                    procImprimir = processes.Where(p => p.MainWindowTitle == tituloJanela).FirstOrDefault();
                    ++count;

                    Thread.Sleep(100);
                }

                var processoId = procImprimir.Id;

                var name = procImprimir.MainWindowTitle;

                var app = FlaUI.Core.Application.Attach(processoId);

                return app;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método AttachToProcess da WndSO: " + Environment.NewLine + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : String.Empty));
            }
        }

        /// <summary>
        /// Abre o menu do Firefox e acessa a opção Imprimir
        /// </summary>
        /// <param name="processesName"></param>
        /// <param name="tituloJanela"></param>
        public void AbrirImpressao(string processesName, string tituloJanela)
        {
            try
            {
                var app = AttachToProcess(processesName, tituloJanela);
                using (var automation = new UIA3Automation())
                {
                    var windowFirefox = app.GetMainWindow(automation);

                    var btnOpcoes = windowFirefox.FindFirstDescendant(cf => cf.ByName("Firefox").And(cf.ByControlType(ControlType.Button))).AsButton();
                    btnOpcoes.Click();

                    Thread.Sleep(2000);

                    var btnImprimir = windowFirefox.FindFirstDescendant(cf => cf.ByName("Imprimir…").And(cf.ByControlType(ControlType.Button))).AsButton();
                    btnImprimir.Click(true);

                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método AbrirImpressao da WndSO: " + Environment.NewLine + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : String.Empty));
            }
        }

        /// <summary>
        /// Acessa a Janela de Impressão do Firefox
        /// </summary>
        /// <param name="tituloJanela"></param>
        /// <param name="caminhoNomeArquivo"></param>
        public void AcessaJanelaImpressaoFlaUI(string processesName, string tituloJanela, string caminhoNomeArquivo)
        {
            try
            {
                Thread.Sleep(1000);

                var app = AttachToProcess(processesName, tituloJanela);
                using (var automation = new UIA3Automation())
                {
                    var windowFirefox = app.GetMainWindow(automation);
                    int count = 0;
                    var wndImprimir = windowFirefox.FindFirstDescendant(cf => cf.ByText("Imprimir"))?.AsWindow();
                    while (wndImprimir == null && count < 50)
                    {
                        wndImprimir = windowFirefox.FindFirstDescendant(cf => cf.ByText("Imprimir"))?.AsWindow();
                        ++count;

                        Thread.Sleep(100);
                    }

                    var destino = windowFirefox.FindFirstDescendant(cf => cf.ByName("Destino").And(cf.ByControlType(ControlType.ComboBox)))?.AsComboBox();
                    count = 0;
                    while (destino == null && count < 50)
                    {
                        destino = windowFirefox.FindFirstDescendant(cf => cf.ByName("Destino").And(cf.ByControlType(ControlType.ComboBox)))?.AsComboBox();
                        ++count;

                        Thread.Sleep(100);
                    }

                    var opcao = destino.Value;

                    count = 0;
                    while (!opcao.Equals("Salvar como PDF") && count < 50)
                    {
                        destino.Expand();
                        inp.Keyboard.KeyPress(VirtualKeyCode.UP);
                        inp.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                        destino.Collapse();
                        opcao = destino.Value;
                        ++count;

                        Thread.Sleep(100);
                    }

                    count = 0;
                    var btnSalvar = windowFirefox.FindFirstDescendant(cf => cf.ByText("Salvar"))?.AsButton();
                    while (btnSalvar == null && count < 50)
                    {
                        btnSalvar = windowFirefox.FindFirstDescendant(cf => cf.ByText("Salvar"))?.AsButton();
                        ++count;

                        Thread.Sleep(100);
                    }

                    Thread.Sleep(4000);

                    btnSalvar.Click();
                }

                app.Dispose();

                Thread.Sleep(5000);

                var tituloDialog = "Salvar como";
                AcessaDlgSalvarComoFlaUI(processesName, caminhoNomeArquivo, tituloJanela, tituloDialog);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método AcessaJanelaImpressaoFlaUI da WndSO: " + Environment.NewLine + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : String.Empty));
            }
        }

        /// <summary>
        /// Acessa o Diálogo "Salvar Como..." do Firefox
        /// </summary>
        /// <param name="processesName"></param>
        /// <param name="caminhoNomeArquivo"></param>
        /// <param name="tituloJanela"></param>
        /// <param name="tituloDialog"></param>
        public void AcessaDlgSalvarComoFlaUI(string processesName, string caminhoNomeArquivo, string tituloJanela, string tituloDialog)
        {
            try
            {
                var app = AttachToProcess(processesName, tituloJanela);
                using (var automation = new UIA3Automation())
                {
                    var windowFirefox = app.GetMainWindow(automation);

                    int count = 0;
                    var dlgSalvarComo = windowFirefox.FindFirstDescendant(cf => cf.ByText(tituloDialog))?.AsWindow();
                    while (dlgSalvarComo == null && count < 50)
                    {
                        dlgSalvarComo = windowFirefox.FindFirstDescendant(cf => cf.ByText(tituloDialog))?.AsWindow();
                        ++count;

                        Thread.Sleep(100);
                    }

                    var lstCampoNome = dlgSalvarComo.FindAllDescendants(cf => cf.ByName("Nome:"));
                    var campoNome = lstCampoNome[lstCampoNome.Length - 1].AsTextBox();

                    campoNome.Enter(caminhoNomeArquivo);

                    count = 0;
                    var btnSalvarComo = dlgSalvarComo.FindFirstDescendant(cf => cf.ByText("Salvar"))?.AsButton();
                    while (btnSalvarComo == null && count < 50)
                    {
                        btnSalvarComo = dlgSalvarComo.FindFirstDescendant(cf => cf.ByText("Salvar"))?.AsButton();
                        ++count;

                        Thread.Sleep(100);
                    }

                    btnSalvarComo.Focus();
                    btnSalvarComo.Click();

                    Thread.Sleep(4000);

                    var dlgConfirmaSalvarComo = windowFirefox.FindFirstDescendant(cf => cf.ByText("Confirmar Salvar como"))?.AsWindow();

                    if (dlgConfirmaSalvarComo != null)
                    {
                        var btnConfirmaSim = dlgConfirmaSalvarComo.FindFirstDescendant(cf => cf.ByText("Sim"))?.AsButton();
                        btnConfirmaSim.Focus();
                        btnConfirmaSim.Click();
                    }
                }

                Thread.Sleep(5000);

                app.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método AcessaDlgSalvarComoFlaUI da WndSO: " + Environment.NewLine + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : String.Empty));
            }
        }

        public void AcessaSelecaoDeCertificados(string processesName, string tituloJanela, string nomeEmpresa)
        {
            try
            {
                Thread.Sleep(1000);

                var app = AttachToProcess(processesName, tituloJanela);

                using (var automation = new UIA3Automation())
                {
                    var windowChrome = app.GetMainWindow(automation);
                    windowChrome.FocusNative();

                    int count = 0;
                    var dlgCertificado = windowChrome.FindFirstDescendant(cf => cf.ByName("Selecione um certificado").Or(cf.ByName("Select a certificate")))?.AsWindow();
                    while (dlgCertificado == null && count < 50)
                    {
                        dlgCertificado = windowChrome.FindFirstDescendant(cf => cf.ByName("Selecione um certificado").Or(cf.ByName("Select a certificate")))?.AsWindow();
                        ++count;

                        Thread.Sleep(100);
                    }

                    var lstItens = dlgCertificado.FindAllDescendants(cf => cf.ByControlType(ControlType.DataItem));
                    var item = dlgCertificado.FindFirstDescendant(cf => cf.ByText(nomeEmpresa))?.AsGridRow();
                    
                    var scrollbar = dlgCertificado.FindFirstDescendant(cf => cf.ByControlType(ControlType.ScrollBar));
                    var scrollBtns = scrollbar.FindAllDescendants(cf => cf.ByControlType(ControlType.Button));

                    count = 0;
                    while (item == null && count+1 < lstItens.Length)
                    {
                        Mouse.MoveTo(scrollBtns.ElementAt(1).BoundingRectangle.Location);
                        Mouse.LeftClick();

                        item = dlgCertificado.FindFirstDescendant(cf => cf.ByText(nomeEmpresa))?.AsGridRow();
                    }

                    Mouse.MoveTo(item.BoundingRectangle.Location);
                    Thread.Sleep(100);
                    Mouse.LeftClick();

                    var btnOk = dlgCertificado.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();

                    Mouse.MoveTo(btnOk.BoundingRectangle.Location);
                    Thread.Sleep(100);
                    Mouse.LeftClick();
                }

                app.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método AcessaSelecaoDeCertificados da WndSO: " + Environment.NewLine + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : String.Empty));
            }
        }

        /// <summary>
        /// Fechar Janela do Firefox
        /// </summary>
        /// <param name="tituloJanela"></param>
        public void FecharJanelaFlaUI(string processesName, string tituloJanela)
        {
            try
            {
                var app = AttachToProcess(processesName, tituloJanela);
                using (var automation = new UIA3Automation())
                {
                    var windowFirefox = app.GetMainWindow(automation);
                    windowFirefox.Close();
                }

                app.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método FecharJanelaFlaUI da WndSO: " + Environment.NewLine + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : String.Empty));
            }
        }

        /// <summary>
        /// Fechar Janela do Firefox
        /// </summary>
        /// <param name="tituloJanela"></param>
        public void FecharDialogoFlaUI(string processesName, string tituloJanela)
        {
            try
            {
                var app = AttachToProcess(processesName, tituloJanela);
                using (var automation = new UIA3Automation())
                {
                    var windowFirefox = app.GetMainWindow(automation);

                    var count = 0;
                    var btnOk = windowFirefox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
                    while (btnOk == null && count < 50)
                    {
                        btnOk = windowFirefox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
                        ++count;

                        Thread.Sleep(100);
                    }

                    btnOk.Focus();
                    btnOk.Click();
                }

                app.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método FecharDialogoFlaUI da WndSO: " + Environment.NewLine + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : String.Empty));
            }
        }

        public void FechaDialogCertificados(string processesName, string tituloJanela, string nomeEmpresa)
        {
            try
            {
                var app = AttachToProcess(processesName, tituloJanela);

                using (var automation = new UIA3Automation())
                {
                    var windowChrome = app.GetMainWindow(automation);
                    windowChrome.FocusNative();

                    var dlgCertificado = windowChrome.FindFirstDescendant(cf => cf.ByName("udigital.uberlandia.mg.gov.br diz").Or(cf.ByName("udigital.uberlandia.mg.gov.br says")))?.AsWindow();

                    var btnOk = dlgCertificado.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();

                    Mouse.MoveTo(btnOk.BoundingRectangle.Location);
                    Mouse.LeftClick();
                }

                app.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no método AcessaSelecaoDeCertificados da WndSO: " + Environment.NewLine + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : String.Empty));
            }
        }
    }
}
