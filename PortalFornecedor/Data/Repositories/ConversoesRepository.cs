using Data.Context;
using Repository.Entities.RFE_ENTITIES;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class ConversoesRepository : Repository<RFE_PORTAL_CONVERSOES, RFEContext>, IConversoesRepository
    {
        public ConversoesRepository(RFEContext context) : base(context)
        {

        }
    }
}
