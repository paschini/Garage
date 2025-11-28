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
        IVehicle? Create(IVehicleInput? input);
        Car CreateCar(string registrationNumber, string make, string model, string color, string trunkContent);
        Motorcycle CreateMotorcycle(string registrationNumber, string make, string model, string color, bool isUtility);
        Bus CreateBus(string registrationNumber, string make, string model, string color, string linjeID);
        Boat CreateBoat(string registrationNumber, string make, string model, string color, string boatType);
        Airplane CreateAirplane(string registrationNumber, string make, string model, string color, double wingSpan, int numberOfEngines);
    }
}
