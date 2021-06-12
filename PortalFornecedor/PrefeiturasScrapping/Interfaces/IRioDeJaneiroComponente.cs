using OpenQA.Selenium;

namespace PrefeiturasScrapping.Interfaces
{
    public interface IRioDeJaneiroComponente
    {
        void RealizarPassosDeClicks(IWebDriver driver, string cnpj);
    }
}
