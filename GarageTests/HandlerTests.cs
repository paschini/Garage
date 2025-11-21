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
    }
}