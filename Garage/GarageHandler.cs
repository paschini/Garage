using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class GarageHandler : IHandler<Vehicle>
    {
        public IGarage<Vehicle>? ActiveGarage { get; private set; } = null;

        public Dictionary<string, IGarage<Vehicle>> Garages { get; private set; } = [];

        public bool GarageNotInitialised => ActiveGarage is null;

        public void CreateGarage(int capacity, string name)
        {
            ActiveGarage = new Garage<Vehicle>(capacity, name);
            Garages.Add(name, ActiveGarage);
        }

        public int GetGarageCapacity()
        {
            return ActiveGarage != null ? ActiveGarage.Capacity : 0;
        }

        public int GetGaragePlacesLeft()
        {
            return ActiveGarage != null ? ActiveGarage.AvailablePlaces : 0;
        }

        public int GetCurrentVehicleCount()
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");
            return ActiveGarage.Count;
        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");
            return ActiveGarage.AllVehicles;
        }

        public Vehicle GetVehicle(int index)
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");

            return  ActiveGarage.GetVehicleAtIndex(index);
        }


        public void AddVehicle(Vehicle vehicle)
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");
            ActiveGarage.AddVehicle(vehicle);
        }

        public Vehicle RemoveVehicle(int index)
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");
            return ActiveGarage.RemoveVehicle(index);
        }

        public Vehicle? FindByRegistraation(string registration)
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");
            return ActiveGarage.FindVehicleByRegistration(registration);
        }

        public IEnumerable<Vehicle> Search(string searchTerm)
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");
            return ActiveGarage.SearchVehicles(searchTerm);
        }

        public bool Populate(int total)
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");

            if (ActiveGarage.Count + total <= ActiveGarage.Capacity)
            {
                for (int i = 0; i < total; i++)
                {
                    Vehicle generated = GenerateRandomVehicle();
                    ActiveGarage.AddVehicle(generated);
                }

                return true;
            }
            return false;
        }

        private Vehicle GenerateRandomVehicle()
        {

            string[] Brands = { "Ford", "BMW", "Honda", "Tesla", "Volvo" };
            string[] Colors = { "Red", "Blue", "Black", "White", "Neon Green" };
            string[] CarModels = { "Focus", "Civic", "3-Series", "Model 3" };
            string[] MotoModels = { "Ninja", "Hornet", "Shadow" };

            string[] TrunkContents = { "hund", "katt", "hunder", "katter", "Påsar", "Grejer", "" };

            Random rand = new Random();
            bool makeCar = rand.Next(2) == 0;

            if (makeCar)
            {
                return new Car(
                    registration: GeneratePlate(),
                    make: Brands[rand.Next(Brands.Length)],
                    model: CarModels[rand.Next(CarModels.Length)],
                    color: Colors[rand.Next(Colors.Length)],
                    trunkContent: TrunkContents[rand.Next(TrunkContents.Length)]
                );
            }
            else
            {
                return new Motorcycle(
                    registration: GeneratePlate(),
                    make: Brands[rand.Next(Brands.Length)],
                    model: MotoModels[rand.Next(MotoModels.Length)],
                    color: Colors[rand.Next(Colors.Length)],
                    isUtility: rand.Next(2) == 0
                );
            }
        }

        private string GeneratePlate()
        {
            Random r = new Random();

            string letters = new string(Enumerable.Range(0, 3)
                .Select(_ => (char)r.Next('A', 'Z' + 1))
                .ToArray());

            string digits = new string(Enumerable.Range(0, 3)
                .Select(_ => (char)r.Next('0', '9' + 1))
                .ToArray());

            return letters + digits;
        }

        public void SaveData(string fileName)
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");

            GarageRepository repo = new GarageRepository($"{fileName}.json");
            repo.Save(ActiveGarage.AllVehicles);
        }

        public void LoadData(string fileName)
        {
            if (ActiveGarage is null) throw new InvalidOperationException("Garage is not initialised!");

            GarageRepository repo = new GarageRepository($"{fileName}.json");
            IEnumerable<Vehicle> savedVehicles = repo.Load(fileName);
            if (savedVehicles.Count() > 0) ActiveGarage.LoadVehicles(savedVehicles);
        }
    }
}
