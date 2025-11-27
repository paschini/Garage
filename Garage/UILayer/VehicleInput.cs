using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.UILayer
{
    public class VehicleInput 
    {
        public record AirplaneInputDTO : IVehicleInput
        {
            public string RegistrationNumber { get; set; } = string.Empty;

            public string Make { get; set; } = string.Empty;

            public string Model { get; set; } = string.Empty;

            public string Color { get; set; } = string.Empty;

            public double WingSpan { get; set; } = 0d;

            public int NumberOfEngines { get; set; } = 0;

            public static AirplaneInputDTO Build(string regNumber, string make, string model, string color, double wingSpan, int engines)
            {
                return new AirplaneInputDTO
                {
                    RegistrationNumber = regNumber,
                    Make = make,
                    Model = model,
                    Color = color,
                    WingSpan = wingSpan,
                    NumberOfEngines = engines
                };
             }
        }

        

        public record BoatInputDTO : IVehicleInput
        {
            public string RegistrationNumber { get; set; } = string.Empty;

            public string Make { get; set; } = string.Empty;

            public string Model { get; set; } = string.Empty;

            public string Color { get; set; } = string.Empty;

            public string BoatType { get; set; } = string.Empty;

            public static BoatInputDTO Build(string regNumber, string make, string model, string color, string boatType)
            {
                return new BoatInputDTO
                {
                    RegistrationNumber = regNumber,
                    Make = make,
                    Model = model,
                    Color = color,
                    BoatType = boatType
                };
            }
        }

        public record BusInputDTO : IVehicleInput
        {
            public string RegistrationNumber { get; set; } = string.Empty;

            public string Make { get; set; } = string.Empty;

            public string Model { get; set; } = string.Empty;

            public string Color { get; set; } = string.Empty;

            public string LineID { get; set; } = string.Empty;

            public static BusInputDTO Build(string regNumber, string make, string model, string color, string lineID)
            {
                return new BusInputDTO
                {
                    RegistrationNumber = regNumber,
                    Make = make,
                    Model = model,
                    Color = color,
                    LineID = lineID
                };
            }
        }

        public record CarInputDTO : IVehicleInput
        {
            public string RegistrationNumber { get; set; } = string.Empty;

            public string Make { get; set; } = string.Empty;

            public string Model { get; set; } = string.Empty;

            public string Color { get; set; } = string.Empty;

            public string TrunkContent { get; set; } = string.Empty;

            public static CarInputDTO Build(string regNumber, string make, string model, string color, string trunkContent)
            {
                return new CarInputDTO
                {
                    RegistrationNumber = regNumber,
                    Make = make,
                    Model = model,
                    Color = color,
                    TrunkContent = trunkContent
                };
            }
        }

        public record MotorcycleInputDTO : IVehicleInput
        {
            public string RegistrationNumber { get; set; } = string.Empty;

            public string Make { get; set; } = string.Empty;

            public string Model { get; set; } = string.Empty;

            public string Color { get; set; } = string.Empty;

            public bool IsUtility { get; set; } = false;

            public static MotorcycleInputDTO Build(string regNumber, string make, string model, string color, bool isUtility)
            {
                return new MotorcycleInputDTO 
                {
                    RegistrationNumber = regNumber,
                    Make = make,
                    Model = model,
                    Color = color,
                    IsUtility = isUtility
                };
            }
        }
    }
}
