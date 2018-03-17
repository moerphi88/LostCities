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
        CardDeck _cardDeck;

        public LostCitiesGameLogic(HandViewModel HandSpielerEins, AblagestapelViewModel Ablagestapel)
        {
            _handSpielerEins = HandSpielerEins;
            _ablagestapel = Ablagestapel;
            _cardDeck = new CardDeck();

            _handSpielerEins.GetHandCards(_cardDeck.GetXCards(3));

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
