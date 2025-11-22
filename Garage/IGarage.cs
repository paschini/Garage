namespace Garage
{
    public interface IGarage<T> where T : IVehicle
    {
        int Capacity { get; }

        void AddVehicle(T vehicle);

        T RemoveVehicle(int index);

        IEnumerable<T> AllVehicles { get; }        
    }
}