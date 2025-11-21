using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class Airplane : Vehicle
    {
        private double _WingSpan { get; }
        private int _NumberOfEngines { get; } = 1;
        public Airplane(string registration, string make, string model, string color, double wingspan, int numberOfEngines) : base(registration, make, model, color)
        {
            Registration = registration;
            Make = make;
            Model = model;
            Color = color;
            _WingSpan = wingspan;
            _NumberOfEngines = numberOfEngines;
        }
    }
}
