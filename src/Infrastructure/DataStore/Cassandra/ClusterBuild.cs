using Cassandra;
using Microsoft.Extensions.Options;

namespace Enfile.Infrastructure.DataStore.Cassandra
{
    public class ClusterBuild
    {
        private readonly CassandraSettings _settings;
        public ClusterBuild(IOptions<CassandraSettings> settings)
        {
            this._settings = settings.Value;
        }

        public ISession Build()
        {
            var builder = Cluster.Builder()
                .AddContactPoint(_settings.ContactPoint)
                .WithPort(int.Parse(_settings.Port));
             
             if(_settings.HasCredentials())
             {
                 builder = builder.WithCredentials(_settings.User, _settings.Password);
             }                 
             return builder.Build().Connect(_settings.KeySpace);
        }
    }
}
 