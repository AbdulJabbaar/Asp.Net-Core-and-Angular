using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VegaAPI.Core
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
