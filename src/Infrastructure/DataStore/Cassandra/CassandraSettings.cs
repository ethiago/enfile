using System;

namespace Enfile.Infrastructure.DataStore.Cassandra
{
    public class CassandraSettings
    {
        public string ContactPoint { get; set; } = "localhost";

        public string Port { get; set; } = "9042";

        public string User { get; set; } = "";

        public string Password { get; set;} = "";

        public string KeySpace { get; set; } = "clr_file_management";

        public bool HasCredentials()
        {
            return ( User != null && !User.Trim().Equals(string.Empty) && Password != null && !Password.Trim().Equals(string.Empty));
        }
    }
}