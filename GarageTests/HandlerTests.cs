using Microsoft.VisualStudio.TestTools.UnitTesting;
using GarageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageSystem.Tests
{
    [TestClass()]
    public class HandlerTests
    {
        [TestCategory("Constructor")]
        
        [TestCategory("Operations")]
        [TestMethod()]
        public void GetGarageCapacity_GarageInitialised_ReturnsCorrectCapacity()
        {
            
            int targetCapacity = 8;
            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));

            int capacity = handler.GetGarageCapacity();
            Assert.AreEqual(targetCapacity, capacity);
        }

        [TestMethod()]
        public void GetCurrentVehicleCount_GarageInitialised_ReturnsZero()
        {
            int targetCapacity = 4;
           GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));

            int? vehicleCount = handler.GetCurrentVehicleCount();
            Assert.AreEqual(0, vehicleCount);
        }

        [TestMethod()]
        public void GetAllVehicles_EmptyGarage_ReturnsEmptyCollection()
        {
            int targetCapacity = 6;
            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            IEnumerable<IVehicle> vehicles = handler.GetAllVehicles();
            
            Assert.IsNotNull(vehicles);
            Assert.AreEqual(0, vehicles.Count());
        }

        [TestMethod]
        public void GetAllVehicles_GarageWithVehicles_ReturnsAllVehicles()
        {
            int targetCapacity = 5;

            Car vehicle1 = new Car("ABC123", "Tesla", "Model S", "Black", "");
            Car vehicle2 = new("DEF456", "Lamborghini", "Murcielago", "Blue", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));

            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            
            IEnumerable<IVehicle> vehicles = handler.GetAllVehicles();
            
            Assert.IsNotNull(vehicles);
            Assert.AreEqual(2, vehicles.Count());
            CollectionAssert.AreEquivalent(new List<IVehicle> { vehicle1, vehicle2 }, vehicles.ToList());
        }

        [TestMethod()]
        public void GetVehicle_GarageInitialised_ReturnsCorrectVehicle()
        {
            int targetCapacity = 4;
            
            Car vehicle1 = new("ABC123", "Tesla", "Model 3", "White", "");
            Car vehicle2 = new("DEF456", "Lamborghini", "Murcielago", "Gray", "");

            GarageHandler handler = new (new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);

            IVehicle? retrievedVehicle1 = handler.GetVehicle(0);
            IVehicle? retrievedVehicle2 = handler.GetVehicle(1);
            
            Assert.AreEqual(vehicle1, retrievedVehicle1);
            Assert.AreEqual(vehicle2, retrievedVehicle2);
        }

        [TestMethod()]
        public void AddVehicle_GarageInitialised_VehicleAdded()
        {
            int targetCapacity = 3;
            Car vehicle = new("XYZ789", "Ford", "Mustang", "Red", "");

            GarageHandler handler = new(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle);

            int? vehicleCount = handler.GetCurrentVehicleCount();
            Assert.AreEqual(1, vehicleCount);
            Assert.AreEqual(vehicle, handler.GetVehicle(0));
        }

        [TestMethod()]
        public void RemoveVehicle_GarageInitialised_VehicleRemoved()
        {
            int targetCapacity = 3;
            
            Motorcycle vehicle = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Green", true);

            GarageHandler handler = new GarageHandler(new Garage<Motorcycle>(targetCapacity, ""));
            handler.AddVehicle(vehicle);
            IVehicle? removedVehicle = handler.RemoveVehicle(0);
            int? vehicleCount = handler.GetCurrentVehicleCount();

            Assert.AreEqual(vehicle, removedVehicle);
            Assert.AreEqual(0, vehicleCount);
        }

        [TestMethod()]
        public void FindByRegistration_VehicleExists_ReturnsVehicle()
        {
            int targetCapacity = 4;
            string registration = "JKL345";

            Car vehicle1 = new("XYZ789", "Ford", "Mustang", "Red", "");
            Car vehicle2 = new(registration, "BMW", "X5", "Silver", "");
            Car vehicle3 = new("DEF456", "Lamborghini", "Murcielago", "Gray", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            handler.AddVehicle(vehicle3);
            
            IVehicle? foundVehicle = handler.FindByRegistraation(registration);
            Assert.IsNotNull(foundVehicle);
            Assert.AreEqual(vehicle2, foundVehicle);
        }

        [TestMethod()]
        public void FindByRegistration_VehicleDoesNotExist_ReturnsNull()
        {
            int targetCapacity = 4;
            string registration = "MNO678";
            
            Car vehicle1 = new("XYZ789", "Ford", "Mustang", "Red", "");
            Car vehicle2 = new("DEF456", "Lamborghini", "Murcielago", "Gray", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            
            IVehicle? foundVehicle = handler.FindByRegistraation(registration);
            Assert.IsNull(foundVehicle);
        }

        [TestMethod()]
        public void Search_VehiclesMatchSearchTerm_ReturnsMatchingVehicles()
        {
            int targetCapacity = 5;
            string searchTerm = "color=Red";
            
            Car vehicle1 = new("XYZ789", "Ford", "Mustang", "Red", "");
            Car vehicle2 = new("DEF456", "Lamborghini", "Murcielago", "Gray", "");
            Car vehicle3 = new("PQR234", "Chevrolet", "Camaro", "Red", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));

            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            handler.AddVehicle(vehicle3);
            IEnumerable<IVehicle> results = handler.Search(searchTerm);

            Assert.AreEqual(2, results.Count());
            CollectionAssert.AreEquivalent(new List<Car> { vehicle1, vehicle3 }, results.ToList());
        }

        [TestMethod()]
        public void Search_NoVehiclesMatchSearchTerm_ReturnsEmptyCollection()
        {
            int targetCapacity = 3;
            string searchTerm = "model=Y;color=red";
            
            Car vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Car vehicle2 = new("DEF456", "Lamborghini", "Murcielago", "Gray", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            IEnumerable<IVehicle> results = handler.Search(searchTerm);

            Assert.AreEqual(0, results.Count());
        }

        [TestMethod()]
        public void Search_EmptySearchTerm_ReturnsAllVehicles()
        {
            int targetCapacity = 3;

            Car vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Car vehicle2 = new("DEF456", "Lamborghini", "Murcielago", "Gray", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            IEnumerable<IVehicle> results = handler.Search(string.Empty);
            
            Assert.AreEqual(2, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1, vehicle2 }, results.ToList());
        }

        [TestMethod()]
        public void Search_MathcingManyProps_ReturnsRedFerrariCarsWithRegistration()
        {
            int targetCapacity = 6;

            Car vehicle1 = new("XYZ789", "Ford", "Mustang", "Red", "");
            Car vehicle2 = new("XYZ789", "Tesla", "X", "Red", "");
            Car vehicle3 = new("XYZ789", "Ferrari", "458 Italia", "Red", "");
            Car vehicle4 = new("GHI012", "Ferrari", "F8 Tributo", "Red", "");
            Car vehicle5 = new("DEF456", "Lamborghini", "Murcielago", "Gray", "");
            Car vehicle6 = new("PQR234", "Chevrolet", "Camaro", "Blue", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            handler.AddVehicle(vehicle3);
            handler.AddVehicle(vehicle4);
            handler.AddVehicle(vehicle5);
            handler.AddVehicle(vehicle6);
            IEnumerable<IVehicle> results = handler.Search("type=car;color=red;make=Ferrari;registration=GHI012");

            Assert.AreEqual(1, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle4 }, results.ToList());
        }

        [TestMethod()]
        public void Populate_EmptyGarage_GarageHasTotalVehicles()
        {
            int targetCapacity = 10;

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            bool result = handler.Populate(targetCapacity);

            Assert.IsTrue(result);
            Assert.AreEqual(targetCapacity, handler.GetAllVehicles().Count());
        }


            [TestCategory("Exception Tests")]
        [TestMethod()]
        public void GetVehicle_IndexOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            int targetCapacity = 1;
            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => handler.GetVehicle(5));
        }

        [TestMethod()]
        public void AddVehicle_GarageFull_ThrowsInvalidOperationException()
        {
            int targetCapacity = 1;

            Car vehicle1 = new("LMN456", "Chevrolet", "Camaro", "Yellow", "");
            Car vehicle2 = new("OPQ789", "Lamborghini", "Murcielago", "Space grey", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            Assert.ThrowsException<InvalidOperationException>(() => handler.AddVehicle(vehicle2));
        }

        [TestMethod()]
        public void AddVehicle_GarageFull_ThrowsInvalidCastException()
        {
            int targetCapacity = 1;

            Car vehicle1 = new("LMN456", "Chevrolet", "Camaro", "Yellow", "");
            Motorcycle vehicle2 = new Motorcycle("OPQ789", "Ducati", "Monster", "Red", false);

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            Assert.ThrowsException<InvalidCastException>(() => handler.AddVehicle(vehicle2));
        }

        [TestMethod()]
        public void AddVehicle_GarageFull_ThrowsInvalidOperationExceptionOnSecondCar()
        {
            int targetCapacity = 2;

            Car vehicle1 = new("LMN456", "Chevrolet", "Camaro", "Yellow", "");
            Car vehicle2 = new("DEF456", "Lamborghini", "Murcielago", "Gray", "");
            Car vehicle3 = new("XYZ789", "Tesla", "Y", "White", "");

            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2); // should not throw here
            Assert.ThrowsException<InvalidOperationException>(() => handler.AddVehicle(vehicle3));
        }   

        [TestMethod()]
        public void RemoveVehicle_IndexOutOfRange_ThrowsIndexOutOfRange()
        {
            int targetCapacity = 1;
            GarageHandler handler = new GarageHandler(new Garage<Car>(targetCapacity, ""));
            Assert.ThrowsException<IndexOutOfRangeException>(() => handler.RemoveVehicle(3));
        }
    }
}