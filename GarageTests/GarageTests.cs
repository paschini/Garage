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
    public class GarageTests
    {
        [TestCategory("Constructor")]
        [TestMethod()]
        public void Garage_EmptyGarage_AllVehiclesIsEmpty()
        {
            int targetCapacity = 3;
            Garage<Car> garage = new(targetCapacity, "");

            var allVehicles = garage.AllVehicles.ToList();

            Assert.AreEqual(0, allVehicles.Count);
            Assert.AreEqual(targetCapacity, garage.Capacity);
        }

        [TestMethod()]
        public void GarageCapacity_CorrectCapacity_ReturnsExpectedCapacity()
        {
            int targetCapacity = 5;
            Garage<Car> garage = new(targetCapacity, "");
            Assert.AreEqual(targetCapacity, garage.Capacity);
        }

        [TestMethod()]
        public void Garage_NotCreated_AnyTypeOfGarageIsCreated()
        {
            int targetCapacity = 1;
            
            Garage<Vehicle> garage1 = new(targetCapacity, "");
            Garage<Car> garage2 = new(targetCapacity, "");
            Garage<Motorcycle> garage3 = new(targetCapacity, "");
            Garage<Bus> garage4 = new(targetCapacity, "");
            Garage<Boat> garage5 = new(targetCapacity, "");
            Garage<Airplane> garage6 = new(targetCapacity, "");

            Assert.AreEqual(targetCapacity, garage1.Capacity);
            Assert.AreEqual(targetCapacity, garage2.Capacity);
            Assert.AreEqual(targetCapacity, garage3.Capacity);
            Assert.AreEqual(targetCapacity, garage4.Capacity);
            Assert.AreEqual(targetCapacity, garage5.Capacity);
            Assert.AreEqual(targetCapacity, garage6.Capacity);
        }

        [TestCategory("AddVehicle")]
        [TestMethod()]
        public void AddVehicle_UnderCapacity_VehicleIsAdded()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicle = new ("ABC123", "Tesla", "Y", "White", "");

            garage.AddVehicle(vehicle);

            Car? first = (Car?) garage.AllVehicles.FirstOrDefault();
            Assert.AreEqual(vehicle, first);
            Assert.AreEqual(targetCapacity, garage.Capacity);
        }

        [TestMethod()]
        public void AddVehicle_AtCapacity_ThrowsInvalidOperationException()
        {
            int targetCapacity = 1;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicle1 = new ("ABC123", "Tesla", "Y", "White", "");
            Car vehicle2 = new ("DEF456", "BMW", "M3", "Black", "hund");

            garage.AddVehicle(vehicle1);

            Assert.AreEqual(targetCapacity, garage.Capacity);
            Assert.AreEqual(1, garage.AllVehicles.Count());
            Assert.ThrowsException<InvalidOperationException>(() => garage.AddVehicle(vehicle2));
        }

        [TestMethod()]
        public void AddVehicle_NullVehicle_ThrowsArgumentNullException()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");
            Assert.ThrowsException<ArgumentNullException>(() => garage.AddVehicle(null!));
        }

        [TestCategory("AllVehicles")]
        [TestMethod()]
        public void AllVehicles_AfterAddingMultipleVehicles_ReturnsAllAddedVehicles()
        {
            int targetCapacity = 3;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicle1 = new("ABC123", "Tesla", "Y", "White", "");
            Car vehicle2 = new("DEF456", "BMW", "M3", "Black", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);

            var allVehicles = garage.AllVehicles.ToList();
            Assert.AreEqual(2, allVehicles.Count);
            Assert.AreEqual(vehicle1, allVehicles[0]);
            Assert.AreEqual(vehicle2, allVehicles[1]);
            Assert.AreEqual(targetCapacity, garage.Capacity);
        }

        [TestCategory("RemoveVehicle")]
        [TestMethod()]
        public void RemoveVehicle_RemoveFirst_VehicleIsRemoved()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicleToRemove = new("DEF456", "BMW", "M3", "Black", "");
            Car vehicle = new("ABC123", "Tesla", "Y", "White", "");


            garage.AddVehicle(vehicleToRemove);
            garage.AddVehicle(vehicle);

            garage.RemoveVehicle(0);
            var allVehicles = garage.AllVehicles.ToList();

            Assert.AreEqual(targetCapacity, garage.Capacity);
            Assert.AreEqual(1, allVehicles.Count);
            Assert.AreEqual(vehicle, allVehicles[0]);
        }

        [TestMethod()]
        public void RemoveVehicle_RemoveLast_VehiclesIsEmpty()
        {
            int targetCapacity = 1;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicle1 = new("ABC123", "Tesla", "Y", "White", "");

            garage.AddVehicle(vehicle1);

            garage.RemoveVehicle(0);
            var allVehicles = garage.AllVehicles.ToList();

            Assert.AreEqual(allVehicles.Count, targetCapacity - 1);
        }

        [TestMethod()]
        public void RemoveVehicle_InvalidIndex_ThrowsIndexOutOfRangeException()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicle1 = new("ABC123", "Tesla", "Y", "White", "");
            Car vehicle2 = new("DEF456", "BMW", "M3", "Black", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);

            Assert.ThrowsException<IndexOutOfRangeException>(() => garage.RemoveVehicle(-1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => garage.RemoveVehicle(2));
        }

        [TestMethod()]
        public void GarageCount_CorrectCount_ReturnsCorrectCount()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicle1 = new("ABC123", "Tesla", "Y", "White", "");
            Car vehicle2 = new("XYZ123", "Ford", "Focus", "Neon Green", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);

            Assert.AreEqual(garage.Count, targetCapacity);

            garage.RemoveVehicle(0);

            Assert.AreEqual(garage.Count, targetCapacity - 1);
        }

        [TestCategory("Find and Search")]
        [TestMethod()]
        public void FindByRegistration_VehicleExists_ReturnVehicle()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicle1 = new("ABC123", "Tesla", "Y", "White", "");
            Car vehicle2 = new("XYZ123", "Ford", "Focus", "Neon Green", "");
            string registrationToFind = "xyz123";

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            Vehicle? found = garage.FindVehicleByRegistration(registrationToFind);

            Assert.IsNotNull(found);
            Assert.AreEqual(vehicle2, found);
        }


        [TestMethod()]
        public void FindByRegistration_VehicleDoesNotExist_ReturnNull()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");
            Car vehicle1 = new("ABC123", "Tesla", "Y", "White", "");
            Car vehicle2 = new("XYZ123", "Ford", "Focus", "Neon Green", "");
            string registrationToFind = "GXS456";

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            Vehicle? found = garage.FindVehicleByRegistration(registrationToFind);

            Assert.IsNull(found);
            Assert.AreEqual(garage.Count, targetCapacity);
        }

        [TestMethod()]
        public void Search_VehiclesMatchSearchTerm_ReturnsMatchingVehicles()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");

            Car vehicle1 = new("ABC123", "Tesla", "Y", "Neon Green", "");
            Car vehicle2 = new("XYZ123", "Lamborghini", "Murcielago", "Neon Green", "");
            string searchTerm = "color=Neon Green";

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            IEnumerable<IVehicle> results = garage.SearchVehicles(searchTerm);

            Assert.IsNotNull(results);
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1, vehicle2 }, results.ToList());
        }

        [TestMethod()]
        public void Search_EmptySearchTerm_ReturnsAllVehicles()
        {
            int targetCapacity = 2;
            Garage<Car> garage = new(targetCapacity, "");

            Car vehicle1 = new("ABC123", "Tesla", "Y", "Neon Green", "");
            Car vehicle2 = new("XYZ123", "Lamborghini", "Murcielago", "Neon Green", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            IEnumerable<Vehicle> results = garage.SearchVehicles(string.Empty);

            Assert.IsNotNull(results);
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1, vehicle2 }, results.ToList());
        }

        [TestMethod()]
        public void Search_MathcingManyProps_ReturnsRedFerrariCarsWithRegistration()
        {
            int targetCapacity = 6;
            Garage<Car> garage = new(targetCapacity, "");

            Car vehicle1 = new("XYZ789", "Ford", "Mustang", "Red", "");
            Car vehicle2 = new("XYZ789", "Tesla", "X", "Red", "");
            Car vehicle3 = new("XYZ789", "Ferrari", "458 Italia", "Red", "");
            Car vehicle4 = new("GHI012", "Ferrari", "F8 Tributo", "Red", "");
            Car vehicle5 = new("XYZ123", "Lamborghini", "Murcielago", "Neon Green", "");
            Car vehicle6 = new("PQR234", "Chevrolet", "Camaro", "Blue", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            garage.AddVehicle(vehicle3);
            garage.AddVehicle(vehicle4);
            garage.AddVehicle(vehicle5);
            garage.AddVehicle(vehicle6);
            IEnumerable<Vehicle> results = garage.SearchVehicles("type=car;color=red;make=Ferrari;registration=GHI012");

            Assert.AreEqual(1, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle4 }, results.ToList());
        }

        [TestMethod()]
        public void Search_MathcingExclusiveProps_ReturnsCarWithDogs()
        {
            int targetCapacity = 6;
            Garage<Car> garage = new(targetCapacity, "");

            Car vehicle1 = new("XYZ789", "Ford", "Mustang", "Red", "");
            Car vehicle2 = new("XYZ789", "Tesla", "X", "Red", "dogs");
            Car vehicle3 = new("XYZ123", "Lamborghini", "Murcielago", "Neon Green", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            garage.AddVehicle(vehicle3);
            IEnumerable<Vehicle> results = garage.SearchVehicles("trunkcontent=dogs");

            Assert.AreEqual(1, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle2 }, results.ToList());
        }

        [TestCategory("Exception Tests")]
        [TestMethod()]
        public void GetVehicle_IndexOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            int targetCapacity = 1;
            Garage<Car> garage = new(targetCapacity, "");

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => garage.GetVehicleAtIndex(5));
        }

        [TestMethod()]
        public void AddVehicle_GarageFull_ThrowsInvalidOperationException()
        {
            int targetCapacity = 1;
            Garage<Car> garage = new(targetCapacity, "");

            Car vehicle1 = new("LMN456", "Chevrolet", "Camaro", "Yellow", "");
            Car vehicle2 = new("OPQ789", "Lamborghini", "Murcielago", "Red", "");

            garage.AddVehicle(vehicle1);

            Assert.ThrowsException<InvalidOperationException>(() => garage.AddVehicle(vehicle2));
        }

        [TestMethod()]
        public void RemoveVehicle_IndexOutOfRange_ThrowsIndexOutOfRangeException()
        {
            int targetCapacity = 1;
            Garage<Car> garage = new(targetCapacity, "");

            Assert.ThrowsException<IndexOutOfRangeException>(() => garage.RemoveVehicle(3));
        }

        [TestMethod()]
        public void RemoveVehicle_IndexOutOfRangeNegativeIndex_ThrowsIndexOutOfRangeException()
        {
            int targetCapacity = 1;
            Garage<Car> garage = new(targetCapacity, "");

            Assert.ThrowsException<IndexOutOfRangeException>(() => garage.RemoveVehicle(-3));
        }
    }
}