using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GarbageCollector.Database.Dbos;
using GarbageCollector.Domain;
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
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();
                cfg.CreateMap<LocationDbo, Location>()
                    .IncludeAllDerived()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<TrashCanDbo, TrashCan>()
                    .IncludeAllDerived()
                    .ForMember(tc => tc.WasteCategories,
                        opt => opt.MapFrom(dbo => dbo.LinksToCategories.Select(x => x.Category).ToHashSet()))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}