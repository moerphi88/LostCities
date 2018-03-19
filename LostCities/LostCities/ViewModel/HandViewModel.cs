using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using LostCities.Model;
using System.Diagnostics;

namespace LostCities.ViewModel
{
    public class HandViewModel : BaseViewModel
    {
        private int _abgelegteKarteIndex;

        private string _ersteHandKarte;
        private string _zweiteHandKarte;
        private string _dritteHandKarte;
        private List<Card> _cardList;

        private bool _isVisibleErsteHandKarte;
        private bool _isVisibleZweiteHandKarte;
        private bool _isVisibleDritteHandKarte;

        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler KarteAnlegen;
        public event CardEventHandler KarteAblegen;

        public HandViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);
            ErsteHandKarteImageUri = "kartenhindergrund.png";
            ZweiteHandKarteImageUri = "kartenhindergrund.png";
            DritteHandKarteImageUri = "kartenhindergrund.png";

            IsVisibleErsteHandKarte = true;
            IsVisibleZweiteHandKarte = true;
            IsVisibleDritteHandKarte = true;

            _abgelegteKarteIndex = -1;

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
                        //Hier muss try-catch etc. noch hin. Und der Index muss überprüft werden
                        var index = int.Parse(value);
                        var CardEventArgs = new CardEventArgs(_cardList[index]);
                        //_cardList.RemoveAt(index);
                        UpdateView(index);
                        _abgelegteKarteIndex = index;

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
            
            //switch (value)
            //{
            //    case "0":
            //        gelberstapelimageuri = new card(farbe.herz.tostring(), wert.dame.tostring()).imageuri;
            //        break;
            //    case "1":
            //        gruenerstapelimageuri = new card(farbe.karo.tostring(), wert.dame.tostring()).imageuri;
            //        break;
            //    case "2":
            //        roterstapelimageuri = new card(farbe.karo.tostring(), wert.dame.tostring()).imageuri;
            //        break;
            //    default:
            //        break;
            //}
        }

        public void GetHandCards(List<Card> cardList)
        {
            _cardList = cardList;
            UpdateViewModel();
        }

        public void GetHandCard(Card card)
        {
            if (null != card)
            {
                if (_abgelegteKarteIndex != -1)
                {
                    _cardList[_abgelegteKarteIndex] = card;
                    UpdateViewModel();
                }
            }
        }
        
        public String ErsteHandKarteImageUri
        { 
            get
            {
                return _ersteHandKarte;
            }

            set
            {
                _ersteHandKarte = value;
                OnPropertyChanged();
            }
        }        

        public String ZweiteHandKarteImageUri
        {
            get
            {
                return _zweiteHandKarte;
            }

            set
            {
                _zweiteHandKarte = value;
                OnPropertyChanged();
            }
        }

        public String DritteHandKarteImageUri
        {
            get
            {
                return _dritteHandKarte;
            }

            set
            {
                _dritteHandKarte = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisibleErsteHandKarte
        {
            get { return _isVisibleErsteHandKarte; }
            set
            {
                _isVisibleErsteHandKarte = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisibleZweiteHandKarte
        {
            get { return _isVisibleZweiteHandKarte; }
            set
            {
                _isVisibleZweiteHandKarte = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisibleDritteHandKarte
        {
            get { return _isVisibleDritteHandKarte; }
            set
            {
                _isVisibleDritteHandKarte = value;
                OnPropertyChanged();
            }
        }

        private void UpdateViewModel()
        {
            try
            {
                ErsteHandKarteImageUri = _cardList[0].ImageUri;
                ZweiteHandKarteImageUri = _cardList[1].ImageUri;
                DritteHandKarteImageUri = _cardList[2].ImageUri;

                IsVisibleErsteHandKarte = true;
                IsVisibleZweiteHandKarte = true;
                IsVisibleDritteHandKarte = true;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void UpdateView(int value)
        {
            switch (value)
            {
                case 0:
                    ErsteHandKarteImageUri = null;
                    IsVisibleErsteHandKarte = false;
                    break;
                case 1:
                    ZweiteHandKarteImageUri = null;
                    IsVisibleZweiteHandKarte = false;
                    break;
                case 2:
                    DritteHandKarteImageUri = null;
                    IsVisibleDritteHandKarte = false;
                    break;
                default:
                    break;
            }
        }
    }
}
