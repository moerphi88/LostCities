using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using LostCities;
using LostCities.Model;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace LostCities.ViewModel
{
    public class HandViewModel : BaseViewModel
    {
        private string _ersteHandKarte;
        private string _zweiteHandKarte;
        private string _dritteHandKarte;
        private List<Card> _cardList;

        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler KarteAnlegen;
        public event CardEventHandler KarteAblegen;

        public HandViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);
            ErsteHandKarteImageUri = "kartenhindergrund.png";
            ZweiteHandKarteImageUri = null;
            DritteHandKarteImageUri = "kartenhindergrund.png";
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
            //todo hier die Antworten des ActionsSheets abfragen und Aktion ausführen.

            if (null != answer)
            {
                if (answer != "Cancel")
                {
                    //Hier muss try-catch etc. noch hin. Und der Index muss überprüft werden
                    var index = int.Parse(value);
                    var CardEventArgs = new CardEventArgs(_cardList[index]);
                    _cardList.RemoveAt(index);
                    UpdateView(index);

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
                return _dritteHandKarte;
            }

            set
            {
                _dritteHandKarte = value;
                OnPropertyChanged();
            }
        }

        public String DritteHandKarteImageUri
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

        private void UpdateViewModel()
        {
            try
            {
                ErsteHandKarteImageUri = _cardList[0].ImageUri;
                ZweiteHandKarteImageUri = _cardList[1].ImageUri;
                DritteHandKarteImageUri = _cardList[2].ImageUri;
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
                    break;
                case 1:
                    ZweiteHandKarteImageUri = null;
                    break;
                case 2:
                    DritteHandKarteImageUri = null;
                    break;
                default:
                    break;
            }
        }
    }
}
