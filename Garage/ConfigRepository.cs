using System;
using System.Text.Json;

namespace GarageSystem
{
    public class ConfigRepository
    {
        private readonly string _filePath;

        public ConfigRepository(string filePath)
        {
            _filePath = filePath;
        }
        public Config? LoadConfig()
        {
            if (!File.Exists(_filePath)) return null;

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<Config>(json) ?? new Config();
        }
    }
}
