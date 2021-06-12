using Services.ViewModels;
using System.Collections.Generic;

namespace PrefeiturasScrapping.Interfaces
{
    public interface IManausAppService
    {
        public void Get(int periodoConsulta);
        public List<XmlNfseViewModel> GetNfsesByUserAndPassword(string user, string password, int periodoConsulta);
    }
}
