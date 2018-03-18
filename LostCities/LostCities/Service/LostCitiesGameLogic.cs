using LostCities.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LostCities.Model;

namespace LostCities.Service
{
    public class LostCitiesGameLogic
    {
        private HandViewModel _handSpielerEins;
        private AblagestapelViewModel _ablagestapel;
        private CardDeck _cardDeck;
        private bool _gameIsOver;

        public LostCitiesGameLogic(HandViewModel HandSpielerEins, AblagestapelViewModel Ablagestapel)
        {
            _handSpielerEins = HandSpielerEins;
            _ablagestapel = Ablagestapel;
            _cardDeck = new CardDeck();
            _gameIsOver = false;

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
            if (!_gameIsOver)
            {
                _ablagestapel.KarteAblegen(e.Card);
                _handSpielerEins.GetHandCard(_cardDeck.GetFirstCard());
                IsGameOver();
            }
            Debug.WriteLine("OnKarteAblegen. GameIsOver:{0}",_gameIsOver);
        }

        private void IsGameOver()
        {
            _gameIsOver = _cardDeck.IsEmpty();
        }
    }
}
