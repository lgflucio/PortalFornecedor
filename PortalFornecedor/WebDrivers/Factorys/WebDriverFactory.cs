using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Repository.Entities.RFE_ENTITIES;
using Shared.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using WebDrivers.Interfaces;

namespace WebDrivers.Factorys
{
    public class WebDriverFactory : IWebDriverFactory
    {
        private string _diretorioDownload;
        private string _applicationDataFolder;
        private string _profilesIniPath;

        public IWebDriver GetChromeDriver(string cnpj = "", string prefeitura = "", string diretorioDownload = "")
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            ChromeOptions options = new ChromeOptions();
            string pastaCliente = string.Empty;
            if (cnpj != "" && diretorioDownload != "")
            {

                pastaCliente = $@"{diretorioDownload}\{cnpj}";
                if (!Directory.Exists(pastaCliente))
                    Directory.CreateDirectory(pastaCliente);
            }
            else if (cnpj != "" && prefeitura != "")
            {

                pastaCliente = $@"C:\Notas Fiscais\{prefeitura}\{cnpj}";
                if (!Directory.Exists(pastaCliente))
                    Directory.CreateDirectory(pastaCliente);
            }

            options.AddUserProfilePreference("download.default_directory", pastaCliente);
            options.AddUserProfilePreference("safebrowsing.enabled", "false");
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            options.AddArgument("no-sandbox");

            var driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(3));
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));

            return driver;
        }

        public IWebDriver GetFireFoxDriver()
        {
            //Receber do banco estas informações
            string cliente = "ClienteRaspagem";
            string certificado = "Certificado digital para colocar no profile";

            string[] profilesIni = Inicializa();

            _diretorioDownload = @"D:\Profiles\";

            string folder = _diretorioDownload;
            string profileCliente = folder + cliente;
            string profileDefaultAnterior = profilesIni.ToList().Where(x => x.StartsWith("Default=" + folder)).FirstOrDefault();
            string profileDefaultAtual = @"Default=" + profileCliente;
            FirefoxOptions options;

            if (cliente.IsNullOrEmpty())
            {
                //Cliente não cadastrado
            }

            FirefoxProfile profile = new FirefoxProfileManager().GetProfile(cliente);
            if (profile == null)
                CriarPerfilFirefox(cliente, profilesIni);

            TrocarProfileDefault(_profilesIniPath, profileDefaultAnterior, profileDefaultAtual);

            if (profile == null)
                profile = new FirefoxProfileManager().GetProfile(cliente);

            options = new FirefoxOptions()
            {
                Profile = profile
            };

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.Host = "::1";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var driver = new FirefoxDriver(service, options);
            return driver;
        }

        public IWebDriver GetFireFoxDriver(RFE_CIA cia)
        {
            //Receber do banco estas informações
            string cliente = cia.CNPJ;
            string certificado = "Certificado digital para colocar no profile";

            string[] profilesIni = Inicializa();

            _diretorioDownload = @"D:\Profiles\";

            string profileCliente = $@"Profiles/{cliente}";

            string profileDefaultAnterior = profilesIni.ToList().Where(x => x.StartsWith("Default=")).FirstOrDefault();

            string profileDefaultAtual = @"Default=" + profileCliente;

            FirefoxProfile profile = new FirefoxProfileManager().GetProfile(cliente);
            if (profile == null)
                CriarPerfilFirefox(cliente, profilesIni);

            TrocarProfileDefault(_profilesIniPath, profileDefaultAnterior, profileDefaultAtual);

            string profileFolder = $@"{_applicationDataFolder}\Mozilla\Firefox\Profiles\{cliente}";

            FirefoxOptions ffOptions = new FirefoxOptions();

            FirefoxProfile firefoxProfile = new FirefoxProfile(profileFolder);
            firefoxProfile.SetPreference("browser.download.folderList", 2); //Use for the default download directory the last folder specified for a download 0 => desktop 1 => pasta download 2 => pasta customizada
            firefoxProfile.SetPreference("browser.download.dir", _diretorioDownload); //Set the last directory used for saving a file from the "What should (browser) do with this file?" dialog.
            firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf"); //list of MIME types to save to disk without asking what to use to open the file
            firefoxProfile.SetPreference("pdfjs.disabled", true);  // disable the built-in PDF viewer
            firefoxProfile.AcceptUntrustedCertificates = true;
            firefoxProfile.AssumeUntrustedCertificateIssuer = true;

            ffOptions.Profile = firefoxProfile;

            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            var driver = new FirefoxDriver(service, ffOptions);

            return driver;
        }

        public IWebDriver GetFireFoxDriver(string diretorioDownload)
        {
            //Receber do banco estas informações
            string cliente = "ClienteRaspagem";
            string certificado = "Certificado digital para colocar no profile";

            string[] profilesIni = Inicializa();

            _diretorioDownload = diretorioDownload;

            string profileCliente = $@"Profiles/{cliente}";

            string profileDefaultAnterior = profilesIni.ToList().Where(x => x.StartsWith("Default=")).FirstOrDefault();

            string profileDefaultAtual = @"Default=" + profileCliente;

            FirefoxProfile profile = new FirefoxProfileManager().GetProfile(cliente);
            if (profile == null)
                CriarPerfilFirefox(cliente, profilesIni);

            TrocarProfileDefault(_profilesIniPath, profileDefaultAnterior, profileDefaultAtual);

            string profileFolder = $@"{_applicationDataFolder}\Mozilla\Firefox\Profiles\{cliente}";

            FirefoxOptions ffOptions = new FirefoxOptions();

            FirefoxProfile firefoxProfile = new FirefoxProfile(profileFolder);
            firefoxProfile.SetPreference("browser.download.folderList", 2); //Use for the default download directory the last folder specified for a download 0 => desktop 1 => pasta download 2 => pasta customizada
            firefoxProfile.SetPreference("browser.download.dir", _diretorioDownload); //Set the last directory used for saving a file from the "What should (browser) do with this file?" dialog.
            firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf"); //list of MIME types to save to disk without asking what to use to open the file
            firefoxProfile.SetPreference("pdfjs.disabled", true);  // disable the built-in PDF viewer
            firefoxProfile.AcceptUntrustedCertificates = true;
            firefoxProfile.AssumeUntrustedCertificateIssuer = true;

            ffOptions.Profile = firefoxProfile;

            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            //service.Host = "::1";
            var driver = new FirefoxDriver(service, ffOptions);

            return driver;
        }

        private string[] Inicializa()
        {
            _applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _profilesIniPath = $@"{_applicationDataFolder}\Mozilla\Firefox\profiles.ini";
            string[] profilesIni = File.ReadAllLines(_profilesIniPath);

            return profilesIni;
        }
        private void TrocarProfileDefault(string profilesIniPath, string profileDefaultAnterior, string profileDefaultAtual)
        {
            string profileDefault = File.ReadAllText(profilesIniPath);

            if (profileDefaultAnterior != profileDefaultAtual)
            {
                profileDefault = profileDefault.Replace(profileDefaultAnterior, profileDefaultAtual);
                File.WriteAllText(profilesIniPath, profileDefault);
            }
        }
        private void CriarPerfilFirefox(string cliente, string[] profilesIni)
        {
            profilesIni = Inicializa();

            string folder = _diretorioDownload;

            string profilePadrao = profilesIni.ToList().Where(x => x.StartsWith("Default=")).Select(x => x.Replace("Default=", "")).FirstOrDefault();

            string profilePadraoPath = $@"{_applicationDataFolder}\Mozilla\Firefox\{profilePadrao}";

            string profileClientePath = $@"{_applicationDataFolder}\Mozilla\Firefox\Profiles\{cliente}";

            if (!Directory.Exists(profileClientePath))
            {
                Directory.CreateDirectory(profileClientePath);
            }

            int ultimoProfile = profilesIni.ToList().Where(x => x.StartsWith("[Profile")).Select(x => x.Replace("[Profile", "")).Select(x => x.Replace("]", "").ToInt()).Max();
            int numeroProximoProfile = ultimoProfile + 1;

            string proximoProfile = $"[Profile{numeroProximoProfile}]";
            string name = "Name=" + cliente;
            string relative = "IsRelative=1";
            string path = $"Path=Profiles/{cliente}";

            string ultimaLinha = profilesIni.LastOrDefault();

            using (StreamWriter sw = File.AppendText(_profilesIniPath))
            {
                if (ultimaLinha != "")
                    sw.WriteLine(sw.NewLine);

                sw.WriteLine(proximoProfile);
                sw.WriteLine(name);
                sw.WriteLine(relative);
                sw.WriteLine(path);
                sw.WriteLine("");
            }
        }
    }
}
