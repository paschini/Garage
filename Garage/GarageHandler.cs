using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Garage
{
    public class GarageHandler<T> : IHandler<T> where T : IVehicle
        //public class GarageHandler<T> where T : Vehicle
    {
        public IGarage<T>? Garage { get; private set; } = null;

        public Type GarageType { get; } = typeof(T);

        public bool GarageInitialised => Garage is not null;

        public void CreateGarage(int capacity, string name)
        {
            Garage = new Garage<T>(capacity, name);
        }

        public int GetGarageCapacity()
        {
            return Garage != null ? Garage.Capacity : 0;
        }

        public int GetGaragePlacesLeft()
        {
            return Garage != null ? Garage.AvailablePlaces : 0;
        }

        public int GetCurrentVehicleCount()
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.Count;
        }

        public IEnumerable<T> GetAllVehicles()
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.AllVehicles;
        }

        public T GetVehicle(int index)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            return Garage.GetVehicleAtIndex(index);
        }


        public void AddVehicle(T vehicle)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            Garage.AddVehicle(vehicle);
        }

        public T RemoveVehicle(int index)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.RemoveVehicle(index);
        }

        public T? FindByRegistraation(string registration)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");
            return Garage.FindVehicleByRegistration(registration);
        }

        public IEnumerable<T> Search(string searchTerm)
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
                    T generated = GenerateRandomVehicle();
                    Garage.AddVehicle(generated);
                }

                return true;
            }
            return false;
        }

        private T GenerateRandomVehicle()
        {
            string[] Brands = { "Ford", "BMW", "Honda", "Tesla", "Volvo" };
            string[] Colors = { "Red", "Blue", "Black", "White", "Neon Green" };
            string[] CarModels = { "Focus", "Civic", "3-Series", "Model 3" };
            string[] MotoModels = { "Ninja", "Hornet", "Shadow" };

            string[] TrunkContents = { "hund", "katt", "hunder", "katter", "Påsar", "Grejer", "" };

            Random rand = new Random();

            if (typeof(T) == typeof(Vehicle) || typeof(T) == typeof(Car) || (typeof(T).IsSubclassOf(typeof(Car))))
            {
                // Safe cast to T after creating Car
                object car = new Car(
                    registration: GeneratePlate(),
                    make: Brands[rand.Next(Brands.Length)],
                    model: CarModels[rand.Next(CarModels.Length)],
                    color: Colors[rand.Next(Colors.Length)],
                    trunkContent: TrunkContents[rand.Next(TrunkContents.Length)]
                );
                return (T)car;
            }
            
            if (typeof(T) == typeof(Motorcycle) || (typeof(T).IsSubclassOf(typeof(Motorcycle))))
            {
                object moto = new Motorcycle(
                    registration: GeneratePlate(),
                    make: Brands[rand.Next(Brands.Length)],
                    model: MotoModels[rand.Next(MotoModels.Length)],
                    color: Colors[rand.Next(Colors.Length)],
                    isUtility: rand.Next(2) == 0
                );
                return (T)moto;
            }

            throw new NotSupportedException($"Random generation for type {typeof(T).Name} is not supported.");
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

            GarageRepository<T> repo = new GarageRepository<T>($"{fileName}.json");
            repo.Save(Garage.AllVehicles);
        }

        public void LoadData(string fileName)
        {
            if (Garage is null) throw new InvalidOperationException("Garage is not initialised!");

            GarageRepository<T> repo = new GarageRepository<T>($"{fileName}.json");
            IEnumerable<T> savedVehicles = repo.Load(fileName);
            if (savedVehicles.Count() > 0) Garage.LoadVehicles(savedVehicles);
        }
    }
}
