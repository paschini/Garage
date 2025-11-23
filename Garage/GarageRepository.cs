using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Garage
{
    public class GarageRepository<T> where T : IVehicle
    {
        private readonly string _filePath;

        public GarageRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(IEnumerable<T> vehicles)
        {
            var json = JsonSerializer.Serialize(vehicles, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });

            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<T> Load(string fileName)
        {
            if (!File.Exists(_filePath ?? fileName)) return new List<T>();

            var json = File.ReadAllText(_filePath ?? fileName);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }
}
