namespace Garage
{
    public interface IGarage<T> where T : IVehicle
    {
        string Name { get; }
        int Capacity { get; }

        int Count { get; }

        int AvailablePlaces { get; }

        void AddVehicle(T vehicle);

        T RemoveVehicle(int index);

        IEnumerable<T> AllVehicles { get; }

        T GetVehicleAtIndex(int index);

        T? FindVehicleByRegistration(string registration);

        IEnumerable<T> SearchVehicles(string searchTerm);

        void LoadVehicles(IEnumerable<T> vehicles);
    }
}