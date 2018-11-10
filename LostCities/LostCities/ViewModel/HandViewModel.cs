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
        
        private String _buttonName = "Hide Hand";

        public String HideShowButtonName
        {
            get { return _buttonName; }
            set { _buttonName = value; OnPropertyChanged(); }
        }

        private bool _cardsAreHidden = false;

        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler PlayCard;

        public HandViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);
            OnHideHandCommand = new Command(OnHideHand);
           

            _abgelegteKarteIndex = -1;

            HandCards = new ObservableCollection<HandCard>();
        }

        protected virtual void OnPlayCard(CardEventArgs e)
        {
            PlayCard?.Invoke(this, e);
        }

        public ICommand OnButtonPressedCommand { get; }
        public ICommand OnHideHandCommand { get; }

        void OnButtonPressed(string value)
        {
            var index = int.Parse(value);
            var CardEventArgs = new CardEventArgs(HandCards[index].Card);
            _abgelegteKarteIndex = index;
            HideDrawnCard(index);

            OnPlayCard(CardEventArgs);
        }

        void OnHideHand()
        {
            if (_cardsAreHidden)
            {
                foreach(var c in HandCards)
                {
                    c.IsVisible = true;
                }
                HideShowButtonName = "Hide Hand";
            }
            else
            {
                foreach (var c in HandCards)
                {
                    c.IsVisible = false;
                }
                HideShowButtonName = "Show Hand";
            }
            _cardsAreHidden = !_cardsAreHidden;
        }

        //Adds a list of cards to the ObservableCollection
        public void GetHandCards(List<Card> cardList)
        {
            foreach(Card card in cardList)
            {
                HandCards.Add(new HandCard { Card = card, ImageUri = card.ImageUri, IsEnabled = true, IsVisible = true });
            }
        }

        //Adds a new card to the ObservableCollection. Exactly to that spot where the last card has been drawn
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

        private void HideDrawnCard(int idx)
        {
            HandCards[idx].IsVisible = false;            
        }

        public void DisableHand()
        {
            DisableEnableAllButtons(false);
        }

        public void EnableHand()
        {
            DisableEnableAllButtons(true);
        }

        private void DisableEnableAllButtons(bool value)
        {
            foreach(HandCard handCard in HandCards)
            {
                handCard.IsEnabled = value;
            }
        }
    }
}
