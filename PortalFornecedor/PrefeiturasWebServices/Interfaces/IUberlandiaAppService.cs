using Repository.Entities.RFE_ENTITIES;
using Services.ViewModels;
using System.Collections.Generic;

namespace PrefeiturasWebServices.Interfaces
{
    public interface IUberlandiaAppService
    {
        public void Get();
        public List<XmlNfseViewModel> GetByCias(List<RFE_CIA> cias);
    }
}
