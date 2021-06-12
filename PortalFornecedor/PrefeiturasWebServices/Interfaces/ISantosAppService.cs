using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrefeiturasWebServices.Interfaces
{
    public interface ISantosAppService
    {
        public void Get();
        public List<XmlNfseViewModel> GetByCnpj(string cnpj);
    }
}
