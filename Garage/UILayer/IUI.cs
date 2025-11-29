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

        /// <summary>
        /// Asks the user for all parameters necessary to create a vehicle of IVehicle vehicleType.
        /// </summary>
        /// <param name="vehicleType">Must be a type that implements IVehicle</param>
        /// <returns>
        /// Returns the IVehicleInput DTO maching IVehicle vehicleType generatred from user input.
        /// Ex. 'CarInputDTO carDTO = GetInputForVehicleOfType(typeof(Car));'
        /// </returns>
        IVehicleInput? GetInputForVehicleOfType(Type vehicleType);
    }
}
