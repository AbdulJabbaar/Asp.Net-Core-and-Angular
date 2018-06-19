using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VegaAPI.Controllers.Resources;
using VegaAPI.Core;
using VegaAPI.Models;
using VegaAPI.Persistence;

namespace VegaAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));
            // Transient seprate instance for repo for every use
            // Singleton only single instance in memory during application lifecycle
            // Scope only single instance in current scope lifecycle of request
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddCors(o => o.AddPolicy("CORSPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
            services.AddAutoMapper(cfg =>
            {
                //Domain To API Resources
                cfg.CreateMap<Photo, PhotoResource>();
                cfg.CreateMap(typeof(BaseModel<>), typeof(BaseModelResource<>));
                cfg.CreateMap<VehicleFilterResource, VehicleFilter>();
                cfg.CreateMap<Make, MakeResource>();
                cfg.CreateMap<Make, KeyValuePairResource>();
                cfg.CreateMap<Model, KeyValuePairResource>();
                cfg.CreateMap<Feature, KeyValuePairResource>();
                cfg.CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(v => v.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(s => s.FeatureId)));
                cfg.CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr=>vr.Make, opt=>opt.MapFrom(v=>v.Model.Make))
                .ForMember(v => v.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vr => vr.Features, opt => opt.Ignore())
                .AfterMap((v, vr) =>
                {
                    foreach (var f in v.Features)
                    {
                        vr.Features.Add(new KeyValuePairResource { Id = f.Feature.Id, Name = f.Feature.Name });
                    }
                });


                // API Resource to Domain
                cfg.CreateMap<SaveVehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((vr, v) =>
                {
                    //Remove Unselcted Features
                    var removedFeature = new List<VehicleFeature>();
                    foreach (var f in v.Features)
                        if (!vr.Features.Contains(f.FeatureId))
                            removedFeature.Add(f);
                    foreach (var f in removedFeature)
                        v.Features.Remove(f);

                    //Add new Feature
                    foreach (var id in vr.Features)
                        if (!v.Features.Any(f => f.FeatureId == id))
                            v.Features.Add(new VehicleFeature { FeatureId = id });
                });
            });
            services.AddDbContext<VegaDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();

            // 1. Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = "https://vegack.auth0.com/";
                options.Audience = "http://localhost:4200";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CORSPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            // 2. Enable authentication middleware
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
