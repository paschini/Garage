using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal interface IHandler<T> where T : Vehicle
    {
        IGarage<T>? Garage { get; set;  }
        bool GarageNotInitialised { get; }
        void CreateGarage(int capacity);
        int GetGarageCapacity();
        int GetGaragePlacesLeft();
        int GetCurrentVehicleCount();
        IEnumerable<T> GetAllVehicles();
        T GetVehicle(int index);
        void AddVehicle(T vehicle);
        T RemoveVehicle(int index);
        T? FindByRegistraation(string registration);
        IEnumerable<T> Search(string searchTerm);

        bool Populate(int total);
        void SaveData(string fileName);
        void LoadData(string fileName);
    }
}
