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
        private String _buttonName = "Hide Hand";
        private bool _cardsAreHidden = false;

        public ObservableCollection<HandCard> HandCards { get; set; } 
        public String HideShowButtonName
        {
            get { return _buttonName; }
            set { _buttonName = value; OnPropertyChanged(); }
        }

        public ICommand OnButtonPressedCommand { get; }
        public ICommand OnHideHandCommand { get; }

        public Card SelectedCard { get; private set; }
        public event EventHandler SelectedCardEvent;
        protected virtual void CardSelected(EventArgs e)
        {
            SelectedCardEvent?.Invoke(this, e);
        }

        public HandViewModel(INavigation navigation) : base(navigation)
        {
            HandCards = new ObservableCollection<HandCard>();

            OnButtonPressedCommand = new Command<string>(OnButtonPressed);
            OnHideHandCommand = new Command(OnHideHand);           

            _abgelegteKarteIndex = -1;
        }

        void OnButtonPressed(string value)
        {
            var index = int.Parse(value);
            _abgelegteKarteIndex = index;
            SelectedCard = HandCards[index].Card;
            CardSelected(null);
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

        public void HideDrawnCard()
        {
            HideDrawnCard(_abgelegteKarteIndex);
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
