using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Domain
{
    public class Motorcycle : Vehicle, IVehicle
    {
        public bool IsUtility { get; } = false;
        public Motorcycle(string registration, string make, string model, string color, bool isUtility) : base(registration, make, model, color)
        {
            IsUtility = isUtility;
        }
    }
}
