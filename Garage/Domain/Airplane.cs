using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Domain
{
    public class Airplane : Vehicle, IVehicle
    {
        public double WingSpan { get; }
        public int NumberOfEngines { get; } = 1;
        public Airplane(string registration, string make, string model, string color, double wingspan, int numberOfEngines) : base(registration, make, model, color)
        {
            WingSpan = wingspan;
            NumberOfEngines = numberOfEngines;
        }
    }
}
