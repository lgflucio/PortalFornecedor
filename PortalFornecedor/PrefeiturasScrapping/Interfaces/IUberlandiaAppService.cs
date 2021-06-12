using Services.ViewModels;
using System.Collections.Generic;

namespace PrefeiturasScrapping.Interfaces
{
    public interface IUberlandiaAppService
    {
        public void Get(int periodoConsulta);
        public List<XmlNfseViewModel> GetNfsesByCertificate(int periodoConsulta);
    }
}
