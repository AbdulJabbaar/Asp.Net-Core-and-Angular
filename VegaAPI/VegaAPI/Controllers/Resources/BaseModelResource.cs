using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VegaAPI.Controllers.Resources
{
    public class BaseModelResource<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
