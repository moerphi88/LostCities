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
using System.Threading.Tasks;

namespace LostCities.Service
{
    public class LostCitiesGameLogic : INotifyPropertyChanged
    {
        public PopupDialogViewModel PopupDialogViewModel { get; set; }
        private HandViewModel _handSpielerEins, _handSpielerZwei;
        private DiscardPileViewModel _ablagestapel;
        private IStapel _anlegestapel, _anlegestapel2;
        private CardDeck _cardDeck;
        private bool _gameIsOver;
        private int _activePlayer = 0;
        private string _countCard = "";

        private const int HandCards = 8;
        private const string PlayerNeedsToPlayACard = "Spieler {0}. Bitte spiele eine Karte, indem Du eine Karte von deiner Hand anklickst.";
        private const string PlayerNeedsToDrawACard = "Spieler {0}. Bitte nimm eine Karte vom Nachziehstapel oder vom Ablagestapel.";

        private string _anweisungsText;
        private bool _karteZiehenButtonIsEnabeld;

        public string AnweisungsLabelText
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
        public string KarteZiehenButtonText { get; set; }
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
            PopupDialogViewModel = new PopupDialogViewModel(null);
            PopupDialogViewModel.TitleLabelText = "Test text";
            _handSpielerEins = handSpielerEins;
            _handSpielerZwei = handSpielerZwei;
            _ablagestapel = ablagestapel;
            _anlegestapel = anlegestapel;
            _anlegestapel2 = anlegestapel2;
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

            Task.Run(async () => //Task.Run automatically unwraps nested Task types!
            {
                Debug.WriteLine("Start");
                PopupDialogViewModel.Rotation = 90;
                await Task.Delay(5000);
                PopupDialogViewModel.Rotation = 180;
                Debug.WriteLine("Done");
            });
            Debug.WriteLine("All done");
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
                                        GetActiveAnlegestapel().KarteAnlegen(e.Card);
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

        private IStapel GetActiveAnlegestapel()
        {
            var anlegestapel = _activePlayer == 0 ? _anlegestapel : _anlegestapel2;
            return anlegestapel;
        }

        private String[] EvaluatePossibilities(Card card)
        {

            var topCard = GetActiveAnlegestapel().GetTopCards();

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
            AnweisungsLabelText = String.Format(PlayerNeedsToDrawACard, _activePlayer == 0 ? "1" : "2");
            //await App.Current.MainPage.DisplayAlert(AnweisungsLabelText, null , "Ok");

            _handSpielerEins.DisableHand();
            _handSpielerZwei.DisableHand();
            _ablagestapel.EnableDrawing();
            KarteZiehenButtonIsEnabled = true;
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
