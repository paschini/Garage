using Garage.UILayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Domain
{
    public interface IVehicleFactory
    {
        public IVehicle? Create(IVehicleInput? input);
        public Car CreateCar(string registrationNumber, string make, string model, string color, string trunkContent);
        public Motorcycle CreateMotorcycle(string registrationNumber, string make, string model, string color, bool isUtility);
        public Bus CreateBus(string registrationNumber, string make, string model, string color, string linjeID);
        public Boat CreateBoat(string registrationNumber, string make, string model, string color, string boatType);
        public Airplane CreateAirplane(string registrationNumber, string make, string model, string color, double wingSpan, int numberOfEngines);
    }
}
