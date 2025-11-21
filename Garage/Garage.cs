using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class Garage<T> where T : Vehicle
    {
        private readonly T[] _vehicles;
        private int _count { get; set; }
        public int Capacity { get; }
        public IEnumerable<T> AllVehicles => _vehicles.Take(_count);

        public Garage(int capacity)
        {
            _vehicles = new T[capacity];
            _count = 0;
            Capacity = capacity;
        }

        public void AddVehicle(T vehicle)
        {
            if (_count >= Capacity) throw new InvalidOperationException("Garage is full!");

            if (vehicle is null) throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null!");

            _vehicles[_count] = vehicle;
            _count++;
        }

        //private int FindVehicleIndex(Vehicle vehicle)
        //{
        //    if (vehicle is null) throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null!");

        //    for (int i = 0; i < _count; i++)
        //    {
        //        if (_vehicles[i].Equals(vehicle)) return i;
        //    }

        //    return -1; // kan inte hittas
        //}

        public T RemoveVehicle(int index)
        {
            if (index > -1 && _count > 1)
            {
                T removedVehicle = _vehicles[index];
                // Flytta alla andra 1 pos uppe
                for (int i = index; i < _count - 1; i++)
                {
                    _vehicles[i] = _vehicles[i + 1];
                }
                _vehicles[_count - 1] = null!; // Rensa ut sistan
                _count--;
                return removedVehicle;
            }
            
            if (index == 0 && _count == 1)
            {
                T removedVehicle = _vehicles[index];
                // Endast ett fordon i garaget
                _vehicles[0] = null!;
                _count--;
                return removedVehicle;
            }

            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range!");
        }
    }
}
