using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Garage
{
    public class GarageRepository
    {
        private readonly string _filePath;

        public GarageRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(IEnumerable<Vehicle> vehicles)
        {
            var json = JsonSerializer.Serialize(vehicles, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });

            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<Vehicle> Load(string fileName)
        {
            if (!File.Exists(_filePath ?? fileName)) return new List<Vehicle>();

            var json = File.ReadAllText(_filePath ?? fileName);
            return JsonSerializer.Deserialize<List<Vehicle>>(json) ?? new List<Vehicle>();
        }
    }
}
