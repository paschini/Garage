using Garage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.UILayer
{
    internal interface IUI
    {
        Dictionary<int, Action> MainMenuActions { get; }

        void ShowMainMenu(string title);

        void ShowSubMenu(string title, string subTitle, Dictionary<int, Action> actions, Dictionary<int, string> options);

        void ShowMessage(string message);

        void ShowError(Exception ex);

        void ShowVehicleCreateInteractions();

        int GetIntInput(string prompt, string error);

        string GetStringInput(string prompt, string error);

        Tuple<Dictionary<int, Action>, Dictionary<int, string>>  CreateAddVehicleMenuOptions(string GarageTitle, float placesLeft, Type GarageType);

        IVehicleInput? GetVehicleOfTypeInput(Type garageType);
    }
}
