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
        private HandViewModel _handSpielerEins, _handSpielerZwei;
        private AblagestapelViewModel _ablagestapel;
        private IStapel _anlegestapel;
        private CardDeck _cardDeck;
        private bool _gameIsOver;
        private int _activePlayer = 0;

        private String _anweisungsText;
        private bool _karteZiehenButtonIsEnabeld;

        public String AnweisungsLabelText
        {
            get
            {
                return _anweisungsText;
            }
            set
            {
                _anweisungsText = value;
                OnPropertyChanged();
            }
        }
        public String KarteZiehenButtonText { get; set; }

        public ICommand OnKarteZiehenButtonPressedCommand { get; }

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

        private void OnButtonPressed()
        {
            DrawHandCard();
        }

        private readonly int HandCards = 5;
        private const string PlayerNeedsToPlayACard = "Spieler {0} ist am Zug. Bitte lege eine Karte ab oder an, indem Du eine Karte von deiner Hand anklickst und den Dialog bestätigst";
        private const string PlayerNeedsToDrawACard = "Spieler {0} ist am Zug. Bitte nimm eine Karte vom Nachziehstapel oder vom Ablagestapel, wenn dort bereits eine Karte abgelegt wurde";

        public LostCitiesGameLogic(HandViewModel handSpielerEins, HandViewModel handSpielerZwei, AblagestapelViewModel ablagestapel, IStapel anlegestapel)
        {
            _handSpielerEins = handSpielerEins;
            _handSpielerZwei = handSpielerZwei;
            _ablagestapel = ablagestapel;
            _anlegestapel = anlegestapel;
            _cardDeck = new CardDeck();
            _gameIsOver = false;

            AnweisungsLabelText = String.Format(PlayerNeedsToPlayACard, _activePlayer == 0 ? "1" : "2");
            KarteZiehenButtonText = "Karte ziehen Binding";
            OnKarteZiehenButtonPressedCommand = new Command(OnButtonPressed);
                                  
            _handSpielerEins.GetHandCards(_cardDeck.GetXCards(HandCards));
            _handSpielerZwei.GetHandCards(_cardDeck.GetXCards(HandCards));

            // Eventbinding
            //TODO die Events müssen noch wieder abgemeldet werden. Ggf. im Destruktor?!
            _handSpielerEins.PlayCard += OnPlayCard;
            _handSpielerZwei.PlayCard += OnPlayCard;

            _ablagestapel.KarteAbheben += OnKarteAbheben;

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
                                        _ablagestapel.KarteAblegen(e.Card);
                                        break;
                                    case "Karte anlegen":
                                        _anlegestapel.KarteAnlegen(e.Card);
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
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LostCitiesGameLogic. OnKarteAblegen. " + ex.Message);
            }
            Debug.WriteLine("OnKarteAblegen. GameIsOver:{0} " + "Active Player: {1}", _gameIsOver, _activePlayer);
        }

        private String[] EvaluatePossibilities(Card card)
        {
            var topCard = _anlegestapel.GetTopCards();

            //Wenn es mindestens eine angelegte Karte gibt,...
            if(topCard.Count != 0)
            {
                foreach (var c in topCard)
                {
                    //Liegt auf dem Farbstapel der ausgewählten Karte bereits eine Karte?
                    if(card.Name == c.Name)
                    {
                        //Wenn ja, ist die ausgewählte Karte höher als die die schon liegt
                        if(card.Zahl > c.Zahl)
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
            //await App.Current.MainPage.DisplayAlert("Ziehe jetzt eine neue Karte", null , "Cancel");
            _handSpielerEins.DisableHand();
            _handSpielerZwei.DisableHand();
            _ablagestapel.EnableDrawing();
            KarteZiehenButtonIsEnabled = true;
            AnweisungsLabelText = String.Format(PlayerNeedsToDrawACard, _activePlayer == 0 ? "1" : "2");
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

        private void InitGame()
        {
            _handSpielerEins.EnableHand();
            _handSpielerZwei.DisableHand();
            _ablagestapel.DisableDrawing();
            KarteZiehenButtonIsEnabled = false;
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
            KarteZiehenButtonIsEnabled = false;
            AnweisungsLabelText = String.Format(PlayerNeedsToPlayACard, _activePlayer == 0 ? "1" : "2");
        }

        private void IsGameOver()
        {
            _gameIsOver = _cardDeck.IsEmpty();
        }

        #region INotifyPropertyChanges Handler

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
