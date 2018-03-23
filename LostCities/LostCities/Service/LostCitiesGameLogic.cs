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
        private IStapel _anlegestapel;
        private CardDeck _cardDeck;
        private bool _gameIsOver;
        private int _activePlayer = 0;

        public LostCitiesGameLogic(HandViewModel handSpielerEins, HandViewModel handSpielerZwei, AblagestapelViewModel ablagestapel, IStapel anlegestapel)
        {
            _handSpielerEins = handSpielerEins;
            _handSpielerZwei = handSpielerZwei;
            _ablagestapel = ablagestapel;
            _anlegestapel = anlegestapel;
            _cardDeck = new CardDeck();
            _gameIsOver = false;

            _handSpielerEins.GetHandCards(_cardDeck.GetXCards(3));
            _handSpielerZwei.GetHandCards(_cardDeck.GetXCards(3));

            _handSpielerEins.PlayCard += OnPlayCard;
            _handSpielerZwei.PlayCard += OnPlayCard;

            _ablagestapel.KarteAbheben += OnKarteAbheben;
        }

        void OnKarteAbheben(object sender, CardEventArgs e)
        {
            GiveNewHandCard(e.Card);
            Debug.WriteLine(nameof(OnKarteAbheben));
        }

        async void OnPlayCard(object sender, CardEventArgs e)
        {
            try
            {
                if (!_gameIsOver)
                {
                    var buttons = new String[] { "Karte ablegen", "Karte anlegen" };
                    //var buttons = new String[] { "Karte anlegen" };
                    var spieler = _activePlayer == 0 ? "Eins" : "Zwei";
                    var text = "Spieler " + spieler + " ist am Zug.";
                    var answer = await App.Current.MainPage.DisplayActionSheet(text, null, "Cancel", buttons);

                    if (null != answer)
                    {
                        if (answer != "Cancel")
                        {
                            switch (answer)
                            {
                                case "Karte ablegen":
                                    _ablagestapel.KarteAblegen(e.Card);
                                    break;
                                case "Karte anlegen":
                                    _anlegestapel.KarteAnlegen(e.Card);
                                    break;
                            }
                            AnnounceNextStepDrawCard();
                            //GiveNewHandCard();
                        } else
                        {
                            CancelCardTransfer(e.Card);
                        }
                    }                    
                    IsGameOver();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LostCitiesGameLogic. ObKarteAblegen. " + ex.Message);
            }
            Debug.WriteLine("OnKarteAblegen. GameIsOver:{0} " + "Active Player: {1}", _gameIsOver, _activePlayer);
        }

        async private void AnnounceNextStepDrawCard()
        {
            await App.Current.MainPage.DisplayAlert("Ziehe jetzt eine neue Karte", null , "Cancel");
            _handSpielerEins.DisableHand();
            _handSpielerZwei.DisableHand();
            _ablagestapel.EnableDrawing();
        }

        public void DrawHandCard()
        {
            GiveNewHandCard(_cardDeck.GetFirstCard());
        }

        private void GiveNewHandCard(Card card)
        {
            switch (_activePlayer)
            {
                case 0: //Spieler 1
                    _handSpielerEins.GetHandCard(card);
                    SwitchActivePlayer();
                    break;
                case 1:
                    _handSpielerZwei.GetHandCard(card);
                    SwitchActivePlayer();
                    break;
            }
        }

        private void CancelCardTransfer(Card card)
        {
            switch (_activePlayer)
            {
                case 0: //Spieler 1
                    _handSpielerEins.GetHandCard(card);
                    break;
                case 1:
                    _handSpielerZwei.GetHandCard(card);
                    break;
            }
        }

        private void SwitchActivePlayer()
        {
            _activePlayer = _activePlayer == 0 ? 1 : 0;
            switch (_activePlayer)
            {
                case 0:
                    _handSpielerEins.EnableHand();
                    _handSpielerZwei.DisableHand();
                    break;
                case 1:
                    _handSpielerEins.DisableHand();
                    _handSpielerZwei.EnableHand();
                    break;
            }
            _ablagestapel.DisableDrawing();
        }

        private void IsGameOver()
        {
            _gameIsOver = _cardDeck.IsEmpty();
        }
    }
}
