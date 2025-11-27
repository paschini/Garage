using Garage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Management
{
    public class GarageFactory : IGarageFactory
    {
        public IHandler Create(string? type, int capacity, string name = "")
        {
            if (type == null || type == string.Empty || capacity == 0) throw new ArgumentException("Unknown garage type");

            return type.ToLower() switch
            {
                "vehicle" => new GarageHandler(new Garage<Vehicle>(capacity, name)),
                "car" => new GarageHandler(new Garage<Car>(capacity, name)),
                "motorcycle" => new GarageHandler(new Garage<Motorcycle>(capacity, name)),
                "boat" => new GarageHandler(new Garage<Boat>(capacity, name)),
                "bus" => new GarageHandler(new Garage<Bus>(capacity, name)),
                "airplane" => new GarageHandler(new Garage<Airplane>(capacity, name)),
                
                _ => throw new ArgumentException("Unknown garage type")
            };
        }
    }
}
