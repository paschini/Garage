using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal interface IHandler
    {
        protected Garage<Vehicle>? Garage { get; set;  }
        bool GarageNotInitialised { get; }
        void CreateGarage(int capacity);
        int GetGarageCapacity();
        int GetCurrentVehicleCount();
        IEnumerable<Vehicle> GetAllVehicles();
        Vehicle GetVehicle(int index);
        void AddVehicle(Vehicle vehicle);
        Vehicle RemoveVehicle(int index);
    }
}
