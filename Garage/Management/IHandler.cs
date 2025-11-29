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

        /// <summary>
        /// Gets the garaage's capacity. Does not account for available places.
        /// </summary>
        int GetGarageCapacity();

        /// <summary>
        /// Gets the garage's available places. Starts at capacity * 3. 
        /// Decreases according to the amount of places different vehicles can take.
        /// Divided by 3 on return, because of motorcycles. 
        /// If formatted with ToMixedFraction, it will also show places accounting for motorcycles that take 1/3 of a place.
        /// Ex: places left 2 2/3 means the garage can still fit 2 cars and 2 motorcycles.
        /// </summary>
        float GetGaragePlacesLeft();
        int? GetCurrentVehicleCount();
        IEnumerable<IVehicle>? GetAllVehicles();
        IVehicle? GetVehicle(int index);
        void AddVehicle(IVehicle vehicle);
        IVehicle? RemoveVehicle(int index);
        IVehicle? FindByRegistraation(string registration);
        IEnumerable<IVehicle> Search(string searchTerm);

        /// <summary>
        /// Populates a garage with the given total of vehicles, or if total is too many, 
        /// fills up to the entire capacity of the garage, accounting for available spaces.
        /// </summary>
        /// <param name="total">The total vehicles to try and include</param>
        /// <returns>
        /// Returns true if the garage was popuolated if at least 1 vehicle.
        /// Returns false if the opration could not be completed.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if the handler has no garage attached (Garage == null)</exception>
        bool Populate(int total);
        void SaveData(string fileName);
        void LoadData(string filename);
    }
}
