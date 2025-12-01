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

        int CountPlaces(IVehicle vehicle);

        void LoadVehicles(IEnumerable<IVehicle> vehicles);

        void AddVehicle(IVehicle vehicle);

        IVehicle RemoveVehicle(int index);

        IVehicle GetVehicleAtIndex(int index);

        IVehicle? FindVehicleByRegistration(string registration);

        IEnumerable<IVehicle> SearchVehicles(string searchTerm);

    }

    public interface IGarage<T> where T : IVehicle
    {
        int CountPlaces(T vehicle);

        void AddVehicle(T vehicle);

        T RemoveVehicle(int index);

        T GetVehicleAtIndex(int index);

        T? FindVehicleByRegistration(string registration);

        IEnumerable<T> SearchVehicles(string searchTerm);

        void LoadVehicles(IEnumerable<T> vehicles);
    }
}