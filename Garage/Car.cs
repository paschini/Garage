using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class Car : Vehicle
    {
        public string TrunkContent { get; } = string.Empty;

        public Car(string registration, string make, string model, string color, string trunkContent) : base(registration, make, model, color)
        {
            Registration = registration;
            Make = make;
            Model = model;
            Color = color;

            TrunkContent = trunkContent;
        }
    }
}

