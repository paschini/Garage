using Garage.UILayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Garage.UILayer.VehicleInput;

namespace Garage.Domain
{
    internal class VehicleFactory : IVehicleFactory
    {
        public IVehicle? Create(IVehicleInput? input)
        {
            if(input != null) { return null; }

            return input switch
            {
                AirplaneInputDTO airplane => CreateAirplane(airplane.RegistrationNumber, airplane.Make, airplane.Model, airplane.Color, airplane.WingSpan, airplane.NumberOfEngines),
                CarInputDTO car => CreateCar(car.RegistrationNumber, car.Make, car.Model, car.Color, car.TrunkContent),
                BoatInputDTO boat => CreateBoat(boat.RegistrationNumber, boat.Make, boat.Model, boat.Color, boat.BoatType),
                BusInputDTO bus => CreateBus(bus.RegistrationNumber, bus.Make, bus.Model, bus.Color, bus.LineID),
                MotorcycleInputDTO moto => CreateMotorcycle(moto.RegistrationNumber, moto.Make, moto.Model, moto.Color, moto.IsUtility),

                _ => throw new InvalidOperationException("Unsupported vehicle input type")
            };
        }

        public Car CreateCar(string registrationNumber, string make, string model, string color, string trunkContent)
        {
            return new Car(registrationNumber, make, model, color, trunkContent);
        }

        public Motorcycle CreateMotorcycle(string registrationNumber, string make, string model, string color, bool isUtility)
        {
            return new Motorcycle(registrationNumber, make, model, color, isUtility);
        }

        public Bus CreateBus(string registrationNumber, string make, string model, string color, string linjeID)
        {
            return new Bus(registrationNumber, make, model, color, linjeID);
        }

        public Boat CreateBoat(string registrationNumber, string make, string model, string color, string boatType)
        {
            return new Boat(registrationNumber, make, model, color, boatType);
        }

        public Airplane CreateAirplane(string registrationNumber, string make, string model, string color, double wingSpan, int numberOfEngines)
        {
            return new Airplane(registrationNumber, make, model, color, wingSpan, numberOfEngines);
        }
    }
}
