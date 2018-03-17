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
            var answer = await App.Current.MainPage.DisplayActionSheet("Initialrunde beendet!", "Was möchtest du mit deiner Karte anstellen?", "Cancel",buttons );
            //todo hier die Antworten des ActionsSheets abfragen und Aktion ausführen.

            switch (answer)
            {
                case "Karte anlegen":
                    OnKarteAnlegen(new CardEventArgs(null));
                    break;
                case "Karte ablegen":
                    OnKarteAblegen(new CardEventArgs(new Card("Herz", "Dame")));
                    break;
            }
            
            //switch (value)
            //{
            //    case "0":
            //        GelberStapelImageUri = new Card(Farbe.Herz.ToString(), Wert.Dame.ToString()).ImageUri;
            //        break;
            //    case "1":
            //        GruenerStapelImageUri = new Card(Farbe.Karo.ToString(), Wert.Dame.ToString()).ImageUri;
            //        break;
            //    case "2":
            //        RoterStapelImageUri = new Card(Farbe.Karo.ToString(), Wert.Dame.ToString()).ImageUri;
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
            ErsteHandKarteImageUri = _cardList[0].ImageUri;
            ZweiteHandKarteImageUri = _cardList[1].ImageUri;
            DritteHandKarteImageUri = _cardList[2].ImageUri;
        }
    }
}
