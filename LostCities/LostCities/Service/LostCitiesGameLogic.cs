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
        private HandViewModel _handSpielerEins, _handSpielerZwei;
        private AblagestapelViewModel _ablagestapel;
        private CardDeck _cardDeck;
        private bool _gameIsOver;
        private int _activePlayer = 0;

        public LostCitiesGameLogic(HandViewModel handSpielerEins, HandViewModel handSpielerZwei, AblagestapelViewModel ablagestapel)
        {
            _handSpielerEins = handSpielerEins;
            _handSpielerZwei = handSpielerZwei;
            _ablagestapel = ablagestapel;
            _cardDeck = new CardDeck();
            _gameIsOver = false;

            _handSpielerEins.GetHandCards(_cardDeck.GetXCards(3));
            _handSpielerZwei.GetHandCards(_cardDeck.GetXCards(3));


            _handSpielerEins.KarteAblegen += OnKarteAblegen;
            _handSpielerZwei.KarteAblegen += OnKarteAblegen;

            _ablagestapel.KarteAbheben += OnKarteAbheben;
        }

        void OnKarteAbheben(object sender, CardEventArgs e)
        {
            switch (_activePlayer)
            {
                case 0: //Spieler 1
                    _handSpielerEins.GetHandCard(e.Card);
                    _activePlayer = 1;
                    break;
                case 1:
                    _handSpielerZwei.GetHandCard(e.Card);
                    _activePlayer = 0;
                    break;
                default:
                    //Do nothing
                    break;
            }
            
            Debug.WriteLine(nameof(OnKarteAbheben) );
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
                switch (_activePlayer)
                {
                    case 0: //Spieler 1
                        _handSpielerEins.GetHandCard(_cardDeck.GetFirstCard());
                        _activePlayer = 1;
                        break;
                    case 1:
                        _handSpielerZwei.GetHandCard(_cardDeck.GetFirstCard());
                        _activePlayer = 0;
                        break;
                    default:
                        //Do nothing
                        break;
                }           
                IsGameOver();
            }
            Debug.WriteLine("OnKarteAblegen. GameIsOver:{0} " + "Active Player: {1}", _gameIsOver, _activePlayer);
        }

        private void IsGameOver()
        {
            _gameIsOver = _cardDeck.IsEmpty();
        }
    }
}
