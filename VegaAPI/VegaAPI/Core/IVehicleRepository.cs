using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VegaAPI.Models;

namespace VegaAPI.Core
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
        Task<BaseModel<Vehicle>> GetVehicles(VehicleFilter filter);
        void Add(Vehicle vehicle);
        void Remove(Vehicle vehicle);
    }
}
