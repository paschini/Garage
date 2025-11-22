using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Garage
{
    internal class Manager
    {
        private IUI _UI = null!; // vet bara om UI
        private IHandler<Vehicle> _Handler = null!; // vet inte om UI, vet om Garage
        private string _GarageTitle { get; set; } = string.Empty;


        private void CreateGarage()
        {
            if (_Handler.GarageNotInitialised)
            {
                int capacity = _UI.GetIntInput("Hur många fordon kan garaget ta samtidigt? ", "Kapacitet i garaget måste vara en nummer: ");
                _Handler.CreateGarage(capacity);
            }
            else
            {
                _UI.ShowMessage($"\nGaraget är redan skapat. Är du säkert? {_GarageTitle} ska bortas");
                string input = _UI.GetStringInput("Skriv 'JA' för att bekräfta: ", "Du måste skriva 'JA' för att bekräfta.");
                if (input == "JA")
                {
                    // TODO: skappa RemoveGarage
                    //_Handler.RemoveGarage();
                    int capacity = _UI.GetIntInput("Hur många fordon kan garaget ta samtidigt? ", "Kapacitet i garaget måste vara en nummer: ");
                    _Handler.CreateGarage(capacity);
                }
            }
        }

        private void AddVehicle()
        {
            if (_Handler.GarageNotInitialised)
            {
                _UI.ShowMessage("\nGaraget är inte skapat än. Du måste skappa ett garage först.");
                return;
            }

            int capacity = _Handler.GetGarageCapacity();
            if (capacity <= 0)
            {
                _UI.ShowMessage("\nGaraget har ingen kapacitet. Du måste ta bort minst ett fordon först.");
                _UI.ShowMessage($"{_GarageTitle} kapacitet: {capacity}");
                return;
            }

            var addVehicleOptions = new Dictionary<int, Action>
            {
                // Skappa factory?
                //{ 1, () => _Handler.AddVehicle(VehicleFactory.CreateCar()) },
                //{ 2, () => _Handler.AddVehicle(VehicleFactory.CreateMotorcycle()) },
                //{ 3, () => _Handler.AddVehicle(VehicleFactory.CreateBus()) },
                //{ 4, () => _Handler.AddVehicle(VehicleFactory.CreateTruck()) }

                { 1, () => AddCar() },
                { 2, () => AddMotorcyckel() },
                { 3, () => AddBus() },
                { 4, () => AddBoat() },
                { 5, () => AddAirplane() },
            };
            var addVehicleMessages = new Dictionary<int, string>
            {
                { 1, "Lägg till bil" },
                { 2, "Lägg till motorcykel" },
                { 3, "Lägg till buss" },
                { 4, "Lägg till båt" },
                { 5, "Lägg till flyggplan" },
            };

            _UI.ShowSubMenu("Välj fordonstyp att lägga till:", addVehicleOptions, addVehicleMessages);
        }


        private void AddCar()
        {
            bool quit = false;
            while (!quit)
            {
                _UI.ShowMessage("\nLägger till en bil: ");
                string registrationNumber = _UI.GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
                string make = _UI.GetStringInput("Ange bilmärke: ", "Bilmärke kan inte vara tomt!");
                string model = _UI.GetStringInput("Ange bilmodell: ", "Bilmodell kan inte vara tomt!");
                string color = _UI.GetStringInput("Ange bilfärg: ", "Bilfärg kan inte vara tomt!");
                string trunkContent = _UI.GetStringInput("Ange innehåll i bagageutrymmet: ", "Innehåll i bagageutrymmet kan inte vara tomt!");

                var car = new Car(registrationNumber, make, model, color, trunkContent);
                _Handler.AddVehicle(car);

                string choice = _UI.GetStringInput("Lägga en till bil? Mata in q och sluta: ", "<Enter> eller q");
                quit = choice.Equals("q");
            }
        }

        private void AddMotorcyckel()
        {
            bool quit = false;
            while (!quit)
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

                var bike = new Motorcycle(registrationNumber, make, model, color, isUtility);
                _Handler.AddVehicle(bike);

                string choice = _UI.GetStringInput("Lägga en till motorcykel? Mata in q och sluta.: ", "<Enter> eller q");
                quit = choice.Equals("q");
            }
        }

        private void AddBus()
        {
            bool quit = false;
            while (!quit)
            {
                _UI.ShowMessage("\nLägger till en buss: ");
                string registrationNumber = _UI.GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
                string make = _UI.GetStringInput("Ange busmärke: ", "Bilmärke kan inte vara tomt!");
                string model = _UI.GetStringInput("Ange bussmodell: ", "Bilmodell kan inte vara tomt!");
                string color = _UI.GetStringInput("Ange bussfärg: ", "Bilfärg kan inte vara tomt!");
                string linjeID = _UI.GetStringInput("Ange buss linje ID: ", "Linje ID kan inte vara tomt!");

                var bus = new Bus(registrationNumber, make, model, color, linjeID);
                _Handler.AddVehicle(bus);

                string choice = _UI.GetStringInput("Lägga en till buss? Mata in q och sluta: ", "<Enter> eller q");
                quit = choice.Equals("q");
            }
        }

        private void AddBoat()
        {
            bool quit = false;
            while (!quit)
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

                var boat = new Boat(registrationNumber, make, model, color, boatType);
                _Handler.AddVehicle(boat);

                string choice = _UI.GetStringInput("Lägga en till båt? Mata in q och sluta: ", "<Enter> eller q");
                quit = choice.Equals("q");
            }
        }

        private void AddAirplane()
        {
            bool quit = false;
            while (!quit)
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

                var airplane = new Airplane(registrationNumber, make, model, color, wingSpan, engines);
                _Handler.AddVehicle(airplane);

                string choice = _UI.GetStringInput("Lägga en till flygplan? Mata in q och sluta: ", "<Enter> eller q");
                quit = choice.Equals("q");
            }
        }

        private void ListVehicles()
        {
            if (_Handler.GarageNotInitialised)
            {
                _UI.ShowMessage("\nGaraget är inte skapat än. Du måste skappa ett garage först.");
                return;
            }

            var vehicles = _Handler.GetAllVehicles();
            int vehicleCount = vehicles.Count();
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
                _UI.ShowMessage($"  {index}. {vehicle.Registration} {vehicle.Make} {vehicle.Model} {vehicle.Color}");
                // TODO: jag kan lägga till formatering som i PluralSight videon :)
            }
            _UI.ShowMessage("-----------------------------------------------------------------\n");
        }

        private void RemoveVehicle()
        {
            if (_Handler.GarageNotInitialised)
            {
                _UI.ShowMessage("\nGaraget är inte skapat än. Du måste skappa ett garage först.\n");
                return;
            }

            var vehicles = _Handler.GetAllVehicles();
            int vehicleCount = vehicles.Count();
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

            Vehicle removed = _Handler.RemoveVehicle(indexToRemove);
            _UI.ShowMessage($"Fordonet {removed.Registration} har tagits bort från garaget.\n");
        }

        private void FindByRegistration()
        {
            _UI.ShowMessage("Hittar fordon via registreringsnummer: ");
            string registrationNumber = _UI.GetStringInput("Ange registreringsnummer att söka efter: ", "Registreringsnummer kan inte vara tomt!");
            Vehicle? found = _Handler.FindByRegistraation(registrationNumber);
            // TODO: behövs inte egen funktion i Handler, jag kan hitta med Search("registration=XYZ") ta bort?
            if (found is not null)
            {
                _UI.ShowMessage($"Fordon hittat: {found.Registration} {found.Make} {found.Model} {found.Color}");
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

            IEnumerable<Vehicle>? found = _Handler.Search(searchTerm);
            if (found is not null)
            {
                _UI.ShowMessage("Alla Fordon hittat: \n");
                _UI.ShowMessage("-----------------------------------------------------------------");
                foreach (var vehicle in found)
                {
                    _UI.ShowMessage($" {vehicle.Registration} {vehicle.Make} {vehicle.Model} {vehicle.Color}");
                    // TODO: jag kan lägga till formatering som i PluralSight videon :)
                }
                _UI.ShowMessage("-----------------------------------------------------------------\n");
            }
            else
            {
                _UI.ShowMessage("Inget fordon hittades med detta sökning order.\n");
            }
        }

        private void Init()
        {
            var mainMenuOptions = new Dictionary<int, Action>
            {
                { 1, CreateGarage },
                { 2, AddVehicle },
                { 3, ListVehicles },
                { 4, FindByRegistration },
                { 5, Search },
                { 9, RemoveVehicle },
            };

            var mainMenuMessages = new Dictionary<int, string>
            {
                { 1, "Create Garage" },
                { 2, "Tilläg fordon" },
                { 3, "Lista alla fordon" },
                { 4, "Sök fordon via registreringsnummer" },
                { 5, "Sök fordon via egenskaper" },
                { 9, "Ta bort fordon" },
            };

            _UI = new ConsoleUI(_GarageTitle, mainMenuOptions, mainMenuMessages);

            //TDOD: kolla om garage finns sparat
            _UI.ShowMessage("Inget garage hittades, vi måste sätta upp.");

            // TODO: flytta till config fil
            _UI.ShowMessage("------------------------------------------");
            _GarageTitle = _UI.GetStringInput("Vad ska garaget heta? ", "Namn kan inte vara tomt!");
            _UI.SetTitle(_GarageTitle);

            _Handler = new GarageHandler();

            CreateGarage();
        }

        public void Run()
        {
            Init();
            _UI.ShowMainMenu();
        }
    }
}
