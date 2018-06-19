using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VegaAPI.Core;

namespace VegaAPI.Persistence
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly VegaDbContext context;

        public UnitOfWork(VegaDbContext context)
        {
            this.context = context;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
