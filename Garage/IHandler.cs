using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal interface IHandler<T> where T : Vehicle
    {
        IGarage<T>? ActiveGarage { get; }
        Dictionary<string, IGarage<T>> Garages { get; }        
        bool GarageNotInitialised { get; }
        void CreateGarage(int capacity, string name);
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
