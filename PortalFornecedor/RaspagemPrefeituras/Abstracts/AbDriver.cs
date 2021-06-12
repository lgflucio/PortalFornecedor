using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RaspagemPrefeituras.ArquivosNFSe.Interfaces;
using System;

namespace RaspagemPrefeituras.Abstracts
{
    public abstract class AbDriver
    {
        protected abstract string BtnGerarXml { get; set; }
        protected abstract string LinkAcessoNFE { get; set; }
        protected abstract string Caminho { get; set; }
        protected abstract string UrlNFSE { get; set; }

        public AbDriver()
        {
        }

        public void CarregarDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            chromeOptions.AddUserProfilePreference("download.default_directory", Caminho);
            IWebDriver driver = new ChromeDriver(chromeOptions);
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Url = UrlNFSE;
            driver.Manage().Window.Maximize();

            //var ba = driver.FindElement(By.CssSelector("html frameset #principal html body #fundopopup"));

            RealizarPassosDeClicks(driver);

            Console.WriteLine("Driver Carregado..." + DateTime.Now.ToString());
        }

        public abstract void RealizarPassosDeClicks(IWebDriver driver);
    }
}
