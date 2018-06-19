using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VegaAPI.Controllers.Resources
{
    public class MakeResource:KeyValuePairResource
    {
        public ICollection<KeyValuePairResource> Models { get; set; }
        public MakeResource()
        {
            Models = new HashSet<KeyValuePairResource>();
        }
    }
}
