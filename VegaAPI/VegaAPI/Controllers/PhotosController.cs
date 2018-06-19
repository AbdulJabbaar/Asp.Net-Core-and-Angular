using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VegaAPI.Controllers.Resources;
using VegaAPI.Core;
using VegaAPI.Models;

namespace VegaAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Photos")]
    public class PhotosController : Controller
    {        
        private readonly IHostingEnvironment host;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;
        private readonly IOptionsSnapshot<PhotoSettings> options;
        private readonly PhotoSettings photoSettings;

        public PhotosController(IHostingEnvironment host, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<PhotoSettings> options, IPhotoRepository photoRepository)
        {
            this.host = host;
            this.vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.photoRepository = photoRepository;
            photoSettings = options.Value;
        }

        public async Task<IEnumerable<PhotoResource>> GetPhotots(int vehicleId)
        {
            var photos = await photoRepository.GetPhotos(vehicleId);
            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost("{vehicleId}")]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file) // For Muliple File IFormCollection
        {
            var vehicle = await vehicleRepository.GetVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound();

            if (file == null)
                return BadRequest("Null file");

            if (file.Length == 0)
                return BadRequest("Empty File");

            if (file.Length > photoSettings.MaxBytes)
                return BadRequest("Maximum file size exceeded");

            if (!photoSettings.IsSupported(file.FileName))
                return BadRequest("Invalid file type");


            var uploadFolderPath = Path.Combine(host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await unitOfWork.SaveChangesAsync();

            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}