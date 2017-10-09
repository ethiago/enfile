

using System;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Enfile.Core.Model;
using Enfile.Infrastructure.DataStore.Cassandra;
using Enfile.Infrastructure.DataStore.Cassandra.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enfile.Application
{
    
    public class StartupDB
    {
        public static void Create()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            MappingConfiguration.Global.Define<CustomMappingConfiguration>();

            var settings = new CassandraSettings()
            {
                ContactPoint = configuration["Cassandra:ContactPoint"],
                Port = configuration["Cassandra:Port"],
                User = configuration["Cassandra:User"],
                Password = configuration["Cassandra:Password"],
                KeySpace = configuration["Cassandra:KeySpace"]
            };

            var builder = Cluster.Builder()
                .AddContactPoint(settings.ContactPoint)
                .WithPort(int.Parse(settings.Port));
             
             if(settings.HasCredentials())
             {
                 builder = builder.WithCredentials(settings.User, settings.Password);
             }                 

             var session = builder.Build().Connect();

            session.CreateKeyspaceIfNotExists(settings.KeySpace);

            session.ChangeKeyspace(settings.KeySpace);

            var fileT = new Table<File>(session);

            fileT.CreateIfNotExists();

            var contentT = new Table<ContentBlock>(session);

            contentT.CreateIfNotExists();
        }
    }
}
