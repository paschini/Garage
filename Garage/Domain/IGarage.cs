namespace Garage.Domain
{
    public interface IGarage 
    {
        public Type VehicleType { get; }
        string Name { get; }

        int Capacity { get; }

        int Count { get; }

        float AvailablePlaces { get; }

        IEnumerable<IVehicle> AllVehicles { get; }

        void LoadVehicles(IEnumerable<IVehicle> vehicles);

        void AddVehicle(IVehicle vehicle);

        IVehicle RemoveVehicle(int index);

        IVehicle GetVehicleAtIndex(int index);

        IVehicle? FindVehicleByRegistration(string registration);

        IEnumerable<IVehicle> SearchVehicles(string searchTerm);

    }

    public interface IGarage<T> where T : IVehicle
    {
        //public Type VehicleType { get; }
        //string Name { get; }

        //int Capacity { get; }

        //int Count { get; }

        //float AvailablePlaces { get; }
        //IEnumerable<T> AllVehicles { get; }
        //new IEnumerable<T> AllVehicles { get; }

        void AddVehicle(T vehicle);

        T RemoveVehicle(int index);

        T GetVehicleAtIndex(int index);

        T? FindVehicleByRegistration(string registration);

        IEnumerable<T> SearchVehicles(string searchTerm);

        void LoadVehicles(IEnumerable<T> vehicles);
    }
}