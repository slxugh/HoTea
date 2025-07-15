using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9
{

    public class DatabaseConfig
    {
        public string Server { get; set; }
        public string Database { get; set; }
    }

    public class AppConfig
    {
        public DatabaseConfig Database { get; set; }
    }
    public class ConfigService
    {
        private readonly string _filePath;

        public ConfigService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<AppConfig> LoadConfigAsync()
        {
            using (StreamReader reader = new StreamReader(_filePath))
            {
                string json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<AppConfig>(json);
            }
        }
    }
}
