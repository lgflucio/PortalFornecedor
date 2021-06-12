using OpenQA.Selenium.Firefox;
using System;
using System.IO;
using System.Text;
using WebDrivers.Interfaces;

namespace WebDrivers.Factorys
{
    public class FirefoxSetup : IFirefoxSetup
    {
        public FirefoxOptions SetFfOptions(string profileName, string diretorioDownload)
        {
            string applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string profileFolder = $@"{applicationDataFolder}\Mozilla\Firefox\Profiles\{profileName}";

            if (!Directory.Exists(profileFolder))
            {
                throw new ArgumentException(@$"Não existe o perfil {profileName} solicitado do Firefox");
            }

            var ffOptions = new FirefoxOptions();

            FirefoxProfile firefoxProfile = new FirefoxProfile(profileFolder);
            firefoxProfile.SetPreference("browser.download.folderList", 2); //Use for the default download directory the last folder specified for a download 0 => desktop 1 => pasta download 2 => pasta customizada
            firefoxProfile.SetPreference("browser.download.dir", diretorioDownload); //Set the last directory used for saving a file from the "What should (browser) do with this file?" dialog.
            firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf"); //list of MIME types to save to disk without asking what to use to open the file
            firefoxProfile.SetPreference("pdfjs.disabled", true);  // disable the built-in PDF viewer
            firefoxProfile.AcceptUntrustedCertificates = true;
            firefoxProfile.AssumeUntrustedCertificateIssuer = true;

            ffOptions.Profile = firefoxProfile;

            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            return ffOptions;
        }
    }
}
