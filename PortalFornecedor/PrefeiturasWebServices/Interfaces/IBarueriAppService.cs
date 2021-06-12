using Services.ViewModels;
using System.Collections.Generic;

namespace PrefeiturasWebServices.Interfaces
{
    public interface IBarueriAppService
    {
        public void Get();
        public List<XmlNfseViewModel> GetByCnpj(string cnpj);
    }
}
