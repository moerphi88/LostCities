using LostCities.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostCities.Model;

namespace LostCities.Service
{
    public class LostCitiesGameLogic
    {
        HandViewModel _handSpielerEins;
        AblagestapelViewModel _ablagestapel;

        public LostCitiesGameLogic(HandViewModel HandSpielerEins, AblagestapelViewModel Ablagestapel)
        {
            _handSpielerEins = HandSpielerEins;
            _ablagestapel = Ablagestapel;

            //HandSpielerEins.KarteAnlegen += OnKarteAnlegen;
            HandSpielerEins.KarteAblegen += OnKarteAblegen;
        }

        void OnKarteAnlegen(object sender, CardEventArgs e)
        {
            Debug.WriteLine("OnKarteAnlegen");
        }

        void OnKarteAblegen(object sender, CardEventArgs e)
        {
            _ablagestapel.KarteAblegen(e.Card);
            Debug.WriteLine("OnKarteAblegen");
        }

    }
}
