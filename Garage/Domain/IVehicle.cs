using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Domain
{
    public interface IVehicle
    {
        string Type { get; }
        string Registration { get; set; }
        string Make { get; set; }
        string Model { get; set; }
        string Color { get; set; }



        //public IVehicle ToIVehicle()
        //{
        //    return this as IVehicle
        //}
    }
}
