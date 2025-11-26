using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GarageSystem
{
    public class GarageHandler : IHandler
    {
        public GarageHandler(IGarage garage)
        {
            Garage = garage;
        }

        public IGarage Garage { get; private set; }

        public Type GarageType => Garage?.VehicleType ?? typeof(object);

        public string GarageName { get; private set; } = string.Empty;

        public bool GarageInitialised => Garage is not null;

        public int GetGarageCapacity()
        {
            return Garage != null ? Garage.Capacity : 0;
        }

        public float GetGaragePlacesLeft()
        {
            return Garage != null ? Garage.AvailablePlaces : 0;
        }

        public int? GetCurrentVehicleCount()
        {
            return Garage?.Count;
        }

        public IEnumerable<IVehicle> GetAllVehicles()
        {
            return Garage.AllVehicles;
        }

        public IVehicle? GetVehicle(int index)
        {
            return Garage.GetVehicleAtIndex(index);
        }


        public void AddVehicle(IVehicle vehicle)
        {
            Garage?.AddVehicle(vehicle);
        }

        public IVehicle? RemoveVehicle(int index)
        {
            return Garage?.RemoveVehicle(index);
        }

        public IVehicle? FindByRegistraation(string registration)
        {
            return Garage.FindVehicleByRegistration(registration);
        }

        public IEnumerable<IVehicle> Search(string searchTerm)
        {
            return Garage.SearchVehicles(searchTerm);
        }

        public bool Populate(int total)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            if (Garage.Count + total <= Garage.Capacity)
            {
                for (int i = 0; i < total; i++)
                {
                    IVehicle generated = GenerateRandomVehicle();
                    Garage.AddVehicle(generated);
                }

                return true;
            }
            return false;
        }

        private IVehicle GenerateRandomVehicle()
        {
            string[] Brands = ["Ford", "BMW", "Honda", "Tesla", "Volvo"];
            string[] Colors = ["Red", "Blue", "Black", "White", "Neon Green"];
            string[] CarModels = ["Focus", "Civic", "3-Series", "Model 3"];
            string[] MotoModels = ["Ninja", "Hornet", "Shadow"];

            string[] TrunkContents = { "hund", "katt", "hunder", "katter", "Påsar", "Grejer", "" };

            Random rand = new Random();

            if (GarageType == typeof(Car) || GarageType.IsSubclassOf(typeof(Car)))
            {
                // Safe cast to T after creating Car
                IVehicle car = new Car(
                    registration: GeneratePlate(),
                    make: Brands[rand.Next(Brands.Length)],
                    model: CarModels[rand.Next(CarModels.Length)],
                    color: Colors[rand.Next(Colors.Length)],
                    trunkContent: TrunkContents[rand.Next(TrunkContents.Length)]
                );
                return car;
            }
            
            if (GarageType == typeof(Motorcycle) || GarageType.IsSubclassOf(typeof(Motorcycle)))
            {
                IVehicle moto = new Motorcycle(
                    registration: GeneratePlate(),
                    make: Brands[rand.Next(Brands.Length)],
                    model: MotoModels[rand.Next(MotoModels.Length)],
                    color: Colors[rand.Next(Colors.Length)],
                    isUtility: rand.Next(2) == 0
                );
                return moto;
            }

            throw new NotSupportedException($"Random generation for type {typeof(IVehicle).Name} is not supported.");
        }

        private string GeneratePlate()
        {
            Random r = new();

            string letters = new([.. Enumerable.Range(0, 3).Select(_ => (char)r.Next('A', 'Z' + 1))]);

            string digits = new([.. Enumerable.Range(0, 3).Select(_ => (char)r.Next('0', '9' + 1))]);

            return letters + digits;
        }

        public void SaveData(string fileName)
        {
            GarageRepository repo = new GarageRepository($"{fileName}.json");
            repo.Save(Garage.AllVehicles);
        }

        public void LoadData(string fileName)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            GarageRepository repo = new GarageRepository($"{fileName}.json");
            IEnumerable<IVehicle> savedVehicles = repo.Load(fileName);
            if (savedVehicles.Count() > 0) Garage.LoadVehicles(savedVehicles);
        }
    }
}
