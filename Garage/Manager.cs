using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GarageSystem
{
    internal class Manager
    {
        private IUI _UI = null!; // vet bara om UI
        private IHandler _Handler = null!; // vet inte om UI, vet om Garage
        private string? _GarageTitle { get; set; } = null;
        private string? _GarageType { get; set; } = null;
        private int? _GarageCapacity = null;
        private Config? _Config { get; set; } = null;

        private List<IHandler> _HandlerList { get; set; } = [];

        private void CreateGarage()
        {
           if (_GarageTitle == null) // menar att vi skapar en andra ny garage
            {
                _GarageTitle = _UI.GetStringInput("Vad ska garaget heta? ", "Namn kan inte vara tomt!");
                _UI.ShowMessage("Garaget tillräckligt typer: car, motorcycle, airplane, bus, boat, vehicle.");
                _UI.ShowMessage("Vanliga garaget kan ta bara en typ av fordon.");
                _UI.ShowMessage("Garaget av vehicle typ är endast som kan ta olika fordon.");
                _GarageType = _UI.GetStringInput("Vilken typ av garage vill du ha? ", "Typ kan inte vara tomt!");
                _GarageCapacity = _UI.GetIntInput("Hur många platser till fordon finns i garaget? ", "Kapacitet i garaget måste vara en hel nummer: ");
            }

            try
            {
                if (_GarageType?.ToLower() == "car")
                {
                    Garage<Car> newGarage = new(_GarageCapacity ?? 0, _GarageTitle);
                    _Handler = new GarageHandler(newGarage);
                }

                if (_GarageType?.ToLower() == "motorcycle")
                {
                    Garage<Motorcycle> newGarage = new(_GarageCapacity ?? 0, _GarageTitle);
                    _Handler = new GarageHandler(newGarage);
                }

                if (_GarageType?.ToLower() == "bus")
                {
                    Garage<Bus> newGarage = new(_GarageCapacity ?? 0, _GarageTitle);
                    _Handler = new GarageHandler(newGarage);
                }

                if (_GarageType?.ToLower() == "boat")
                {
                    Garage<Bus> newGarage = new(_GarageCapacity ?? 0, _GarageTitle);
                    _Handler = new GarageHandler(newGarage);
                }

                if (_GarageType?.ToLower() == "airplane")
                {
                    Garage<Airplane> newGarage = new(_GarageCapacity ?? 0, _GarageTitle);
                    _Handler = new GarageHandler(newGarage);
                }

                if (_GarageType?.ToLower() == "vehicle")
                {
                    Garage<Vehicle> newGarage = new(_GarageCapacity ?? 0, _GarageTitle);
                    _Handler = new GarageHandler(newGarage);
                }

                _HandlerList.Add(_Handler);
            } 
            catch (Exception ex)
            {
                _UI.ShowMessage($"Nått gick fel: {ex.Message}");
            }
        }

        private void AddVehicle()
        {
            float placesLeft = _Handler.GetGaragePlacesLeft();
            _UI.ShowMessage($"{_GarageTitle} kan ta {_GarageType} ({_Handler.GarageType})");
            _UI.ShowMessage($"{_GarageTitle} har {ToFraction(placesLeft)} platser kvar.");

            if (!_Handler.GarageInitialised)
            {
                _UI.ShowMessage("\nGaraget är inte skapat än. Du måste skappa ett garage först.");
                return;
            }

            if (placesLeft <= 0.2f)
            {
                _UI.ShowMessage("\nGaraget har ingen plats kvar. Du måste ta bort minst ett fordon först.");
                return;
            }

            var addVehicleOptions = new Dictionary<int, Action>();
            var addVehicleMessages = new Dictionary<int, string>();

            if (_Handler.GarageType == typeof(Vehicle))
            {
                if (placesLeft > 0.2f)
                {
                    addVehicleOptions.Add(0, () => AddVehicleOfType<Motorcycle>(FactoryMotorcycle));
                    addVehicleMessages.Add(0, "Lägg till motorcykel");
                }

                if (placesLeft >= 1)
                {
                    addVehicleOptions.Add(1, () => AddVehicleOfType<Car>(FactoryCar));
                    addVehicleMessages.Add(1, "Lägg till bil");
                }


                if (placesLeft >= 2)
                {
                    addVehicleOptions.Add(3, () => AddVehicleOfType<Bus>(FactoryBus));
                    addVehicleOptions.Add(4, () => AddVehicleOfType<Boat>(FactoryBoat));
                    addVehicleMessages.Add(3, "Lägg till buss");
                    addVehicleMessages.Add(4, "Lägg till båt");
                }

                if (placesLeft >= 3)
                {
                    addVehicleOptions.Add(5, () => AddVehicleOfType<Airplane>(FactoryAirplane));
                    addVehicleMessages.Add(5, "Lägg till flygplan");
                }

                _UI.ShowSubMenu(_GarageTitle ?? string.Empty, "Välj fordonstyp att lägga till:", addVehicleOptions, addVehicleMessages);
            }

            if (_Handler.GarageType == typeof(Motorcycle))
            {
                addVehicleOptions.Add(0, () => AddVehicleOfType<Motorcycle>(FactoryMotorcycle));
                addVehicleMessages.Add(0, "Lägg till motorcykel");
            }

            if (_Handler.GarageType == typeof(Car))
            {
                addVehicleOptions.Add(0, () => AddVehicleOfType<Car>(FactoryCar));
                addVehicleMessages.Add(0, "Lägg till bil");
            }

            if (_Handler.GarageType == typeof(Bus))
            {
                addVehicleOptions.Add(0, () => AddVehicleOfType<Bus>(FactoryBus));
                addVehicleMessages.Add(0, "Lägg till Buss");
            }

            if (_Handler.GarageType == typeof(Boat))
            {
                addVehicleOptions.Add(0, () => AddVehicleOfType<Boat>(FactoryBoat));
                addVehicleMessages.Add(0, "Lägg till båt");
            }

            if (_Handler.GarageType == typeof(Airplane))
            {
                addVehicleOptions.Add(0, () => AddVehicleOfType<Airplane>(FactoryAirplane));
                addVehicleMessages.Add(0, "Lägg till bil");
            }
        }

        private void AddVehicleOfType<T>(Action factory) where T : Vehicle
        {
            // mindre upprepning med en lite Factory xD
            bool quit = false;
            while (!quit)
            {
                try
                {
                   factory();
                }
                catch (Exception e)
                {
                    _UI.ShowMessage($"Nått gick fel: {e.Message}");
                }

                _UI.ShowMessage($"Kapacitet nu: {_Handler.GetGarageCapacity()}");
                _UI.ShowMessage($"Platser kvar nu: {_Handler.GetGaragePlacesLeft()}");

                string choice = _UI.GetStringInput("\nLägga en till bil? Mata in q och sluta: ", "<Enter> eller q ");
                quit = choice.Equals("q");
            }
        }

        private void FactoryCar()
        {
            _UI.ShowMessage("\nLägger till en bil: ");
            string registrationNumber = _UI.GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = _UI.GetStringInput("Ange bilmärke: ", "Bilmärke kan inte vara tomt!");
            string model = _UI.GetStringInput("Ange bilmodell: ", "Bilmodell kan inte vara tomt!");
            string color = _UI.GetStringInput("Ange bilfärg: ", "Bilfärg kan inte vara tomt!");
            string trunkContent = _UI.GetStringInput("Ange innehåll i bagageutrymmet: ", "Innehåll i bagageutrymmet kan inte vara tomt!");
            Car newCar = new Car(registrationNumber, make, model, color, trunkContent);
            _Handler.AddVehicle(newCar);
        }
        private void FactoryMotorcycle()
        {
            _UI.ShowMessage("\nLägger till en motorcykel: ");
            string registrationNumber = _UI.GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = _UI.GetStringInput("Ange motorcykelmärke: ", "Bilmärke kan inte vara tomt!");
            string model = _UI.GetStringInput("Ange motorcykelmodell: ", "Bilmodell kan inte vara tomt!");
            string color = _UI.GetStringInput("Ange motorcykellfärg: ", "Bilfärg kan inte vara tomt!");
            string utility = _UI.GetStringInput("Är det en utility motorcykel? (ja/nej): ", "Du måste ange 'ja' eller 'nej'!");

            while (utility != "ja" && utility != "nej")
            {
                _UI.GetStringInput("Är det en utility motorcykel? (ja/nej): ", "Du måste ange 'ja' eller 'nej'!");
            }

            bool isUtility = utility.Equals("ja");

            Motorcycle newMoto = new Motorcycle(registrationNumber, make, model, color, isUtility);
            _Handler.AddVehicle(newMoto);
        }

        private void FactoryBus()
        {
            _UI.ShowMessage("\nLägger till en buss: ");
            string registrationNumber = _UI.GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = _UI.GetStringInput("Ange busmärke: ", "Bilmärke kan inte vara tomt!");
            string model = _UI.GetStringInput("Ange bussmodell: ", "Bilmodell kan inte vara tomt!");
            string color = _UI.GetStringInput("Ange bussfärg: ", "Bilfärg kan inte vara tomt!");
            string linjeID = _UI.GetStringInput("Ange buss linje ID: ", "Linje ID kan inte vara tomt!");

            Bus newBus = new Bus(registrationNumber, make, model, color, linjeID);
            _Handler.AddVehicle(newBus);
        }

        private void FactoryBoat()
        {
            _UI.ShowMessage("\nLägger till en båt: ");
            string registrationNumber = _UI.GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = _UI.GetStringInput("Ange båtmärke: ", "Bilmärke kan inte vara tomt!");
            string model = _UI.GetStringInput("Ange båtmodell: ", "Bilmodell kan inte vara tomt!");
            string color = _UI.GetStringInput("Ange båtfärg: ", "Bilfärg kan inte vara tomt!");
            string boatType = _UI.GetStringInput("Ange båttyp (segelbåt, motorbåt, katamaran): ", "Båttyp kan inte vara tomt!");

            while (boatType != "segelbåt" && boatType != "motorbåt" && boatType != "katamaran")
            {
                _UI.GetStringInput("Är det en utility motorcykel? (ja/nej): ", "Du måste ange 'segelbåt', 'motorbåt' eller 'katamaran'!");
            }

            Boat newBoat = new Boat(registrationNumber, make, model, color, boatType);
            _Handler.AddVehicle(newBoat);
        }

        private void FactoryAirplane()
        {
            _UI.ShowMessage("\nLägger till ett flygplan: ");
            string registrationNumber = _UI.GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = _UI.GetStringInput("Ange flygplanmärke: ", "Bilmärke kan inte vara tomt!");
            string model = _UI.GetStringInput("Ange flygplanmodell: ", "Bilmodell kan inte vara tomt!");
            string color = _UI.GetStringInput("Ange flygplanfärg: ", "Bilfärg kan inte vara tomt!");
            string wingSpanInput = _UI.GetStringInput("Ange vingbredd i meter: ", "Vingbredd kan inte vara tomt!");
            int engines = _UI.GetIntInput("Ange antal motorer: ", "Antal motorer måste vara ett positivt nummer!");

            double wingSpan;
            while (!double.TryParse(wingSpanInput, out wingSpan) || wingSpan <= 0)
            {
                wingSpanInput = _UI.GetStringInput("Ange vingbredd i meter (positivt nummer): ", "Vingbredd måste vara ett positivt nummer!");
            }

            Airplane newAirplane = new Airplane(registrationNumber, make, model, color, wingSpan, engines);
            _Handler.AddVehicle(newAirplane);
        }

        private void ListVehicles()
        {
            if (!_Handler.GarageInitialised)
            {
                _UI.ShowMessage("\nGaraget är inte skapat än. Du måste skappa ett garage först.");
                return;
            }

            var vehicles = _Handler.GetAllVehicles();
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
                var vehicle = _Handler.GetVehicle(index);
                try
                {
                    _UI.ShowMessage($"  {index}. {vehicle?.Registration} {vehicle?.Make} {vehicle?.Model} {vehicle?.Color}");
                }
                catch (Exception ex)
                {
                    _UI.ShowMessage($"Nått gick fel: {ex.Message}");
                }
            }
            _UI.ShowMessage("-----------------------------------------------------------------\n");
        }

        private void RemoveVehicle()
        {
            if (!_Handler.GarageInitialised)
            {
                _UI.ShowMessage("\nGaraget är inte skapat än. Du måste skappa ett garage först.\n");
                return;
            }

            var vehicles = _Handler.GetAllVehicles();
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
                IVehicle? removed = _Handler.RemoveVehicle(indexToRemove);
                _UI.ShowMessage($"Fordonet {removed?.Registration} har tagits bort från garaget.\n");
            }
            catch (Exception ex)
            {
                _UI.ShowMessage($"Nått gick fel: {ex.Message}");
            }
        }

        private void FindByRegistration()
        {
            _UI.ShowMessage("Hittar fordon via registreringsnummer: ");
            string registrationNumber = _UI.GetStringInput("Ange registreringsnummer att söka efter: ", "Registreringsnummer kan inte vara tomt!");
            IVehicle? found = _Handler.FindByRegistraation(registrationNumber);
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

            IEnumerable<IVehicle>? found = _Handler.Search(searchTerm);
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
                bool result = _Handler.Populate(antalFordon);

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
                _UI.ShowMessage($"Nått gick fel: {ex.Message}");
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
                    _Handler.SaveData(_GarageTitle);
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
                    _Handler.LoadData(_GarageTitle);
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

            _UI.ShowMessage("Vilken garage vill du arbeta med?\n");

            for (int i = 0; i < _HandlerList.Count; i++)
            {
                _UI.ShowMessage($"{i}.{_HandlerList[i].GarageName}");
            }

            int choice = _UI.GetIntInput("Ange nummer av garaget: ", "Du måste mata in en nummer.");

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

                _Config = null; // använd config bara på Init   
            }
            else
            {
                _UI.ShowMessage("Inget config hittades, vi måste sätta upp.");
                _UI.ShowMessage("------------------------------------------");
            }

            CreateGarage();
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
