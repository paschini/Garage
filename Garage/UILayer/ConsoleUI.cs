using Garage.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Garage.UILayer.VehicleInput;

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
            if (MainMenuActions.Count != MainMenuOptions.Count)
            {
                throw new ArgumentException($"Argument count doesnt match.");
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Välkommen till {title}\n\n");

                if (MainMenuActions.Count == 0 || MainMenuOptions.Count == 0 || MainMenuActions.Count != MainMenuOptions.Count)
                {
                    Console.WriteLine("Fel Meny");
                    return;
                }

                foreach (var action in MainMenuActions)
                { 
                    Console.WriteLine($"{action.Key}: {MainMenuOptions[action.Key]}");
                }

                Console.Write("\nNavigera vid att välja en alternativ: ");
                
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && MainMenuActions.ContainsKey(choice))
                {
                    MainMenuActions[choice].Invoke();
                }
                else
                {
                    Console.WriteLine($"\nFel alternativ. Du måste välja en av {MainMenuActions.Keys}");
                    Console.ReadKey();
                }

                Console.ReadKey(); // Visa medelander inan att rensa ut
            }
        }

        public int ShowSubMenu(string title, string subMenuTitle, Dictionary<int, string> SubMenuOptions)
        {
            SubMenuOptions.Add(0, "Tillbaka till Main Meny");

            while(true)
            {
                Console.Clear();
                Console.WriteLine(subMenuTitle);

                foreach (var action in SubMenuOptions)
                {
                    Console.WriteLine($"{action.Key}: {SubMenuOptions[action.Key]}");
                }

                Console.Write("\nNavigera vid att välja en alternativ: ");

                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && SubMenuOptions.ContainsKey(choice))
                {
                    if (choice == 0) ShowMainMenu(title);
                    return choice;
                }
                else
                {
                    Console.WriteLine($"Fel alternativ. Du måste välja en av {SubMenuOptions.Keys}");
                }
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowError(Exception ex)
        {
            Console.WriteLine($"Nått gick fel: {ex.Message}");
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

        public IVehicleInput? GetInputForVehicleOfType(Type vehicleType)
        {
            return vehicleType.Name switch
            {
                "Car" => AskCarParameters(),
                "Motorcycle" => AskMotorcycleParameters(),
                "Bus" => AskBusParameters(),
                "Boat" => AskBoatParameters(),
                "Airplane" => AskAirplaneParameters(),

                _ => throw new InvalidOperationException("Valid argumet values aare 1 thru 5")
            };
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

        public Dictionary<int, string> CreateAddVehicleMenuOptions(string GarageTitle, float placesLeft, Type GarageType)
        {
           var addVehicleMessages = new Dictionary<int, string>();

            switch (GarageType.Name)
            {
                case "Motorcycle":
                    addVehicleMessages.Add(1, "Motorcykel");
                    break;
                case "Car":
                    addVehicleMessages.Add(2, "Bil");
                    break;
                case "Bus":
                    addVehicleMessages.Add(3, "Buss");
                    break;
                case "Boat":
                    addVehicleMessages.Add(4, "Båt");
                    break;
                case "Airplane":
                    addVehicleMessages.Add(5, "Flyggplan");
                    break;
                case "Vehicle":
                    if (placesLeft > 0.2f)
                    {
                        addVehicleMessages.Add(1, "Motorcykel");
                    }

                    if (placesLeft >= 1)
                    {
                        addVehicleMessages.Add(2, "Bil");
                    }


                    if (placesLeft >= 2)
                    {
                        addVehicleMessages.Add(3, "Buss");
                        addVehicleMessages.Add(4, "Båt");
                    }

                    if (placesLeft >= 3)
                    {
                        addVehicleMessages.Add(5, "Flygplan");
                    }
                    break;
                
            }

            return addVehicleMessages;
        }
    }
}
