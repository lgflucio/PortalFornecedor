using Repository.DTOs;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IAuthenticateAppService
    {
        AuthReturnDTO Authenticate(AuthenticateViewModel model);
    }
}
