using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Management
{    public interface IGarageFactory
    {
        IHandler Create(string type, int capacity, string name);
    }
}
