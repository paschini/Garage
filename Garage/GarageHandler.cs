using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class GarageHandler : IHandler<Vehicle>
    {
        public IGarage<Vehicle>? Garage { get; set; } = null;

        public bool GarageNotInitialised => Garage is null;

        public void CreateGarage(int capacity)
        {
            Garage = new Garage<Vehicle>(capacity);
        }

        public int GetGarageCapacity()
        {
            return Garage != null ? Garage.Capacity : 0;
        }

        public int GetCurrentVehicleCount()
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.AllVehicles.Count();
        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.AllVehicles;
        }

        public Vehicle GetVehicle(int index)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            var vehicles = Garage.AllVehicles.ToList();
            if (index < 0 || index >= vehicles.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range!");
            return vehicles[index];
        }


        public void AddVehicle(Vehicle vehicle)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            Garage.AddVehicle(vehicle);
        }

        public Vehicle RemoveVehicle(int index)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            var vehicles = Garage.AllVehicles.ToList();
            if (index < 0 || index >= vehicles.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range!");
            return Garage.RemoveVehicle(index);
        }

        public Vehicle? FindByRegistraation(string registration)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            var vehicle = Garage.AllVehicles.FirstOrDefault(v => v.Registration.Equals(registration, StringComparison.OrdinalIgnoreCase));
            return vehicle;
        }

        public IEnumerable<Vehicle> Search(string searchTerm)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            if (string.IsNullOrWhiteSpace(searchTerm)) return Garage.AllVehicles.ToList();

            var filters = searchTerm.Split(';', StringSplitOptions.RemoveEmptyEntries)
                               .Select(f => f.Split('='))
                               .Where(parts => parts.Length == 2)
                               .Select(parts => new { Property = parts[0].Trim(), Value = parts[1].Trim() })
                               .ToList();

            return GetAllVehicles().Where(vehicle =>
            {
                foreach (var filter in filters)
                {
                    var prop = typeof(Vehicle).GetProperty(filter.Property,
                        System.Reflection.BindingFlags.IgnoreCase |
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.Instance);

                    if (prop == null) return false;

                    var propValue = prop.GetValue(vehicle)?.ToString() ?? "";

                    if (!propValue.Equals(filter.Value, StringComparison.OrdinalIgnoreCase)) return false;
                }
                return true;
            });
            //return Garage.AllVehicles.Where(predicate);
        }
    }
}
