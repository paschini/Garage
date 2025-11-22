using Microsoft.VisualStudio.TestTools.UnitTesting;
using Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Tests
{
    [TestClass()]
    public class HandlerTests
    {
        [TestCategory("Constructor")]
        [TestMethod()]
        public void GetGarageCapacity_GarageNotInitialised_ReturnsZero()
        {
            GarageHandler handler = new GarageHandler();
            int capacity = handler.GetGarageCapacity();
            Assert.AreEqual(0, capacity);
        }
        [TestMethod()]
        public void CreateGarage_GarageNotInitialised_GarageInitialised()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 5;

            handler.CreateGarage(targetCapacity);

            Assert.IsFalse(handler.GarageNotInitialised);
            Assert.AreEqual(targetCapacity, handler.GetGarageCapacity());
        }

        [TestMethod()]
        public void CreateGarage_GarageAlreadyInitialised_GarageReinitialized()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 10;

            handler.CreateGarage(targetCapacity - 5); // must be different than targetCapacity
            handler.CreateGarage(targetCapacity);

            Assert.IsFalse(handler.GarageNotInitialised);
            Assert.AreEqual(targetCapacity, handler.GetGarageCapacity());
        }

        [TestCategory("Operations")]
        [TestMethod()]
        public void GetGarageCapacity_GarageInitialised_ReturnsCorrectCapacity()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 8;
            
            handler.CreateGarage(targetCapacity);

            int capacity = handler.GetGarageCapacity();
            Assert.AreEqual(targetCapacity, capacity);
        }

        [TestMethod()]
        public void GetCurrentVehicleCount_GarageInitialised_ReturnsZero()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 4;

            handler.CreateGarage(targetCapacity);

            int vehicleCount = handler.GetCurrentVehicleCount();
            Assert.AreEqual(0, vehicleCount);
        }

        [TestMethod()]
        public void GetAllVehicles_EmptyGarage_ReturnsEmptyCollection()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 6;

            handler.CreateGarage(targetCapacity);
            IEnumerable<Vehicle> vehicles = handler.GetAllVehicles();
            
            Assert.IsNotNull(vehicles);
            Assert.AreEqual(0, vehicles.Count());
        }

        [TestMethod]
        public void GetAllVehicles_GarageWithVehicles_ReturnsAllVehicles()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 5;
            
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Model S", "Black", "");
            Vehicle vehicle2 = new Motorcycle("DEF456", "Harley-Davidson", "Street 750", "Blue", false);

            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            
            IEnumerable<Vehicle> vehicles = handler.GetAllVehicles();
            
            Assert.IsNotNull(vehicles);
            Assert.AreEqual(2, vehicles.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1, vehicle2 }, vehicles.ToList());
        }

        [TestMethod()]
        public void GetVehicle_GarageInitialised_ReturnsCorrectVehicle()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 4;
            
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Model 3", "White", "");
            Vehicle vehicle2 = new Motorcycle("DEF456", "Yamaha", "MT-07", "Gray", false);

            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);

            Vehicle retrievedVehicle1 = handler.GetVehicle(0);
            Vehicle retrievedVehicle2 = handler.GetVehicle(1);
            
            Assert.AreEqual(vehicle1, retrievedVehicle1);
            Assert.AreEqual(vehicle2, retrievedVehicle2);
        }

        [TestMethod()]
        public void AddVehicle_GarageInitialised_VehicleAdded()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 3;
            Vehicle vehicle = new Car("XYZ789", "Ford", "Mustang", "Red", "");

            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle);

            int vehicleCount = handler.GetCurrentVehicleCount();
            Assert.AreEqual(1, vehicleCount);
            Assert.AreEqual(vehicle, handler.GetVehicle(0));
        }

        [TestMethod()]
        public void RemoveVehicle_GarageInitialised_VehicleRemoved()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 3;
            
            Vehicle vehicle = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Green", true);
            
            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle);
            Vehicle removedVehicle = handler.RemoveVehicle(0);
            int vehicleCount = handler.GetCurrentVehicleCount();

            Assert.AreEqual(vehicle, removedVehicle);
            Assert.AreEqual(0, vehicleCount);
        }

        [TestMethod()]
        public void FindByRegistration_VehicleExists_ReturnsVehicle()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 4;
            string registration = "JKL345";

            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Car(registration, "BMW", "X5", "Silver", "");
            Vehicle vehicle3 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Green", true);

            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            handler.AddVehicle(vehicle3);
            
            Vehicle? foundVehicle = handler.FindByRegistraation(registration);
            Assert.IsNotNull(foundVehicle);
            Assert.AreEqual(vehicle2, foundVehicle);
        }

        [TestMethod()]
        public void FindByRegistration_VehicleDoesNotExist_ReturnsNull()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 4;
            string registration = "MNO678";
            
            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Green", true);
            
            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            
            Vehicle? foundVehicle = handler.FindByRegistraation(registration);
            Assert.IsNull(foundVehicle);
        }

        [TestMethod()]
        public void Search_VehiclesMatchSearchTerm_ReturnsMatchingVehicles()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 5;
            string searchTerm = "color=Red";
            
            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Green", true);
            Vehicle vehicle3 = new Car("PQR234", "Chevrolet", "Camaro", "Red", "");
            
            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            handler.AddVehicle(vehicle3);
            IEnumerable<Vehicle> results = handler.Search(searchTerm);
            
            Assert.AreEqual(2, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1, vehicle3 }, results.ToList());
        }

        [TestMethod()]
        public void Search_NoVehiclesMatchSearchTerm_ReturnsEmptyCollection()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 3;
            string searchTerm = "model=Ninja;color=red";
            
            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Green", true);
            
            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            IEnumerable<Vehicle> results = handler.Search(searchTerm);
            
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod()]
        public void Search_EmptySearchTerm_ReturnsAllVehicles()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 3;
            
            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Green", true);
            
            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            IEnumerable<Vehicle> results = handler.Search(string.Empty);
            
            Assert.AreEqual(2, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1, vehicle2 }, results.ToList());
        }

        [TestMethod()]
        public void Search_MathcingType_ReturnsRedCars()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 4;
            
            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Red", true);
            Vehicle vehicle3 = new Car("PQR234", "Chevrolet", "Camaro", "Blue", "");
            
            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            handler.AddVehicle(vehicle3);
            IEnumerable<Vehicle> results = handler.Search("type=car;color=red");
            
            Assert.AreEqual(1, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1 }, results.ToList());
        }

        [TestMethod()]
        public void Search_MathcingManyProps_ReturnsRedFerrariCarsWithRegistration()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 6;

            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Car("XYZ789", "Tesla", "X", "Red", "");
            Vehicle vehicle3 = new Car("XYZ789", "Ferrari", "458 Italia", "Red", "");
            Vehicle vehicle4 = new Car("GHI012", "Ferrari", "F8 Tributo", "Red", "");
            Vehicle vehicle5 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Red", true);
            Vehicle vehicle6 = new Car("PQR234", "Chevrolet", "Camaro", "Blue", "");

            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            handler.AddVehicle(vehicle2);
            handler.AddVehicle(vehicle3);
            handler.AddVehicle(vehicle4);
            handler.AddVehicle(vehicle5);
            handler.AddVehicle(vehicle6);
            IEnumerable<Vehicle> results = handler.Search("type=car;color=red;make=Ferrari;registration=GHI012");

            Assert.AreEqual(1, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle4 }, results.ToList());
        }

        [TestCategory("Exception Tests")]
        [TestMethod()]
        public void GetCurrentVehicleCount_GarageNotInitialised_ThrowsInvalidOperationException()
        {
            GarageHandler handler = new GarageHandler();
            Assert.ThrowsException<InvalidOperationException>(() => handler.GetCurrentVehicleCount());
        }

        [TestMethod()]
        public void GetAllVehicles_GarageNotInitialised_ThrowsInvalidOperationException()
        {
            GarageHandler handler = new GarageHandler();
            Assert.ThrowsException<InvalidOperationException>(() => handler.GetAllVehicles());
        }

        [TestMethod()]
        public void GetVehicle_GarageNotInitialised_ThrowsInvalidOperationException()
        {
            GarageHandler handler = new GarageHandler();
            Assert.ThrowsException<InvalidOperationException>(() => handler.GetVehicle(0));
        }

        [TestMethod()]
        public void GetVehicle_IndexOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            GarageHandler handler = new GarageHandler();
            handler.CreateGarage(2);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => handler.GetVehicle(5));
        }

        [TestMethod()]
        public void AddVehicle_GarageNotInitialised_ThrowsInvalidOperationException()
        {
            GarageHandler handler = new GarageHandler();
            Vehicle vehicle = new Car("ABC123", "Tesla", "Y", "White", "");
            Assert.ThrowsException<InvalidOperationException>(() => handler.AddVehicle(vehicle));
        }

        [TestMethod()]
        public void AddVehicle_GarageFull_ThrowsInvalidOperationException()
        {
            GarageHandler handler = new GarageHandler();
            int targetCapacity = 1;
            Vehicle vehicle1 = new Car("LMN456", "Chevrolet", "Camaro", "Yellow", "");
            Vehicle vehicle2 = new Motorcycle("OPQ789", "Ducati", "Monster", "Red", false);
            handler.CreateGarage(targetCapacity);
            handler.AddVehicle(vehicle1);
            Assert.ThrowsException<InvalidOperationException>(() => handler.AddVehicle(vehicle2));
        }

        [TestMethod()]
        public void RemoveVehicle_GarageNotInitialised_ThrowsInvalidOperationException()
        {
            GarageHandler handler = new GarageHandler();
            Assert.ThrowsException<InvalidOperationException>(() => handler.RemoveVehicle(0));
        }                

        [TestMethod()]
        public void RemoveVehicle_IndexOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            GarageHandler handler = new GarageHandler();
            handler.CreateGarage(2);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => handler.RemoveVehicle(3));
        }
    }
}