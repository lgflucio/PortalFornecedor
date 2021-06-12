using Data.Context;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PortalNfseItemRepository : Repository<RFE_PORTAL_ITEM_NOTA, RFEContext>, IPortalNfseItemRepository
    {
        public PortalNfseItemRepository(RFEContext context) : base(context)
        {

        }
    }
}
