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
        private HandViewModel _handSpielerEins, _handSpielerZwei;
        private DiscardPileViewModel _ablagestapel;
        private IStapel _anlegestapel, _anlegestapel2;
        private CardDeck _cardDeck;
        public CardDeck CardDeck {
            get
            {
                return _cardDeck;
            }
        }
        private bool _gameIsOver;
        private int _activePlayer = 0;

        public event EventHandler StatusChangedEvent;

        public PopupDialogViewModel PopupDialogViewModel { get; set; }
        public GameStatus GameStatus { get; private set; }

        public LostCitiesGameLogic(HandViewModel handSpielerEins, HandViewModel handSpielerZwei, DiscardPileViewModel ablagestapel, IStapel anlegestapel, IStapel anlegestapel2)
        {
            PopupDialogViewModel = new PopupDialogViewModel(null);

            _handSpielerEins = handSpielerEins;
            _handSpielerZwei = handSpielerZwei;
            _ablagestapel = ablagestapel;
            _anlegestapel = anlegestapel;
            _anlegestapel2 = anlegestapel2;
            _cardDeck = new CardDeck();
            _gameIsOver = false;

            _cardDeck.GetXCards(35); //Karten wegwerfen

            //Task.Run(async () => //Task.Run automatically unwraps nested Task types!
            //{
            //    Debug.WriteLine("Start");
            //    PopupDialogViewModel.Rotation = 90;
            //    await Task.Delay(5000);
            //    PopupDialogViewModel.Rotation = 180;
            //    Debug.WriteLine("Done");
            //});
            //Debug.WriteLine("All done");
        }

        protected virtual void StatusChanged(EventArgs e)
        {
            StatusChangedEvent?.Invoke(this, e);
        }

        private void GameStateMachine()
        {
            switch (GameStatus)
            {
                case GameStatus.Idle:
                    GameStatus = GameStatus.PlayerOnePlayCard;
                    break;
                case GameStatus.PlayerOnePlayCard:
                    //wenn CardDeck = empty => GameOver ansonsten KarteZiehen
                    GameStatus = GameStatus.PlayerOneDrawCard;
                    break;
                case GameStatus.PlayerOneDrawCard:
                    GameStatus = GameStatus.PlayerTwoPlayCard;
                    break;
                case GameStatus.PlayerTwoPlayCard:
                    //wenn CardDeck = empty => GameOver ansonsten KarteZiehen
                    GameStatus = GameStatus.PlayerTwoDrawCard;
                    break;
                case GameStatus.PlayerTwoDrawCard:
                    GameStatus = GameStatus.PlayerOnePlayCard;
                    break;
                case GameStatus.GameOver:
                default:
                    GameStatus = GameStatus.Idle;
                    break;
            }
            StatusChanged(null);
        }

        public void DrawHandCard()
        {
            GiveNewHandCard(_cardDeck.GetFirstCard());
        }

        public void OnKarteAbheben(object sender, CardEventArgs e)
        {
            GiveNewHandCard(e.Card);
            Debug.WriteLine(nameof(OnKarteAbheben));
        }

        public async void OnPlayCard(object sender, CardEventArgs e)
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
                        //await App.Current.MainPage.DisplayAlert("Das Spiel ist zu Ende", ShowWinnerWithPoints(), "Ok");
                        PopupDialogViewModel.CreatePopupDialog("Das Spiel ist zu Ende", ShowWinnerWithPoints(), null, null, "OK");
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
            //await App.Current.MainPage.DisplayAlert(AnweisungsLabelText, null , "Ok");

            _handSpielerEins.DisableHand();
            _handSpielerZwei.DisableHand();
            _ablagestapel.EnableDrawing();
            //KarteZiehenButtonIsEnabled = true; ToDo => Sobald im mainViewModel der GameState abgefragt werden kann, muss das übernommen werden! 
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

        public void InitGame()
        {
            GameStateMachine();  //Start game
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
            //KarteZiehenButtonIsEnabled = false; ToDo => Sobald im mainViewModel der GameState abgefragt werden kann, muss das übernommen werden!
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
