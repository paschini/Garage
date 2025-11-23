namespace Garage
{
    public interface IGarage
    {
        string Name { get; }

        Type VehicleType { get; }

        int Capacity { get; }

        int Count { get; }

        int AvailablePlaces { get; }
    }
    public interface IGarage<T> : IGarage where T : IVehicle
    {
        void AddVehicle(T vehicle);

        T RemoveVehicle(int index);

        IEnumerable<T> AllVehicles { get; }

        T GetVehicleAtIndex(int index);

        T? FindVehicleByRegistration(string registration);

        IEnumerable<T> SearchVehicles(string searchTerm);

        void LoadVehicles(IEnumerable<T> vehicles);
    }
}