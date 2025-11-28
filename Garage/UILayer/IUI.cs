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

        int ShowSubMenu(string title, string subTitle, Dictionary<int, string> options);

        void ShowMessage(string message);

        void ShowError(Exception ex);

        int GetIntInput(string prompt, string error);

        string GetStringInput(string prompt, string error);

        Dictionary<int, string>  CreateAddVehicleMenuOptions(string GarageTitle, float placesLeft, Type GarageType);

        IVehicleInput? GetInputForVehicleOfType(Type vehicleType);
    }
}
