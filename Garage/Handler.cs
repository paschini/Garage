using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{   
    internal class Handler
    {
        private Garage? _garage { get; } = null;

        public bool GarageNotInitialised => _garage is null;

        public void CreateGarage(int capacity)
        {

        }
    }
}
