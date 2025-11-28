using Garage.Domain;
using Garage.UILayer;
using GarageSystem;
using System.Diagnostics.Metrics;

namespace Garage.Management
{
    internal class Manager(IGarageFactory factory)
    {
        private IUI _UI = null!; // vet bara om UI
        private readonly IGarageFactory _factory = factory;
        private IHandler? _Handler = null!; // vet inte om UI, vet om Garage
        private string? _GarageTitle { get; set; } = null;
        private string? _GarageType { get; set; } = null;
        private int? _GarageCapacity = null;
        private Config? _Config { get; set; } = null;

        private List<IHandler> _HandlerList { get; set; } = [];
        private IVehicleFactory _VehicleFactory = new VehicleFactory();

        private void CheckGarageInitialised()
        {
            if (!_Handler?.GarageInitialised ?? false)
            {
                _UI.ShowMessage("\nGaraget är inte skapat än. Du måste skappa ett garage först.");
                return;
            }
        }

        private void CreateGarage()
        {
            bool quit = false;
            while (!quit)
            {
                _GarageTitle = _UI.GetStringInput("\nVad ska garaget heta? ", "Namn kan inte vara tomt!");

                _UI.ShowMessage("\nGaraget tillräckligt typer: car, motorcycle, airplane, bus, boat, vehicle.");
                _UI.ShowMessage("Vanliga garaget kan ta bara en typ av fordon.");
                _UI.ShowMessage("Garaget av vehicle typ är endast som kan ta olika fordon.");

                _GarageType = _UI.GetStringInput("Vilken typ av garage vill du ha? ", "Typ kan inte vara tomt!");

                _GarageCapacity = _UI.GetIntInput("Hur många platser till fordon finns i garaget? ", "Kapacitet i garaget måste vara en hel nummer: ");

                try
                {
                    _Handler = _factory.Create(_GarageType ?? "", _GarageCapacity ?? 0, _GarageTitle ?? "IngetNamn");
                    _HandlerList.Add(_Handler);
                    quit = true;
                }
                catch (Exception ex)
                {
                    _UI.ShowError(ex);
                    _GarageTitle = null;
                    _GarageType = null;
                    _GarageCapacity = null;
                }
            }
        }

        private void AddVehicle()
        {
            float placesLeft = _Handler?.GetGaragePlacesLeft() ?? 0;
            _UI.ShowMessage($"\n{_GarageTitle} kan ta {_GarageType} ({_Handler?.GarageType})");
            _UI.ShowMessage($"{_GarageTitle} har {ToFraction(placesLeft)} platser kvar.\n");

            CheckGarageInitialised();

            if (placesLeft <= 0.2f)
            {
                _UI.ShowMessage("\nGaraget har ingen plats kvar. Du måste ta bort minst ett fordon först.");
                return;
            }

            Dictionary<int, string> vehicleMessages;

            vehicleMessages = _UI.CreateAddVehicleMenuOptions(_GarageTitle ?? "", placesLeft, _Handler?.GarageType ?? typeof(Vehicle));
            int chosen = _UI.ShowSubMenu(_GarageTitle ?? "", "Skappa fordon: \n", vehicleMessages);
            Type vehicleChoiceType = chosen switch
            {
                1 => typeof(Motorcycle),
                2 => typeof(Car),
                3 => typeof(Bus),
                4 => typeof(Boat),
                5 => typeof(Airplane),
                _ => typeof(Car)
            };

            IVehicleInput? vehicleToCreate = _UI.GetInputForVehicleOfType(vehicleChoiceType);
            IVehicle? vehicleToAdd = _VehicleFactory?.Create(vehicleToCreate);

            if (vehicleToAdd != null) _Handler?.AddVehicle(vehicleToAdd);

            _UI.ShowMessage($"\nKapacitet nu: {_Handler?.GetGarageCapacity()}");
            _UI.ShowMessage($"Platser kvar nu: {ToFraction(_Handler?.GetGaragePlacesLeft() ?? 0)}\n\n");
        }


        private void ListVehicles()
        {
            CheckGarageInitialised();

            var vehicles = _Handler?.GetAllVehicles();
            int vehicleCount = vehicles?.Count() ?? 0;
            if (vehicleCount == 0)
            {
                _UI.ShowMessage("\nInga fordon i garaget.");
                return;
            }

            _UI.ShowMessage($"\nFordon i garaget {_GarageTitle}:");
            _UI.ShowMessage("-----------------------------------------------------------------");
            for (int index = 0; index < vehicleCount; index++)
            {
                var vehicle = _Handler?.GetVehicle(index);
                try
                {
                    _UI.ShowMessage($"  {index}. {vehicle?.Registration} {vehicle?.Make} {vehicle?.Model} {vehicle?.Color}");
                }
                catch (Exception ex)
                {
                    _UI.ShowError(ex);
                }
            }
            _UI.ShowMessage("-----------------------------------------------------------------\n");
        }

        private void RemoveVehicle()
        {
            if (!_Handler?.GarageInitialised ?? true)
            {
                _UI.ShowMessage("\nGaraget är inte skapat än. Du måste skappa ett garage först.\n");
                return;
            }

            var vehicles = _Handler?.GetAllVehicles();
            int vehicleCount = vehicles?.Count() ?? 0;
            if (vehicleCount == 0)
            {
                _UI.ShowMessage("\nInga fordon i garaget att ta bort.\n");
                return;
            }

            ListVehicles();
            int indexToRemove = _UI.GetIntInput("\nAnge index för fordonet att ta bort: ", "Index måste vara ett giltigt nummer!");
            while (indexToRemove < 0 || indexToRemove >= vehicleCount)
            {
                indexToRemove = _UI.GetIntInput("Ange ett giltigt index för fordonet att ta bort: ", "Index måste vara ett giltigt nummer!");
            }

            try
            {
                IVehicle? removed = _Handler?.RemoveVehicle(indexToRemove);
                _UI.ShowMessage($"Fordonet {removed?.Registration} har tagits bort från garaget.\n");
            }
            catch (Exception ex)
            {
                _UI.ShowError(ex);
            }
        }

        private void FindByRegistration()
        {
            _UI.ShowMessage("Hittar fordon via registreringsnummer: ");
            string registrationNumber = _UI.GetStringInput("Ange registreringsnummer att söka efter: ", "Registreringsnummer kan inte vara tomt!");
            IVehicle? found = _Handler?.FindByRegistraation(registrationNumber);
            // TODO: behövs inte egen funktion i Handler, jag kan hitta med Search("registration=XYZ") ta bort?
            if (found is not null)
            {
                _UI.ShowMessage("-----------------------------------------------------------------");
                _UI.ShowMessage($"Fordon hittat: {found.Registration} {found.Make} {found.Model} {found.Color}");
                _UI.ShowMessage("-----------------------------------------------------------------");
            }
            else
            {
                _UI.ShowMessage("Inget fordon hittades med det angivna registreringsnumret.\n");
            }
        }

        private void Search()
        {
            _UI.ShowMessage("Söker fordon via egenskaper: ");
            string searchTerm = _UI.GetStringInput("Ange sökterm (ex: model=Tesla;color=red): ", "Sökterm kan inte vara tomt!");

            IEnumerable<IVehicle>? found = _Handler?.Search(searchTerm);
            if (found is not null)
            {
                _UI.ShowMessage("Alla Fordon hittat: \n");
                _UI.ShowMessage("-----------------------------------------------------------------");
                foreach (Vehicle vehicle in found)
                {
                    _UI.ShowMessage($" {vehicle.Registration} {vehicle.Make} {vehicle.Model} {vehicle.Color}");
                }
                _UI.ShowMessage("-----------------------------------------------------------------\n");
            }
            else
            {
                _UI.ShowMessage("Inget fordon hittades med detta sökning order.\n");
            }
        }

        private void Populate()
        {
            _UI.ShowMessage($"Populära {_GarageTitle} med en antal slumpmässigt fordon...");
            _UI.ShowMessage($"Vi garantera inte att det kommer inte uppreppa!");
            _UI.ShowMessage($"Garaget måste vara av typ `car´ eller ´motorcycle´.");

            int antalFordon = _UI.GetIntInput("\nHur många fordon vill du ha i garaget?  ", "Du måste mata en hel nummer!");

            try
            {
                bool result = _Handler?.Populate(antalFordon) ?? false;

                if (result)
                {
                    _UI.ShowMessage($"Garaget är populärade!");
                }
                else
                {
                    _UI.ShowMessage($"Antalet kan inte vara mer än Garages kapacitet!");
                }
            }
            catch (Exception ex)
            {
                _UI.ShowError(ex);
            }
        }

        public string ToFraction(float value, int maxDenominator = 10)
        {
            int numerator = (int)Math.Round(value * maxDenominator);
            int denominator = maxDenominator;

            int gcd = GCD(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;

            return $"{numerator}/{denominator}";
        }

        private int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return Math.Abs(a);
        }

        private void SaveData()
        {
            if (_GarageTitle != null)
            {
                _UI.ShowMessage($"Sparar {_GarageTitle} till JSON fil..");
                try
                {
                    _Handler?.SaveData(_GarageTitle);
                    _UI.ShowMessage("Klart.\n");
                }
                catch (Exception ex)
                {
                    _UI.ShowMessage($"Nått gick fel:  {ex.Message}\n");
                }

            }
        }

        private void LoadData()
        {
            if (_GarageTitle != null)
            {
                _UI.ShowMessage($"Laddar sparade JSON fil till {_GarageTitle}...");
                try
                {
                    _Handler?.LoadData(_GarageTitle);
                    _UI.ShowMessage("Klart.\n");
                }
                catch (Exception ex)
                {
                    _UI.ShowMessage($"Nått gick fel:  {ex.Message}\n");
                }
            }
        }

        private void SwitchActiveHandler()
        {
            _GarageTitle = null;
            _GarageType = null;
            _GarageCapacity = null;

           var switchGarageMenuOptions = new Dictionary<int, string>();

            _UI.ShowMessage("Vilken garage vill du arbeta med?\n");

            for (int i = 0; i < _HandlerList.Count; i++)
            {
                switchGarageMenuOptions.Add(i, _HandlerList[i].GarageName);
            }

            int choice = _UI.ShowSubMenu("Välja andra garage...", "Vilken garage vill du arbeta med ?\n", switchGarageMenuOptions);

            _GarageTitle = _HandlerList[choice].GarageName;
            _GarageType = _HandlerList[choice].GarageType.Name;
            _Handler = _HandlerList[choice];
        }

        private void Init()
        {
            _Config = ReadConfigFile();

            var mainMenuOptions = new Dictionary<int, Action>
            {
                { 1, CreateGarage },
                { 2, AddVehicle },
                { 3, ListVehicles },
                { 4, FindByRegistration },
                { 5, Search },
                { 6, Populate },
                { 7, SaveData },
                { 8, LoadData },
                { 9, RemoveVehicle },
                { 10, SwitchActiveHandler },
            };

            var mainMenuMessages = new Dictionary<int, string>
            {
                { 1, "Create Garage: man kan skappa mer en en Garage.\n Nytt tillägade garaage blir  'activa' omedelbart." },
                { 2, "Tilläg fordon" },
                { 3, "Lista alla fordon" },
                { 4, "Sök fordon via registreringsnummer" },
                { 5, "Sök fordon via egenskaper" },
                { 6, "Populära garaget med en antal fordon" },
                { 7, "Skriv ut Garage fordoner till fil" },
                { 8, "Laddar fordoner från fil" },
                { 9, "Ta bort fordon" },
                { 10, "Bytta activa garage" },
            };

            _UI = new ConsoleUI(mainMenuOptions, mainMenuMessages);

            if (_Config != null)
            {
                _GarageTitle = _Config.GarageTitle!;
                _GarageType = _Config.GarageType!;
                _GarageCapacity = _Config.GarageCapacity!;

                _Handler = _factory.Create(_GarageType ?? "", _GarageCapacity ?? 0, _GarageTitle ?? "IngetNamn");
                _HandlerList.Add(_Handler);
                _Config = null; // använd config bara på Init   
            }
            else
            {
                _UI.ShowMessage("Inget config hittades, vi måste sätta upp.");
                _UI.ShowMessage("------------------------------------------");
                CreateGarage();
            }
        }

        public void Run()
        {
            Init();
            _UI.ShowMainMenu(_GarageTitle ?? string.Empty);
        }

        private Config? ReadConfigFile()
        {
            ConfigRepository repo = new("config.json");
            return repo.LoadConfig();
        }
    }
}
