using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace GarageSystem
{

    public class ConfigRepository
    {
       public static IConfiguration Config()
        {
            string currentDir = Directory.GetCurrentDirectory();
            return new ConfigurationBuilder()
                .SetBasePath(currentDir)
                .AddJsonFile("config.json")
                .Build();
        }
    }
}