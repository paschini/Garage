using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Domain
{
    public abstract class Vehicle : IVehicle
    {
        public string Type => GetType().Name;
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
    }
}
