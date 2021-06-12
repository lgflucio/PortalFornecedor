using OpenQA.Selenium.Firefox;

namespace WebDrivers.Interfaces
{
    public interface IFirefoxSetup
    {
        public FirefoxOptions SetFfOptions(string profileName, string diretorioDownload);
    }
}
