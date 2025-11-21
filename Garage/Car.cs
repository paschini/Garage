using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class Car : Vehicle
    {
        private string _TrunkContent { get; } = string.Empty;

        public Car(string registration, string make, string model, string color, string trunkContent) : base(registration, make, model, color)
        {
            Registration = registration;
            Make = make;
            Model = model;
            Color = color;

            _TrunkContent = trunkContent;
        }
    }
}

