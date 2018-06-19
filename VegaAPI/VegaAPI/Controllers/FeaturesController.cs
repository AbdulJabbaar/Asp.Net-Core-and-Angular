using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VegaAPI.Controllers.Resources;
using VegaAPI.Models;
using VegaAPI.Persistence;

namespace VegaAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Features")]
    public class FeaturesController : Controller
    {
        private readonly VegaDbContext context;
        private readonly IMapper mapper;

        public FeaturesController(VegaDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<KeyValuePairResource>> GetFeatures()
        {
            var features = await context.Features.ToListAsync();
            return Mapper.Map<List<Feature>, List<KeyValuePairResource>>(features);
        }
    }
}