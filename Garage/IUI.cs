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

        
    }
}
