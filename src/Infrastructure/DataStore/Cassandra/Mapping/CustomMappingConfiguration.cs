using Cassandra.Mapping;
using Enfile.Commons.Model;
using Enfile.Core.Model;

namespace Enfile.Infrastructure.DataStore.Cassandra.Mapping
{
    public class CustomMappingConfiguration : Mappings
    {
        public CustomMappingConfiguration()
        {
            For<File>()
                .TableName("files")
                .PartitionKey(f => f.Id)
                .Column(f => f.Encoding, c => c.WithDbType<int>() )
                .Column(f => f.State, c => c.WithDbType<int>() )
                .Column(f => f.Content, c => c.Ignore() )
                ;
            For<ContentBlock>()
                .TableName("contents")
                .PartitionKey(c => c.FileID)
                .ClusteringKey( c => c.Priority)
                ;
        }
    }
}