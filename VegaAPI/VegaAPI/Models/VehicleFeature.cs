using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VegaAPI.Models
{
    [Table("VehicleFeatures")]
    public class VehicleFeature
    {
        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        [ForeignKey("FeatureId")]
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
