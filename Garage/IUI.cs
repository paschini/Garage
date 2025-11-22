using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal interface IUI
    {
        string GarageTitle { get; }

        Dictionary<int, Action> MainMenuActions { get; }

        void ShowMainMenu();

        void ShowSubMenu(string title, Dictionary<int, Action> actions, Dictionary<int, string> options);

        void ShowMessage(string message);

        int GetIntInput(string prompt, string error);

        string GetStringInput(string prompt, string error);
    }
}
