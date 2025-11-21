using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class Motorcykel : Vehicle
    {
        private bool _IsUtility { get; } = false;
        public Motorcykel(string registration, string make, string model, string color, bool isUtility) : base(registration, make, model, color)
        {
            Registration = registration;
            Make = make;
            Model = model;
            Color = color;
            _IsUtility = isUtility;
        }
    }
}
