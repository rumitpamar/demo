using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Acme.BookStore.DataTransferObjects
{
    public class DatabaseSecrets
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("engine")]
        public string Engine { get; set; }
        [JsonPropertyName("host")]
        public string Host { get; set; }
        [JsonPropertyName("port")]
        public int Port { get; set; }
        [JsonPropertyName("dbInstanceIdentifier")]
        public string DbInstanceIdentifier { get; set; }

        public string GetConnectionString()
        {
            return $"Server={Host};Port={Port};Database={DbInstanceIdentifier};User Id={Username};Password={Password};";
        }
    }
}
