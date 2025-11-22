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
            return Garage.Count;
        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.AllVehicles;
        }

        public Vehicle GetVehicle(int index)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            return  Garage.GetVehicleAtIndex(index);
        }


        public void AddVehicle(Vehicle vehicle)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            Garage.AddVehicle(vehicle);
        }

        public Vehicle RemoveVehicle(int index)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.RemoveVehicle(index);
        }

        public Vehicle? FindByRegistraation(string registration)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.FindVehicleByRegistration(registration);
        }

        public IEnumerable<Vehicle> Search(string searchTerm)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.SearchVehicles(searchTerm);
        }
    }
}
