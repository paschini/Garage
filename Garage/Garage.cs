using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal class Garage
    {
        private int Capacity { get; set; } // måste vara användar definierade

        public Garage(int capacity)
        {
            Capacity = capacity;
        }
    }
}
