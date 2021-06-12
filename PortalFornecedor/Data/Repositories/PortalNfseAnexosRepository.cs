using Data.Context;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PortalNfseAnexosRepository : Repository<RFE_ANEXOS_NFSE, RFEContext>, IPortalNfseAnexosRepository
    {
        public PortalNfseAnexosRepository(RFEContext context) : base(context)
        {

        }
    }
}
