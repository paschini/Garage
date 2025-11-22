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
    public class GarageTests
    {
        [TestCategory("Constructor")]
        [TestMethod()]
        public void Garage_EmptyGarage_AllVehiclesIsEmpty()
        {
            int targetCapacity = 3;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            var allVehicles = garage.AllVehicles.ToList();

            Assert.AreEqual(0, allVehicles.Count);
            Assert.AreEqual(targetCapacity, garage.Capacity);
        }

        [TestMethod()]
        public void GarageCapacity_CorrectCapacity_ReturnsExpectedCapacity()
        {
            int targetCapacity = 5;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Assert.AreEqual(targetCapacity, garage.Capacity);
        }

        [TestCategory("AddVehicle")]
        [TestMethod()]
        public void AddVehicle_UnderCapacity_VehicleIsAdded()
        {
            int targetCapacity = 2;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle = new Car("ABC123", "Tesla", "Y", "White", "");

            garage.AddVehicle(vehicle);

            Vehicle? first = garage.AllVehicles.FirstOrDefault();
            Assert.AreEqual(vehicle, first);
            Assert.AreEqual(targetCapacity, garage.Capacity);
        }

        [TestMethod()]
        public void AddVehicle_AtCapacity_ThrowsInvalidOperationException()
        {
            int targetCapacity = 1;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "White", "");
            Vehicle vehicle2 = new Car("DEF456", "BMW", "M3", "Black", "hund");

            garage.AddVehicle(vehicle1);

            Assert.AreEqual(targetCapacity, garage.Capacity);
            Assert.AreEqual(1, garage.AllVehicles.Count());
            Assert.ThrowsException<InvalidOperationException>(() => garage.AddVehicle(vehicle2));
        }

        [TestMethod()]
        public void AddVehicle_NullVehicle_ThrowsArgumentNullException()
        {
            int targetCapacity = 2;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Assert.ThrowsException<ArgumentNullException>(() => garage.AddVehicle(null!));
        }

        [TestCategory("AllVehicles")]
        [TestMethod()]
        public void AllVehicles_AfterAddingMultipleVehicles_ReturnsAllAddedVehicles()
        {
            int targetCapacity = 3;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "White", "");
            Vehicle vehicle2 = new Car("DEF456", "BMW", "M3", "Black", "");

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
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicleToRemove = new Car("DEF456", "BMW", "M3", "Black", "");
            Vehicle vehicle = new Car("ABC123", "Tesla", "Y", "White", "");


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
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "White", "");

            garage.AddVehicle(vehicle1);

            garage.RemoveVehicle(0);
            var allVehicles = garage.AllVehicles.ToList();

            Assert.AreEqual(allVehicles.Count, 0);
        }

        [TestMethod()]
        public void RemoveVehicle_InvalidIndex_ThrowsIndexOutOfRangeException()
        {
            int targetCapacity = 2;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "White", "");
            Vehicle vehicle2 = new Car("DEF456", "BMW", "M3", "Black", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);

            Assert.ThrowsException<IndexOutOfRangeException>(() => garage.RemoveVehicle(-1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => garage.RemoveVehicle(2));
        }

        [TestMethod()]
        public void GarageCount_CorrectCount_ReturnsCorrectCount()
        {
            int targetCapacity = 2;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "White", "");
            Vehicle vehicle2 = new Motorcycle("XYZ123", "Kawasaki", "Ninja", "Neon Green", false);

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);

            Assert.AreEqual(garage.Count, 2);

            garage.RemoveVehicle(0);

            Assert.AreEqual(garage.Count, 1);
        }

        [TestCategory("Find and Search")]
        [TestMethod()]
        public void FindByRegistration_VehicleExists_ReturnVehicle()
        {
            int targetCapacity = 2;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "White", "");
            Vehicle vehicle2 = new Motorcycle("XYZ123", "Kawasaki", "Ninja", "Neon Green", false);
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
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "White", "");
            Vehicle vehicle2 = new Motorcycle("XYZ123", "Kawasaki", "Ninja", "Neon Green", false);
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
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "Neon Green", "");
            Vehicle vehicle2 = new Motorcycle("XYZ123", "Kawasaki", "Ninja", "Neon Green", false);
            string searchTerm = "color=Neon Green";

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            IEnumerable<Vehicle> results = garage.SearchVehicles(searchTerm);

            Assert.IsNotNull(results);
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1, vehicle2 }, results.ToList());
        }

        [TestMethod()]
        public void Search_EmptySearchTerm_ReturnsAllVehicles()
        {
            int targetCapacity = 2;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "Neon Green", "");
            Vehicle vehicle2 = new Motorcycle("XYZ123", "Kawasaki", "Ninja", "Neon Green", false);

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            IEnumerable<Vehicle> results = garage.SearchVehicles(string.Empty);

            Assert.IsNotNull(results);
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1, vehicle2 }, results.ToList());
        }

        [TestMethod()]
        public void Search_MatchingType_ReturnsRedCars()
        {
            int targetCapacity = 3;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Red", true);
            Vehicle vehicle3 = new Car("PQR234", "Chevrolet", "Camaro", "Blue", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);
            garage.AddVehicle(vehicle3);
            IEnumerable<Vehicle> results = garage.SearchVehicles("type=car;color=red");

            Assert.AreEqual(1, results.Count());
            CollectionAssert.AreEquivalent(new List<Vehicle> { vehicle1 }, results.ToList());
        }

        [TestMethod()]
        public void Search_MathcingManyProps_ReturnsRedFerrariCarsWithRegistration()
        {
            int targetCapacity = 6;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Car("XYZ789", "Tesla", "X", "Red", "");
            Vehicle vehicle3 = new Car("XYZ789", "Ferrari", "458 Italia", "Red", "");
            Vehicle vehicle4 = new Car("GHI012", "Ferrari", "F8 Tributo", "Red", "");
            Vehicle vehicle5 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Red", true);
            Vehicle vehicle6 = new Car("PQR234", "Chevrolet", "Camaro", "Blue", "");

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
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Vehicle vehicle1 = new Car("XYZ789", "Ford", "Mustang", "Red", "");
            Vehicle vehicle2 = new Car("XYZ789", "Tesla", "X", "Red", "dogs");
            Vehicle vehicle3 = new Motorcycle("GHI012", "Kawasaki", "Ninja", "Red", true);

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
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => garage.GetVehicleAtIndex(5));
        }

        [TestMethod()]
        public void AddVehicle_GarageFull_ThrowsInvalidOperationException()
        {
            int targetCapacity = 1;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Vehicle vehicle1 = new Car("LMN456", "Chevrolet", "Camaro", "Yellow", "");
            Vehicle vehicle2 = new Motorcycle("OPQ789", "Ducati", "Monster", "Red", false);

            garage.AddVehicle(vehicle1);

            Assert.ThrowsException<InvalidOperationException>(() => garage.AddVehicle(vehicle2));
        }

        [TestMethod()]
        public void RemoveVehicle_IndexOutOfRange_ThrowsIndexOutOfRangeException()
        {
            int targetCapacity = 1;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Assert.ThrowsException<IndexOutOfRangeException>(() => garage.RemoveVehicle(3));
        }

        [TestMethod()]
        public void RemoveVehicle_IndexOutOfRangeNegativeIndex_ThrowsIndexOutOfRangeException()
        {
            int targetCapacity = 1;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);

            Assert.ThrowsException<IndexOutOfRangeException>(() => garage.RemoveVehicle(-3));
        }
    }
}