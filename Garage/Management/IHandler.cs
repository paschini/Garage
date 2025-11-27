using Garage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Management
{
    public interface IHandler
    {
        IGarage Garage { get; }
        Type GarageType { get; }
        string GarageName { get; }
        bool GarageInitialised { get; }
        int GetGarageCapacity();
        float GetGaragePlacesLeft();
        int? GetCurrentVehicleCount();
        IEnumerable<IVehicle>? GetAllVehicles();
        IVehicle? GetVehicle(int index);
        void AddVehicle(IVehicle vehicle);
        IVehicle? RemoveVehicle(int index);
        IVehicle? FindByRegistraation(string registration);
        IEnumerable<IVehicle> Search(string searchTerm);

        bool Populate(int total);
        void SaveData(string fileName);
        void LoadData(string filename);
    }
}
