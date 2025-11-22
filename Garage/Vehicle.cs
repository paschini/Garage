using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public abstract class Vehicle : IVehicle, IEquatable<Vehicle>
    {
        public string Type => this.GetType().Name;
        public string Registration { get; set; }
        public string Make { get; set; }
        public string Model { get; set; } 
        public string Color { get; set; }

        public Vehicle(string registrationNumber, string make, string model, string color)
        {
            Registration = registrationNumber;
            Make = make;
            Model = model;
            Color = color;
        }

        public bool Equals(Vehicle? other)
        {
            if (Registration == other?.Registration && Make == other.Make && Model == other.Model && Color == other.Color)
            {
                return true;
            }

            return false;
        }
    }
}
