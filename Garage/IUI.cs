using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageSystem
{
    internal interface IUI
    {
        Dictionary<int, Action> MainMenuActions { get; }

        void ShowMainMenu(string title);

        void ShowSubMenu(string title, string subTitle, Dictionary<int, Action> actions, Dictionary<int, string> options);

        void ShowMessage(string message);

        int GetIntInput(string prompt, string error);

        string GetStringInput(string prompt, string error);
    }
}
