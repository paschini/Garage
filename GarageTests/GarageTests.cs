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

            Assert.Equals(allVehicles.Count, 0);
        }

        [TestMethod()]
        public void RemoveVehicle_InvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            int targetCapacity = 2;
            Garage<Vehicle> garage = new Garage<Vehicle>(targetCapacity);
            Vehicle vehicle1 = new Car("ABC123", "Tesla", "Y", "White", "");
            Vehicle vehicle2 = new Car("DEF456", "BMW", "M3", "Black", "");

            garage.AddVehicle(vehicle1);
            garage.AddVehicle(vehicle2);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => garage.RemoveVehicle(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => garage.RemoveVehicle(2));
        }
    }
}