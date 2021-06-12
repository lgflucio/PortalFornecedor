using Services.ViewModels;
using System.Collections.Generic;

namespace PrefeiturasScrapping.Interfaces
{
    public interface ICampinasAppService
    {
        public void Get();
        public List<XmlNfseViewModel> GetNfsesByUserAndPassword(string user, string password);
    }
}
