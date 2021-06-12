using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IPortalNfseAppService
    {
        List<PortalNfseViewModel> Get();
        PortalNfseDetalhesViewModel GetById(int id);
        FileContentResult DownloadFile(int id);
    }
}
