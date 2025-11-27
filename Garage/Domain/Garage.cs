using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Domain
{
    public class Garage<T> : IGarage, IGarage<T>, IEnumerable<T> where T : IVehicle
    {
        private T[] _vehicles;
        public Type VehicleType => typeof(T);
        public string Name { get; }
        public int Count { get; private set; }
        public int Capacity { get; private set; }
        
        private float _availablePlaces;
        public float AvailablePlaces
        {
            get => _availablePlaces / 3;
            private set => _availablePlaces = value;
        }

        public IEnumerable<IVehicle> AllVehicles => (IEnumerable<IVehicle>)_vehicles.Take(Count);

        public Garage(int capacity, string name)
        {
            Name = name;
            _vehicles = new T[capacity];
            Count = 0;
            Capacity = capacity;
            AvailablePlaces = capacity * 3;
        }

        private int CountPlaces(T vehicle)
        {
            switch (vehicle)
            {
                case Airplane:
                    return 9; // 3 platser
                case Motorcycle:
                    return  1; // 1/3 plats
                case Boat:
                    return 6; // 2
                default:
                    return 3; // 1
            }
        }

        public void AddVehicle(T vehicle)
        {
            //if (Count >= Capacity) throw new InvalidOperationException("Garage is full!");
            if (_availablePlaces - CountPlaces(vehicle) < 0) throw new InvalidOperationException($"Garage is full! available places: {AvailablePlaces}");

            if (vehicle is null) throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null!");

            _vehicles[Count] = vehicle;
            Count++;
            _availablePlaces -= CountPlaces(vehicle);
        }

        public T RemoveVehicle(int index)
        {
            if (index > -1 && Count > 1)
            {
                T removedVehicle = _vehicles[index];
                // Flytta alla andra 1 pos uppe
                for (int i = index; i < Count - 1; i++)
                {
                    _vehicles[i] = _vehicles[i + 1];
                }
                _vehicles[Count - 1] = default!; // Rensa ut sistan
                Count--;
                AvailablePlaces += CountPlaces(removedVehicle);
                return removedVehicle;
            }
            
            if (index == 0 && Count == 1)
            {
                T removedVehicle = _vehicles[index];
                // Första är endast ett fordon i garaget
                _vehicles[0] = default!;
                Count--;
                AvailablePlaces += CountPlaces(removedVehicle);
                return removedVehicle;
            }

            throw new IndexOutOfRangeException("Index is out of range!");
        }

        public T GetVehicleAtIndex(int index)
        {
            if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range!");
            return _vehicles[index];
        }

        public T? FindVehicleByRegistration(string registration)
        {
            return _vehicles.Take(Count).FirstOrDefault(v => v.Registration.Equals(registration, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<T> SearchVehicles(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return _vehicles.Take(Count);

            var filters = searchTerm.Split(';', StringSplitOptions.RemoveEmptyEntries)
                               .Select(f => f.Split('='))
                               .Where(parts => parts.Length == 2)
                               .Select(parts => new { Property = parts[0].Trim(), Value = parts[1].Trim() })
                               .ToList();

            return _vehicles
                .Where(v => v != null)
                .Where(vehicle =>
                {
                    foreach (var filter in filters)
                    {
                        var prop = vehicle.GetType().GetProperty(filter.Property,
                            System.Reflection.BindingFlags.IgnoreCase |
                            System.Reflection.BindingFlags.Public |
                            System.Reflection.BindingFlags.Instance);

                        if (prop == null) return false;

                        var propValue = prop.GetValue(vehicle)?.ToString() ?? "";

                        if (!propValue.Equals(filter.Value, StringComparison.OrdinalIgnoreCase)) return false;
                    }
                    return true;
                });
        }

        public void LoadVehicles(IEnumerable<T> vehicles)
        {
            var list = vehicles.ToList();

            Capacity = list.Count + 3;
            _vehicles = new T[Capacity];

            for (int i = 0; i < list.Count; i++)
            {
                _vehicles[i] = list[i];
            }

            Count = list.Count;
        }

        public void LoadVehicles(IEnumerable<IVehicle> vehicle)
        {

        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_vehicles.Take(Count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddVehicle(IVehicle vehicle)
        {
            AddVehicle((T) vehicle);
        }

        IVehicle IGarage.RemoveVehicle(int index)
        {
            return RemoveVehicle(index);
        }

        IVehicle IGarage.GetVehicleAtIndex(int index)
        {
            return GetVehicleAtIndex(index);
        }

        IVehicle? IGarage.FindVehicleByRegistration(string registration)
        {
            return FindVehicleByRegistration(registration);
        }

        IEnumerable<IVehicle> IGarage.SearchVehicles(string searchTerm)
        {
            IEnumerable<T> vehicles = _vehicles;
            return (IEnumerable<IVehicle>) SearchVehicles(searchTerm);
        }
    }
}
