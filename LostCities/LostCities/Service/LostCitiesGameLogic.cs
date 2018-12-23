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
    public class LostCitiesGameLogic
    {        
        private IStapel _anlegestapel, _anlegestapel2;
        public CardDeck CardDeck { get; private set; }
        public GameStatus GameStatus { get; private set; }

        public event EventHandler StatusChangedEvent;
        protected virtual void StatusChanged(EventArgs e)
        {
            StatusChangedEvent?.Invoke(this, e);
        }

        public LostCitiesGameLogic(DiscardPileViewModel ablagestapel, IStapel anlegestapel, IStapel anlegestapel2)
        {           
            _anlegestapel = anlegestapel;
            _anlegestapel2 = anlegestapel2;
            CardDeck = new CardDeck();

            CardDeck.GetXCards(35); //Karten wegwerfen
        }

        public void InitGame()
        {
            GameStateMachine();  //Start game
        }
        
        public void GameStateMachine()
        {
            switch (GameStatus)
            {
                case GameStatus.Idle:
                    GameStatus = GameStatus.PlayerOnePlayCard;
                    break;
                case GameStatus.PlayerOnePlayCard:
                    GameStatus = CardDeck.IsEmpty() ? GameStatus.GameOver : GameStatus.PlayerOneDrawCard;
                    break;
                case GameStatus.PlayerOneDrawCard:
                    GameStatus = GameStatus.PlayerTwoPlayCard;
                    break;
                case GameStatus.PlayerTwoPlayCard:
                    GameStatus = CardDeck.IsEmpty() ? GameStatus.GameOver : GameStatus.PlayerTwoDrawCard;
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

        public Card GetNewHandCard()
        {
            return CardDeck.GetFirstCard();
        }

        //ToDO Diese Methode/Logik in die Anlegestapel verlagern
        public String ShowWinnerWithPoints()
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

        //ToDO Diese Methode/Logik in die Anlegestapel verlagern
        public bool IsAnlegenPossible(Card card)
        {
            var topCard = GetActiveAnlegestapel().GetTopCards();

            //Wenn es mindestens eine angelegte Karte gibt,...
            if (topCard.Count != 0)
            {
                foreach (var c in topCard)
                {
                    //Liegt auf dem Farbstapel der ausgewählten Karte bereits eine Karte?
                    if (card.Name == c.Name)
                    {
                        //Wenn ja, ist die ausgewählte Karte höher oder gleich (gleich bei mehreren Händen) als die die schon liegt
                        if (card.Zahl >= c.Zahl)
                        {
                            return true; //Ja, dann kann der Nutzer frei entscheiden
                        }
                        return false; //Wenn nicht, dann kann er die Karte nur ablegen
                    }
                }
                return true;//Es liegt noch keine Karte der ausgewählten Farbe auf dem Anlegestapel
            }
            else
            { //Spieler kann alles legen, weil noch keine Karte angelegt wurde
                return true;
            }
        }

        #region Helper
        public Player GetActivePlayer()
        {
            if (GameStatus == GameStatus.PlayerOneDrawCard || GameStatus == GameStatus.PlayerOnePlayCard)
            {
                return Player.PlayerOne;
            }
            else
            {
                return Player.PlayerTwo;
            }
        }

        private IStapel GetActiveAnlegestapel()
        {
            return GetActivePlayer() == Player.PlayerOne ? _anlegestapel : _anlegestapel2;
        }
        #endregion
    }
}
