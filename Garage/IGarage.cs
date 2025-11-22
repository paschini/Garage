namespace Garage
{
    public interface IGarage<T> where T : IVehicle
    {
        int Capacity { get; }

        int Count { get; }

        void AddVehicle(T vehicle);

        T RemoveVehicle(int index);

        IEnumerable<T> AllVehicles { get; }

        T GetVehicleAtIndex(int index);

        T? FindVehicleByRegistration(string registration);

        IEnumerable<T> SearchVehicles(string searchTerm);
    }
}