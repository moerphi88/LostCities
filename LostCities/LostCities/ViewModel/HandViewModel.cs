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
        public event CardEventHandler PlayCard;

        public HandViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);

            _abgelegteKarteIndex = -1;

            HandCards = new ObservableCollection<HandCard>();
        }

        protected virtual void OnPlayCard(CardEventArgs e)
        {
            PlayCard?.Invoke(this, e);
        }

        public ICommand OnButtonPressedCommand { get; }

        void OnButtonPressed(string value)
        {
            var index = int.Parse(value);
            var CardEventArgs = new CardEventArgs(HandCards[index].Card);
            _abgelegteKarteIndex = index;
            UpdateView(index);

            OnPlayCard(CardEventArgs);
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
