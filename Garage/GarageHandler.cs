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

        public bool Populate(int total)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            if (Garage.Count + total <= Garage.Capacity)
            {
                for (int i = 0; i < total; i++)
                {
                    Vehicle generated = GenerateRandomVehicle();
                    Garage.AddVehicle(generated);
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
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            GarageRepository repo = new GarageRepository($"{fileName}.json");
            repo.Save(Garage.AllVehicles);
        }

        public void LoadData(string fileName)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            GarageRepository repo = new GarageRepository($"{fileName}.json");
            IEnumerable<Vehicle> savedVehicles = repo.Load(fileName);
            if (savedVehicles.Count() > 0) Garage.LoadVehicles(savedVehicles);
        }
    }
}
