using LostCities.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostCities.Service
{
    public class LostCitiesGameLogic
    {
        public LostCitiesGameLogic(HandViewModel HandViewModelEins, HandViewModel HandVieWModelZwei)
        {
            HandViewModelEins.CountdownCompleted += OnContdownCompleted;
        }

        void OnContdownCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("Es scheint geklappt zu haben");
        }
    }
}
