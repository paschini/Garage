using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class Boat : Vehicle
    {
        public string BoatType { get; }
        public Boat(string registration, string make, string model, string color, string boatType) : base(registration, make, model, color)
        {
            Registration = registration;
            Make = make;
            Model = model;
            Color = color;
            BoatType = boatType;
        }
    }
}
