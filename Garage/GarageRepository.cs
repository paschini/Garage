using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GarageSystem
{
    public class GarageRepository
    {
        private readonly string _filePath;

        public GarageRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(IEnumerable<IVehicle> vehicles)
        {
            var json = JsonSerializer.Serialize(vehicles, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            });

            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<IVehicle> Load(string fileName)
        {
            if (!File.Exists(_filePath ?? fileName)) return new List<IVehicle>();

            var json = File.ReadAllText(_filePath ?? fileName);
            return JsonSerializer.Deserialize<List<IVehicle>>(json) ?? new List<IVehicle>();
        }
    }
}
