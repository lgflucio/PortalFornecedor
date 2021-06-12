using OpenQA.Selenium;
using Repository.Entities.RFE_ENTITIES;

namespace WebDrivers.Interfaces
{
    public interface IWebDriverFactory
    {
        IWebDriver GetChromeDriver(string cnpj = "", string prefeitura = "", string diretorioDownload = "");
        IWebDriver GetFireFoxDriver();
        IWebDriver GetFireFoxDriver(RFE_CIA cia);
        IWebDriver GetFireFoxDriver(string diretorioDownload);
    }
}
