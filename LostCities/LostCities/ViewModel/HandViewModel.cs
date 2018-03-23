using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using LostCities.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace LostCities.ViewModel
{
    public class HandViewModel : BaseViewModel
    {
        private int _abgelegteKarteIndex;

        public ObservableCollection<HandCard> HandCards { get; set; }

        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler KarteAnlegen;
        public event CardEventHandler KarteAblegen;

        public HandViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);

            _abgelegteKarteIndex = -1;

            HandCards = new ObservableCollection<HandCard>();
        }

        protected virtual void OnKarteAnlegen(CardEventArgs e)
        {
            KarteAnlegen?.Invoke(this, e);
        }

        protected virtual void OnKarteAblegen(CardEventArgs e)
        {
            KarteAblegen?.Invoke(this, e);
        }

        public ICommand OnButtonPressedCommand { get; }

        async void OnButtonPressed(string value)
        {
            var buttons = new String[] { "Karte ablegen", "Karte anlegen" };
            var answer = await App.Current.MainPage.DisplayActionSheet("Initialrunde beendet!", null, "Cancel",buttons );

            try
            {
                if (null != answer)
                {
                    if (answer != "Cancel")
                    {
                        var index = int.Parse(value);
                        var CardEventArgs = new CardEventArgs(HandCards[index].Card);                      
                        _abgelegteKarteIndex = index;
                        UpdateView(index); //Erst wieder reinnehmen, sobald der Spieler auch manuell wieder KArten aufnehmen soll.

                        switch (answer)
                        {
                            case "Karte anlegen":
                                OnKarteAnlegen(new CardEventArgs(null));
                                break;
                            case "Karte ablegen":
                                OnKarteAblegen(CardEventArgs);
                                break;
                        }                        
                    }
                }
            } catch (Exception e)
            {
                Debug.WriteLine("HandViewModel" + e.Message);
            }
        }

        public void GetHandCards(List<Card> cardList)
        {
            foreach(Card card in cardList)
            {
                HandCards.Add(new HandCard { Card = card, ImageUri = card.ImageUri, IsEnabled = true, IsVisible = true });
            }
        }

        public void GetHandCard(Card card)
        {
            if (null != card)
            {
                if (_abgelegteKarteIndex != -1)
                {
                    HandCards[_abgelegteKarteIndex].Card = card;
                }
            }
        }        

        private void UpdateView(int value)
        {
            HandCards[value].IsVisible = false;            
        }

        public void DisableHand()
        {
            DisableEnableAllButtons(false);
        }

        public void EnableHand()
        {
            DisableEnableAllButtons(true);
        }

        private void DisableEnableAllButtons(bool val)
        {
            foreach(HandCard handCard in HandCards)
            {
                handCard.IsEnabled = val;
            }
        }
    }
}
