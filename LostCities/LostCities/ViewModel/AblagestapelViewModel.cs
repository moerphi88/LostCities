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

namespace LostCities.ViewModel
{
    public class AblagestapelViewModel : BaseViewModel
    {
        private string _gelberStapelImageUri;
        private string _roterStapelImageUri;
        private string _gruenerStapelImageUri;
        private string _blauerStapelImageUri;
        private string _weißerStapelImageUri;

        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler KarteAbheben;
        
        protected virtual void OnKarteAbheben(CardEventArgs e)
        {
            KarteAbheben?.Invoke(this, e);
        }

        public AblagestapelViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);
            GelberStapelImageUri = "kartenhindergrund.png";
            BlauerStapelImageUri = "kartenhindergrund.png";
            GruenerStapelImageUri = "kartenhindergrund.png";
            RoterStapelImageUri = "kartenhindergrund.png";
            WeißerStapelImageUri = "kartenhindergrund.png";
        }
        public ICommand OnButtonPressedCommand { get; }

        async void OnButtonPressed(string value)
        {
            var answer = await App.Current.MainPage.DisplayAlert(null,"Wollen Sie die Karte wirklich aufnehmen?", "Ja", "Nein");

            try
            {
                if (answer)
                {
                    var CardEventArgs = new CardEventArgs(new Card());
                        OnKarteAbheben(CardEventArgs);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void KarteAblegen(Card Card)
        {
            switch (Card.Name)
            {
                case "Herz":
                    GelberStapelImageUri = Card.ImageUri;
                    break;
                case "Karo":
                    BlauerStapelImageUri = Card.ImageUri;
                    break;
                case "Pik":
                    GruenerStapelImageUri = Card.ImageUri;
                    break;
                case "Kreuz":
                    RoterStapelImageUri = Card.ImageUri;
                    break;
                default:
                    break;
            }
        }
        
        public String GelberStapelImageUri
        { 
            get
            {
                return _gelberStapelImageUri;
            }

            set
            {
                _gelberStapelImageUri = value;
                OnPropertyChanged();
            }
        }

        public String BlauerStapelImageUri
        {
            get
            {
                return _blauerStapelImageUri;
            }

            set
            {
                _blauerStapelImageUri = value;
                OnPropertyChanged();
            }
        }

        public String GruenerStapelImageUri
        {
            get
            {
                return _gruenerStapelImageUri;
            }

            set
            {
                _gruenerStapelImageUri = value;
                OnPropertyChanged();
            }
        }

        public String RoterStapelImageUri
        {
            get
            {
                return _roterStapelImageUri;
            }

            set
            {
                _roterStapelImageUri = value;
                OnPropertyChanged();
            }
        }

        public String WeißerStapelImageUri
        {
            get
            {
                return _weißerStapelImageUri;
            }

            set
            {
                _weißerStapelImageUri = value;
                OnPropertyChanged();
            }
        }


    }
}
