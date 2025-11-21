using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal class Manager
    {
        private ConsoleUI _UI = null!; // vet bara om UI
        private Handler _Handler = null!; // vet inte om UI, vet om Garage
        private string _GarageTitle { get; set; } = string.Empty;


        private void Init()
        {
            var mainMenuOptions = new Dictionary<int, Action>
            {
                { 1, () => Console.WriteLine("option 1") }
                //{ 1, _Handler.AddVehicle },
                //{ 2, _Handler.RemoveVehicle },
                //{ 2, _Handler.ListVehicles }
            };

            var mainMenuMessages = new Dictionary<int, string>
            {
                { 1, "Lägg till fordon" },
                { 2, "Ta bort fordon" },
                { 3, "Lista fordon" }
            };

            _UI = new ConsoleUI(_GarageTitle, mainMenuOptions, mainMenuMessages);

            //TDOD: kolla om garage finns sparat
            _UI.ShowMessage("Inget garage hittades, vi måste sätta upp.");

            // TODO: flytta till config fil
            _UI.ShowMessage("------------------------------------------");
            _GarageTitle = _UI.GetStringInput("Vad ska garaget heta?", "Namn kan inte vara tomt!");
            
            _Handler = new Handler();

            if (_Handler.GarageNotInitialised)
            {
                _UI.ShowMessage($"Du måste skappa ett nytt garage.");
                int capacity = _UI.GetIntInput("Hur många fordon kan garaget ta samtidigt? ", "Kapacitet i garaget måste vara en nummer: ");
                _Handler.CreateGarage(capacity);
            }
        }

        public void Run()
        {
            Init();
            _UI.ShowMainMenu();
        }
    }
}
