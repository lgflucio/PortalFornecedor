using Data.Context;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PortalNfseRepository : Repository<RFE_PORTAL_HEADER_NOTA, RFEContext>, IPortalNfseRepository
    {
        public PortalNfseRepository(RFEContext context) : base(context)
        {

        }
    }
}
