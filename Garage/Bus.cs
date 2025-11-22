using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class Bus : Vehicle
    {
        public string LinjeID { get; }
        public Bus(string registration, string make, string model, string color, string linjeID) : base(registration, make, model, color)
        {
            Registration = registration;
            Make = make;
            Model = model;
            Color = color;
            LinjeID = linjeID;
        }
    }
}
