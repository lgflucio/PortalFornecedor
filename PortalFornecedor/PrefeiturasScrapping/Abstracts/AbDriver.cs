using OpenQA.Selenium;
using Repository.Entities.RFE_ENTITIES;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using WebDrivers.Interfaces;

namespace PrefeiturasScrapping.Abstracts
{
    public abstract class AbDriver
    {

        protected abstract string BtnGerarXml { get; set; }
        protected abstract string LinkAcessoNFE { get; set; }
        protected abstract string Caminho { get; set; }
        protected abstract string UrlNFSE { get; set; }
        protected IWebDriver Driver { get; set; }
        public AbDriver()
        {

        }

        public AbDriver(RFE_CIA cia, IWebDriverFactory webDriverFactory)
        {
            Driver = webDriverFactory.GetChromeDriver(cia.CNPJ, "RioDeJaneiro");
        }

        protected void CarregarDriver()
        {
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Driver.Url = UrlNFSE;
            Driver.Manage().Window.Maximize();
        }

        public abstract void RealizarPassosDeClicks(IWebDriver driver, string cnpj);
    }
}
