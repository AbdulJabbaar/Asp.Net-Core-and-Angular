using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VegaAPI.Core;
using VegaAPI.Models;

namespace VegaAPI.Persistence
{
    public class VehicleRepository: IVehicleRepository
    {
        private readonly VegaDbContext context;

        public VehicleRepository(VegaDbContext context)
        {
            this.context = context;
        }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true) {
            if (!includeRelated)
                return await context.Vehicles.FindAsync(id);
            return await context.Vehicles
                    .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                    .Include(i => i.Features)
                    .ThenInclude(vf => vf.Feature)
                    .SingleOrDefaultAsync(s => s.Id == id);
        }
        public void Add(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
        }
        public void Remove(Vehicle vehicle)
        {
            context.Remove(vehicle);
        }

        public async Task<BaseModel<Vehicle>> GetVehicles(VehicleFilter filter)
        {
            var result = new BaseModel<Vehicle>();
            var query = context.Vehicles
                    .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                    .Include(i => i.Features)
                    .ThenInclude(vf => vf.Feature).AsQueryable();

            if (filter.MakeId.HasValue)
            {
                query = query.Where(a => a.Model.MakeId == filter.MakeId);
            }

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"]=v=>v.Model.Make.Name,
                ["model"]=v=>v.Model.Name,
                ["contractName"]=v=>v.ContactName,
                ["id"]=v=>v.Id
            };

            if (!string.IsNullOrEmpty(filter.SortBy)) {
                if (filter.IsSortAscending)
                    query = query.OrderBy(columnsMap[filter.SortBy]);
                else
                    query = query.OrderByDescending(columnsMap[filter.SortBy]);
            }
            //if (filter.SortBy == "make")
            //    query = (filter.IsSortAscending) ? query.OrderBy(o => o.Model.Make.Name) : query.OrderByDescending(o => o.Model.Make.Name);
            //if (filter.SortBy == "model")
            //    query = (filter.IsSortAscending) ? query.OrderBy(o => o.Model.Name) : query.OrderByDescending(o => o.Model.Name);
            //if (filter.SortBy == "contactName")
            //    query = (filter.IsSortAscending) ? query.OrderBy(o => o.ContactName) : query.OrderByDescending(o => o.ContactName);

            result.TotalItems = await query.CountAsync();

            if (filter.Page <= 0)
                filter.Page = 1;
            if (filter.PageSize <= 0)
                filter.PageSize = 10;

            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);

            result.Items = await query.ToListAsync();
            return result;
        }
    }    
}
