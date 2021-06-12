using PrefeiturasWebServices.Interfaces;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using Services.ViewModels;
using System.Collections.Generic;
using UberlandiaWebService;

namespace PrefeiturasWebServices.Services
{
    public class UberlandiaAppService : IUberlandiaAppService
    {
        private readonly ICiaRepository _repositoryCia;
        public UberlandiaAppService(ICiaRepository repositoryCia)
        {
            _repositoryCia = repositoryCia;
        }
        public void Get()
        {
            List<RFE_CIA> _cias = _repositoryCia.Get();
            List<XmlNfseViewModel> _nfses = GetByCias(_cias);
        }

        public List<XmlNfseViewModel> GetByCias(List<RFE_CIA> cias)
        {

            cias.ForEach(cia =>
            {
                LoteRpsClient _webService = new LoteRpsClient();
                _webService.consultarNotaTomada("");
            });
           

            return new List<XmlNfseViewModel>();
        }
    }
}
