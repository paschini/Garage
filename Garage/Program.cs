using Garage.Domain;
using Garage.Management;
using Garage.UILayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GarageSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {   
                    services.AddSingleton<Manager>();
                    services.AddSingleton<ConfigRepository>();
                    services.AddSingleton<IUI, ConsoleUI>();
                    services.AddSingleton<IHandler, GarageHandler>();
                    services.AddSingleton<IGarageFactory, GarageFactory>();
                    services.AddSingleton<IVehicleFactory, VehicleFactory>();
                })
                .UseConsoleLifetime()
                .Build();

            host.Services.GetRequiredService<Manager>().Run();
        }
    }
}
