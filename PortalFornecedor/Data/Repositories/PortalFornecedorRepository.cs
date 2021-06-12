using Data.Context;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PortalFornecedorRepository : Repository<RFE_PORTAL_FORNECEDOR, RFEContext>, IPortalFornecedorRepository
    {
        public PortalFornecedorRepository(RFEContext context) : base(context)
        {

        }
    }
}
