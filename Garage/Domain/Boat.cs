using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Domain
{
    public class Boat : Vehicle, IVehicle
    {
        public string BoatType { get; }
        public Boat(string registration, string make, string model, string color, string boatType) : base(registration, make, model, color)
        {
            BoatType = boatType;
        }
    }
}
