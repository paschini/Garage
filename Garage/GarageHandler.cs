using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{   
    public class GarageHandler : IHandler
    {
        public Garage<Vehicle>? Garage { get; set;  } = null;

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
    }
}
