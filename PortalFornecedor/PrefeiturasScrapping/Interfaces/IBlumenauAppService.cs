using Services.ViewModels;
using System.Collections.Generic;

namespace PrefeiturasScrapping.Interfaces
{
    public interface IBlumenauAppService
    {
        public void Get(int periodoConsulta, string diretorioDownload = "");
        List<XmlNfseViewModel> ReadXmlsInDirectory(string pathDirectory);
    }
}
