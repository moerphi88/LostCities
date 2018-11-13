using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using LostCities.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;
using LostCities.Service;

namespace LostCities.ViewModel
{
    public class HandViewModel : BaseViewModel
    {
        private int _abgelegteKarteIndex;
        private bool _cardsAreHidden = false;
        private String _hideShowButtonName = "Hide Hand";
        private GameDataRepository _gameDataRepository;

        public String HideShowButtonName
        {
            get { return _hideShowButtonName; }
            set { _hideShowButtonName = value; OnPropertyChanged(); }
        }
        public ObservableCollection<HandCard> HandCards { get; set; }
        public ICommand OnUserSelectedHandCardToPlayCommand { get; }
        public ICommand OnHideHandCommand { get; }

        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler PlayCard;

        public HandViewModel(INavigation navigation) : base(navigation)
        {
            OnUserSelectedHandCardToPlayCommand = new Command<string>(OnUserSelectedHandCardToPlay);
            OnHideHandCommand = new Command(OnHideHand);

            Init();
        }

        private void Init()
        {
            _gameDataRepository = new GameDataRepository();
            _abgelegteKarteIndex = -1;   
            HandCards = new ObservableCollection<HandCard>();
        }

        private void PersistCollection(string key)
        {
            _gameDataRepository.SetJsonCollection(key, HandCards);
        }

        protected virtual void OnPlayCard(CardEventArgs e)
        {
            PlayCard?.Invoke(this, e);
        }

        private void OnUserSelectedHandCardToPlay(string value)
        {
            var index = int.Parse(value);
            var CardEventArgs = new CardEventArgs(HandCards[index].Card);
            _abgelegteKarteIndex = index;
            HideDrawnCard(index);

            OnPlayCard(CardEventArgs);
        }
        private void OnHideHand()
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
        public void GetHandCards(List<Card> cardList, string keyToPersist)
        {
            foreach(Card card in cardList)
            {
                HandCards.Add(new HandCard { Card = card, ImageUri = card.ImageUri, IsEnabled = true, IsVisible = true });
            }
            PersistCollection(keyToPersist);
        }

        public void GetHandCardsFromPersistency(string key)
        {
            //Frage an Saschas: Ich übergebe hier ja nur die Referenz zum Object, was passiert, wenn das übergebene Object gelöscht wird. M.w.n. bleibt das Object solange erhalten wie es Refernzen gibt, korrekt? Danach kommt der GC
            HandCards = _gameDataRepository.GetJsonCollection(key);
            OnHideHand();
        }

        //Adds a new card to the ObservableCollection. Exactly to that spot where the last card has been drawn
        public void GetHandCard(Card card, string keyToPersist)
        {
            if (null != card)
            {
                if (_abgelegteKarteIndex != -1)
                {
                    HandCards[_abgelegteKarteIndex].Card = card;
                }
                PersistCollection(keyToPersist);
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
