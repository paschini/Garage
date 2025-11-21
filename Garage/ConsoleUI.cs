using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class ConsoleUI : IUI
    {   
        public string GarageTitle { get; private set; } = "No Title";

        public Dictionary<int, string> MainMenuOptions { get; }
        public Dictionary<int, Action> MainMenuActions { get; }


       

        public ConsoleUI(string title, Dictionary<int, Action> options, Dictionary<int, string> messages)
        {
            GarageTitle = title;
            MainMenuActions = options;
            MainMenuOptions = messages;

            MainMenuActions.Add(0, () => Environment.Exit(0));
            MainMenuOptions.Add(0, "Sluta Applikation");
        }

        public void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine(GarageTitle);

            while(true)
            {
                if (MainMenuActions.Count == 0 || MainMenuOptions.Count == 0 || MainMenuActions.Count != MainMenuOptions.Count)
                {
                    Console.WriteLine("Fel Meny");
                    return;
                }

                foreach (var action in MainMenuActions)
                { 
                    Console.WriteLine($"{action.Key}: {MainMenuOptions[action.Key]}");
                }

                Console.Write("Navigaate by choosing an option: ");
                
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && MainMenuActions.ContainsKey(choice))
                {
                    MainMenuActions[choice].Invoke();
                }
                else
                {
                    Console.WriteLine($"Invalid option. You must choose one of {MainMenuActions.Keys}");
                }
            }
        }

        public void ShowSubMenu(string subMenuTitle, Dictionary<int, Action> SubMenuActions, Dictionary<int, string> SubMenuOptions)
        {
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
    }
}

//static void Swap<T>(ref T lhs, ref T rhs)