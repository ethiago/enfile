using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Enfile.Infrastructure.DataStore.Cassandra;
using Enfile.Infrastructure.DataStore.Cassandra.Mapping;
using Cassandra;
using Enfile.Infrastructure.Repository;
using Cassandra.Mapping;
using Cassandra.Data.Linq;
using Enfile.Core.Model;
using Enfile.Core.Service;
using Enfile.Core.Interfaces;
using Enfile.Presentation.Filter;

namespace Enfile.Application
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            MappingConfiguration.Global.Define<CustomMappingConfiguration>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCassandra(services);

            ConfigureCoreServices(services);

            services.AddMvc( config => config.Filters.Add(new ValidateModelAttribute()));
        }

        public void ConfigureCoreServices(IServiceCollection services)
        {
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileService, FileService>();
        }

        private void ConfigureCassandra(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<CassandraSettings>(Configuration.GetSection("Cassandra"));

            services.AddSingleton<ClusterBuild>();

            services.Add(new ServiceDescriptor(typeof(ISession), (p) => {
                return p.GetRequiredService<ClusterBuild>().Build();
            }, ServiceLifetime.Singleton));

            services.AddScoped<Table<File>>();
            services.AddScoped<Table<ContentBlock>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
