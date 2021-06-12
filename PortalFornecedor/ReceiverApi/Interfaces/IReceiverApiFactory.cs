using ReceiverApi.Responses;
using Repository.Entities.RFE_ENTITIES;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverApi.Interfaces
{
    public interface IReceiverApiFactory
    {
        ResponseToken GetToken();
        ResponseNotas NfsesSent(List<RFE_NFSE> notas);
    }
}
