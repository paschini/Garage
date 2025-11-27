using Garage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.UILayer
{
    public class ConsoleUI : IUI
    {   
        public Dictionary<int, string> MainMenuOptions { get; }
        public Dictionary<int, Action> MainMenuActions { get; }

        public ConsoleUI(Dictionary<int, Action> options, Dictionary<int, string> messages)
        {
            MainMenuActions = options;
            MainMenuOptions = messages;

            MainMenuActions.Add(0, () => Environment.Exit(0));
            MainMenuOptions.Add(0, "Sluta Applikation");
        }

        public void ShowMainMenu(string title)
        {
            Console.Clear();

            while(true)
            {
                Console.WriteLine($"Välkommen till {title}");

                if (MainMenuActions.Count == 0 || MainMenuOptions.Count == 0 || MainMenuActions.Count != MainMenuOptions.Count)
                {
                    Console.WriteLine("Fel Meny");
                    return;
                }

                foreach (var action in MainMenuActions)
                { 
                    Console.WriteLine($"{action.Key}: {MainMenuOptions[action.Key]}");
                }

                Console.Write("Navigra vid att välja en alternativ: ");
                
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && MainMenuActions.ContainsKey(choice))
                {
                    MainMenuActions[choice].Invoke();
                }
                else
                {
                    Console.WriteLine($"Fel alternativ. Du måste välja en av {MainMenuActions.Keys}");
                }
            }
        }

        public void ShowSubMenu(string title, string subMenuTitle, Dictionary<int, Action> SubMenuActions, Dictionary<int, string> SubMenuOptions)
        {

            SubMenuActions.Add(0, () => ShowMainMenu(title));
            SubMenuOptions.Add(0, "Tillback till Main Meny");

            Console.Clear();
            Console.WriteLine(subMenuTitle);

            while (true)
            {
                if (SubMenuActions.Count == 0 || SubMenuOptions.Count == 0 || SubMenuActions.Count != SubMenuOptions.Count)
                {
                    Console.WriteLine("Fel Meny");
                    return;
                }

                foreach (var action in SubMenuActions)
                {
                    Console.WriteLine($"{action.Key}: {SubMenuOptions[action.Key]}");
                }

                Console.Write("Navigera vid att välja en anledning: ");

                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && SubMenuActions.ContainsKey(choice))
                {
                    SubMenuActions[choice].Invoke();
                }
                else
                {
                    Console.WriteLine($"Fel anledning. Du måste välja en av {SubMenuActions.Keys}");
                }
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowError(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        private T GetUserInput<T>(string message, string errorMessage, Func<string, (bool ok, T value)> parser)
        {
            Console.Write(message);
            var input = Console.ReadLine();
            var (ok, value) = parser(input!);

            while(!ok)
            {
                Console.WriteLine(errorMessage);
                input = Console.ReadLine();
                (ok, value) = parser(input!);
            }

            return value;
        }

        public int GetIntInput(string message, string errorMessage) => GetUserInput(message, errorMessage, input => (int.TryParse(input, out var v), v));

        public string GetStringInput(string message, string errorMessage) => GetUserInput(message, errorMessage, input => (!string.IsNullOrWhiteSpace(input), input!));

        public IVehicleInput? GetVehicleOfTypeInput(Type garageType)
        {
            IVehicleInput? vehicleToCreate = null;
            bool quit = false;
            while (!quit)
            {
                try
                {
                    switch (garageType.Name)
                    {
                        case "Car":
                            vehicleToCreate = AskCarParameters();
                            break;
                        case "Motorcycle":
                            vehicleToCreate = AskMotorcycleParameters();
                            break;
                        case "Bus":
                            vehicleToCreate = AskBusParameters();
                            break;
                        case "Boat":
                            vehicleToCreate = AskBoatParameters();
                            break;
                        case "Airplane":
                            vehicleToCreate = AskAirplaneParameters();
                            break;
                        default:
                            vehicleToCreate = AskCarParameters();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                }

                string choice = GetStringInput("\nLägga en till bil? Mata in q och sluta: ", "<Enter> eller q ");
                quit = choice.Equals("q");
            }

            return vehicleToCreate;
        }

        private VehicleInput.CarInputDTO AskCarParameters()
        {
            ShowMessage("\nLägger till en bil: ");
            string registrationNumber = GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = GetStringInput("Ange bilmärke: ", "Bilmärke kan inte vara tomt!");
            string model = GetStringInput("Ange bilmodell: ", "Bilmodell kan inte vara tomt!");
            string color = GetStringInput("Ange bilfärg: ", "Bilfärg kan inte vara tomt!");
            string trunkContent = GetStringInput("Ange innehåll i bagageutrymmet: ", "Innehåll i bagageutrymmet kan inte vara tomt!");

            return VehicleInput.CarInputDTO.Build(registrationNumber, make, model, color, trunkContent);
        }
        private VehicleInput.MotorcycleInputDTO AskMotorcycleParameters()
        {
            ShowMessage("\nLägger till en motorcykel: ");
            string registrationNumber = GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = GetStringInput("Ange motorcykelmärke: ", "Bilmärke kan inte vara tomt!");
            string model = GetStringInput("Ange motorcykelmodell: ", "Bilmodell kan inte vara tomt!");
            string color = GetStringInput("Ange motorcykellfärg: ", "Bilfärg kan inte vara tomt!");
            string utility = GetStringInput("Är det en utility motorcykel? (ja/nej): ", "Du måste ange 'ja' eller 'nej'!");

            while (utility != "ja" && utility != "nej")
            {
                GetStringInput("Är det en utility motorcykel? (ja/nej): ", "Du måste ange 'ja' eller 'nej'!");
            }

            bool isUtility = utility.Equals("ja");

            return VehicleInput.MotorcycleInputDTO.Build(registrationNumber, make, model, color, isUtility);
        }

        private VehicleInput.BusInputDTO AskBusParameters()
        {
            ShowMessage("\nLägger till en buss: ");
            string registrationNumber = GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = GetStringInput("Ange busmärke: ", "Bilmärke kan inte vara tomt!");
            string model = GetStringInput("Ange bussmodell: ", "Bilmodell kan inte vara tomt!");
            string color = GetStringInput("Ange bussfärg: ", "Bilfärg kan inte vara tomt!");
            string lineID = GetStringInput("Ange buss linje ID: ", "Linje ID kan inte vara tomt!");

            return VehicleInput.BusInputDTO.Build(registrationNumber, make, model, color, lineID);
        }

        private VehicleInput.BoatInputDTO AskBoatParameters()
        {
            ShowMessage("\nLägger till en båt: ");
            string registrationNumber = GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = GetStringInput("Ange båtmärke: ", "Bilmärke kan inte vara tomt!");
            string model = GetStringInput("Ange båtmodell: ", "Bilmodell kan inte vara tomt!");
            string color = GetStringInput("Ange båtfärg: ", "Bilfärg kan inte vara tomt!");
            string boatType = GetStringInput("Ange båttyp (segelbåt, motorbåt, katamaran): ", "Båttyp kan inte vara tomt!");

            while (boatType != "segelbåt" && boatType != "motorbåt" && boatType != "katamaran")
            {
                GetStringInput("Är det en utility motorcykel? (ja/nej): ", "Du måste ange 'segelbåt', 'motorbåt' eller 'katamaran'!");
            }

            return VehicleInput.BoatInputDTO.Build(registrationNumber, make, model, color, boatType);
        }

        private VehicleInput.AirplaneInputDTO AskAirplaneParameters()
        {
            ShowMessage("\nLägger till ett flygplan: ");
            string registrationNumber = GetStringInput("Ange registreringsnummer: ", "Registreringsnummer kan inte vara tomt!");
            string make = GetStringInput("Ange flygplanmärke: ", "Bilmärke kan inte vara tomt!");
            string model = GetStringInput("Ange flygplanmodell: ", "Bilmodell kan inte vara tomt!");
            string color = GetStringInput("Ange flygplanfärg: ", "Bilfärg kan inte vara tomt!");
            string wingSpanInput = GetStringInput("Ange vingbredd i meter: ", "Vingbredd kan inte vara tomt!");
            int engines = GetIntInput("Ange antal motorer: ", "Antal motorer måste vara ett positivt nummer!");

            double wingSpan;
            while (!double.TryParse(wingSpanInput, out wingSpan) || wingSpan <= 0)
            {
                wingSpanInput = GetStringInput("Ange vingbredd i meter (positivt nummer): ", "Vingbredd måste vara ett positivt nummer!");
            }

            return VehicleInput.AirplaneInputDTO.Build(registrationNumber, make, model, color, wingSpan, engines);
        }

        public Tuple<Dictionary<int, Action>, Dictionary<int, string>> CreateAddVehicleMenuOptions(string GarageTitle, float placesLeft, Type GarageType)
        {
            var addVehicleOptions = new Dictionary<int, Action>();
            var addVehicleMessages = new Dictionary<int, string>();

            if (GarageType == typeof(Vehicle))
            {
                if (placesLeft > 0.2f)
                {
                    addVehicleOptions.Add(0, () => GetVehicleOfTypeInput(GarageType));
                    addVehicleMessages.Add(0, "Lägg till motorcykel");
                }

                if (placesLeft >= 1)
                {
                    addVehicleOptions.Add(1, () => GetVehicleOfTypeInput(GarageType));
                    addVehicleMessages.Add(1, "Lägg till bil");
                }


                if (placesLeft >= 2)
                {
                    addVehicleOptions.Add(3, () => GetVehicleOfTypeInput(GarageType));
                    addVehicleOptions.Add(4, () => GetVehicleOfTypeInput(GarageType));
                    addVehicleMessages.Add(3, "Lägg till buss");
                    addVehicleMessages.Add(4, "Lägg till båt");
                }

                if (placesLeft >= 3)
                {
                    addVehicleOptions.Add(5, () => GetVehicleOfTypeInput(GarageType));
                    addVehicleMessages.Add(5, "Lägg till flygplan");
                }
            }

            if ((GarageType) == typeof(Motorcycle))
            {
                addVehicleOptions.Add(0, () => GetVehicleOfTypeInput(GarageType));
                addVehicleMessages.Add(0, "Lägg till motorcykel");
            }

            if (GarageType == typeof(Car))
            {
                addVehicleOptions.Add(0, () => GetVehicleOfTypeInput(GarageType));
                addVehicleMessages.Add(0, "Lägg till bil");
            }

            if (GarageType == typeof(Bus))
            {
                addVehicleOptions.Add(0, () => GetVehicleOfTypeInput(GarageType));
                addVehicleMessages.Add(0, "Lägg till Buss");
            }

            if (GarageType == typeof(Boat))
            {
                addVehicleOptions.Add(0, () => GetVehicleOfTypeInput(GarageType));
                addVehicleMessages.Add(0, "Lägg till båt");
            }

            if (GarageType == typeof(Airplane))
            {
                addVehicleOptions.Add(0, () => GetVehicleOfTypeInput(GarageType));
                addVehicleMessages.Add(0, "Lägg till bil");
            }

            return Tuple.Create(addVehicleOptions, addVehicleMessages);
        }
    }
}
