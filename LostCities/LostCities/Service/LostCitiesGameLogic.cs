using LostCities.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LostCities.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace LostCities.Service
{
    public class LostCitiesGameLogic : INotifyPropertyChanged
    {
        private const string HandOneCardsKeyString = "hand_one_cards_key_string";
        private const string HandTwoCardsKeyString = "hand_two_cards_key_string";
        private const string AnlegeStapelOneCardsKeyString = "anlegestapel_one_cards_key_string";
        private const string AnlegeStapelTwoCardsKeyString = "anlegestapel_two_cards_key_string";
        private const int HandCards = 8;


        private HandViewModel _handPlayerOne, _handPlayerTwo;
        private DiscardPileViewModel _discardPile;
        private IStapel _anlegestapel, _anlegestapel2;
        private CardDeck _cardDeck;
        private bool _gameIsOver = false;
        private int _activePlayer = 0;

        private GameDataRepository _gameDataRepository;
        
        private bool _karteZiehenButtonIsEnabeld;
        private string _countCard = "";

        public bool KarteZiehenButtonIsEnabled
        {
            get
            {
                return _karteZiehenButtonIsEnabeld;
            }
            set
            {
                _karteZiehenButtonIsEnabeld = value;
                OnPropertyChanged();
            }
        }
        public string CountCard
        {
            get
            {
                return _countCard;
            }
            set
            {
                _countCard = value;
                if(int.Parse(_countCard) <= 5) OnPropertyChanged();
            }
        }

        public ICommand OnKarteZiehenButtonPressedCommand { get; }

        private void OnButtonPressed()
        {
            DrawHandCard();
            CountCard = _cardDeck.CountCard.ToString();
        }

        public LostCitiesGameLogic(HandViewModel handSpielerEins, HandViewModel handSpielerZwei, DiscardPileViewModel ablagestapel, IStapel anlegestapel, IStapel anlegestapel2)
        {
            _gameDataRepository = new GameDataRepository();

            _handPlayerOne = handSpielerEins;
            _handPlayerTwo = handSpielerZwei;
            _discardPile = ablagestapel;
            _anlegestapel = anlegestapel;
            _anlegestapel2 = anlegestapel2;

            _cardDeck = new CardDeck();

            OnKarteZiehenButtonPressedCommand = new Command(OnButtonPressed);

            if (_gameDataRepository.GetGameSaved() == true)
            {
                _anlegestapel.GetStapelCardsFromPersistency(AnlegeStapelOneCardsKeyString);
                _anlegestapel2.GetStapelCardsFromPersistency(AnlegeStapelTwoCardsKeyString);

                _handPlayerOne.GetHandCardsFromPersistency(HandOneCardsKeyString);
                _handPlayerTwo.GetHandCardsFromPersistency(HandTwoCardsKeyString);
            }
            else
            {
                _anlegestapel.PersistStapel(AnlegeStapelOneCardsKeyString);
                _anlegestapel2.PersistStapel(AnlegeStapelTwoCardsKeyString);

                _handPlayerOne.GetHandCards(_cardDeck.GetXCards(HandCards), HandOneCardsKeyString);
                _handPlayerTwo.GetHandCards(_cardDeck.GetXCards(HandCards), HandTwoCardsKeyString);
            }


            //To quickly end a game us this:
            _cardDeck.GetXCards(28); //Do 

            // Eventbinding
            //TODO die Events müssen noch wieder abgemeldet werden. Ggf. im Destruktor?!
            _handPlayerOne.PlayCard += OnPlayCard;
            _handPlayerTwo.PlayCard += OnPlayCard;

            _discardPile.KarteAbheben += OnKarteAbheben;

            InitGame();
        }

        public void DrawHandCard()
        {
            GiveNewHandCard(_cardDeck.GetFirstCard());
        }

        private void OnKarteAbheben(object sender, CardEventArgs e)
        {
            GiveNewHandCard(e.Card);
            Debug.WriteLine(nameof(OnKarteAbheben));
        }

        private async void OnPlayCard(object sender, CardEventArgs e)
        {
            try
            {
                if (!_gameIsOver)
                {
                    // GameLogik für MauMau
                    var buttons = EvaluatePossibilities(e.Card);

                    if (null != buttons)
                    {
                        //var buttons = new String[] { "Karte ablegen", "Karte anlegen" };
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
                                        _discardPile.KarteAblegen(e.Card);
                                        break;
                                    case "Karte anlegen":
                                        var stapel = GetActiveAnlegestapel();
                                        stapel.s.KarteAnlegen(e.Card);
                                        stapel.s.PersistStapel(stapel.key);
                                        break;
                                }
                                AnnounceNextStepDrawCard();
                            }
                            else
                            {
                                CancelCardTransfer(e.Card);
                            }
                        }
                        else
                        {
                            CancelCardTransfer(e.Card);
                        }
                    }
                    else
                    {
                        CancelCardTransfer(e.Card);
                    }
                    IsGameOver();
                    if (_gameIsOver)
                    {
                        await App.Current.MainPage.DisplayAlert("Das Spiel ist zu Ende", ShowWinnerWithPoints(), "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LostCitiesGameLogic. OnKarteAblegen. " + ex.Message);
            }
            Debug.WriteLine("OnKarteAblegen. GameIsOver:{0} " + "Active Player: {1}", _gameIsOver, _activePlayer);
        }

        private String ShowWinnerWithPoints()
        {
            var pointsPlayer1 = _anlegestapel.CountPoints();
            var pointsPlayer2 = _anlegestapel2.CountPoints();
            if (pointsPlayer1 > pointsPlayer2)
            {
                return $"Herzlich Glückwunsch, Spieler 1 du hast gewonnen mit {pointsPlayer1} Punkten. Spieler 2 hat {pointsPlayer2} Punkte. Bitte startet die App neu, um ein neues Spiel zu beginnen";
            } else if (pointsPlayer1 < pointsPlayer2)
            {
                return $"Herzlich Glückwunsch, Spieler 2 du hast gewonnen mit {pointsPlayer2} Punkten. Spieler 1 hat {pointsPlayer1} Punkte. Bitte startet die App neu, um ein neues Spiel zu beginnen ";
            } else
            {
                return $"Unentschieden. Herzlich Glückwunsch. Spieler 1 hat {pointsPlayer1} Punkte und Spieler 2 {pointsPlayer2} Punkte. Bitte startet die App neu, um ein neues Spiel zu beginnen";
            }
        }

        private (IStapel s,string key) GetActiveAnlegestapel()
        {
            var anlegestapel = _activePlayer == 0 ? _anlegestapel : _anlegestapel2;
            var key = _activePlayer == 0 ? AnlegeStapelOneCardsKeyString : AnlegeStapelTwoCardsKeyString;
            return (anlegestapel,key);
        }

        private String[] EvaluatePossibilities(Card card)
        {

            var topCard = GetActiveAnlegestapel().s.GetTopCards();

            //Wenn es mindestens eine angelegte Karte gibt,...
            if(topCard.Count != 0)
            {
                foreach (var c in topCard)
                {
                    //Liegt auf dem Farbstapel der ausgewählten Karte bereits eine Karte?
                    if(card.Name == c.Name)
                    {
                        //Wenn ja, ist die ausgewählte Karte höher oder gleich (gleich bei mehreren Händen) als die die schon liegt
                        if(card.Zahl >= c.Zahl)
                        {
                            return new String[] { "Karte ablegen", "Karte anlegen" }; //Ja, dann kann der Nutzer frei entscheiden
                        }
                        return new String[] { "Karte ablegen" }; //Wenn nicht, dann kann er die Karte nur ablegen
                    }                    
                }
                return new String[] { "Karte ablegen", "Karte anlegen" };//Es liegt noch keine Karte der ausgewählten Farbe auf dem Anlegestapel
            }
            else { //Spieler kann alles legen, weil noch keine Karte angelegt wurde
                return new String[] { "Karte ablegen", "Karte anlegen" };
            }
        }

        private void AnnounceNextStepDrawCard()
        {
            _handPlayerOne.DisableHand();
            _handPlayerTwo.DisableHand();
            _discardPile.EnableDrawing();
            KarteZiehenButtonIsEnabled = true;
        }

        private void GiveNewHandCard(Card card)
        {
            switch (_activePlayer)
            {
                case 0: //Spieler 1
                    _handPlayerOne.GetHandCard(card, HandOneCardsKeyString);
                    SwitchActivePlayer();
                    break;
                case 1:
                    _handPlayerTwo.GetHandCard(card, HandTwoCardsKeyString);
                    SwitchActivePlayer();
                    break;
            }
        }

        private void CancelCardTransfer(Card card)
        {
            switch (_activePlayer)
            {
                case 0: //Spieler 1
                    _handPlayerOne.GetHandCard(card, HandOneCardsKeyString);
                    break;
                case 1:
                    _handPlayerTwo.GetHandCard(card, HandTwoCardsKeyString);
                    break;
            }
        }

        private void InitGame()
        {
            _handPlayerOne.EnableHand();
            _handPlayerTwo.DisableHand();
            _discardPile.DisableDrawing();
            KarteZiehenButtonIsEnabled = false;
            _gameDataRepository.SetGameSaved(true);
        }

        private void SwitchActivePlayer()
        {
            _activePlayer = _activePlayer == 0 ? 1 : 0;
            switch (_activePlayer)
            {
                case 0:
                    _handPlayerOne.EnableHand();
                    _handPlayerTwo.DisableHand();
                    break;
                case 1:
                    _handPlayerOne.DisableHand();
                    _handPlayerTwo.EnableHand();
                    break;
            }
            _discardPile.DisableDrawing();
            KarteZiehenButtonIsEnabled = false;
        }

        private void IsGameOver()
        {
            _gameIsOver = _cardDeck.IsEmpty();
            if(_gameIsOver == true) _gameDataRepository.SetGameSaved(false);
        }

        #region INotifyPropertyChanges Handler

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
