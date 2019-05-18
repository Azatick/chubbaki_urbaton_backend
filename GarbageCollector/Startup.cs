﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
using GarbageCollector.Services;
using GarbageCollector.Services.Impl;
using GarbageCollector.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace GarbageCollector
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
            services.AddEntityFrameworkNpgsql().AddDbContext<GarbageCollectorContext>(options =>
                options
                    .UseNpgsql(Configuration.GetConnectionString("KonStr"), x => x.UseNetTopologySuite())
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)));

            #region AutomapperConfig

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GarbageAppUserDbo, GarbageAppUser>()
                    .IncludeAllDerived()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter()
                    .ReverseMap();
                cfg.CreateMap<LocationDbo, Location>()
                    .IncludeAllDerived()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter()
                    .ReverseMap();

                cfg.CreateMap<TrashCanDbo, TrashCan>()
                    .IncludeAllDerived()
                    .ForMember(tc => tc.WasteCategories,
                        opt => opt.MapFrom(dbo => dbo.LinksToCategories.Select(x => x.Category).ToHashSet()))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter()
                    .ReverseMap();
                cfg.CreateMap<WasteCategoryDbo, WasteCategory>()
                    .IncludeAllDerived()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter()
                    .ReverseMap();
                
                cfg.CreateMap<WasteTakePointDbo, WasteTakePoint>()
                    .IncludeAllDerived()
                    .ForMember(tc => tc.AcceptingCategories,
                        opt => opt.MapFrom(dbo => dbo.LinksToCategories.Select(x => x.Category).ToHashSet()))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter()
                    .ReverseMap();

                cfg.CreateMap<LocationDbo, LocationViewModel>()
                    .ForMember(x => x.Latitude, x => x.MapFrom(s => s.Coordinates.X))
                    .ForMember(x => x.Longitude, x => x.MapFrom(s => s.Coordinates.Y));

                cfg.CreateMap<WasteTakePointDbo, WasteTakePointViewModel>()
                    .ForMember(x => x.Location, x => x.MapFrom(s => s.Location))
                    .ForMember(x => x.Name, x => x.MapFrom(s => s.Name));
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<DomainOptions>(Configuration.GetSection(nameof(DomainOptions)));
            services.AddTransient<IDataUploader, DataUploader>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GarbageCollector API", Version = "v1" });
                
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               // app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}