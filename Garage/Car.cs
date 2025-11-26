using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageSystem
{
    public class Car : Vehicle, IVehicle
    {
        public string TrunkContent { get; } = string.Empty;

        public Car(string registration, string make, string model, string color, string trunkContent) : base(registration, make, model, color)
        {
            TrunkContent = trunkContent;
        }
    }
}

