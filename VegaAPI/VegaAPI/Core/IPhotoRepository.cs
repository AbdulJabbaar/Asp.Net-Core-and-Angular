using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VegaAPI.Models;

namespace VegaAPI.Core
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
    }
}
