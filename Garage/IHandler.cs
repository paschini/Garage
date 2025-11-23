using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    //public interface IHandler
    //{
    //    bool GarageNotInitialised { get; }
    //    Type GarageType { get; }
    //    void CreateGarage(int capacity, string name);
    //    int GetGarageCapacity();
    //    int GetGaragePlacesLeft();
    //    int GetCurrentVehicleCount();
    //    //bool Populate(int total);
    //    void SaveData(string fileName);
    //    void LoadData(string fileName);
    //}

    internal interface IHandler<T> where T : IVehicle
    {
        IGarage<T>? Garage { get; }
        bool GarageInitialised { get; }
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

        //bool Populate(int total);
        void SaveData(string fileName);
        void LoadData(string filename);
    }
}
