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
    public class AblagestapelViewModel : BaseViewModel
    {
        private string _gelberStapelImageUri;
        private string _roterStapelImageUri;
        private string _gruenerStapelImageUri;
        private string _blauerStapelImageUri;
        private string _weißerStapelImageUri;

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

        void OnButtonPressed(string value)
        {
            switch (value)
            {
                case "0":
                    GelberStapelImageUri = new Card(Farbe.Herz.ToString(), Wert.Dame.ToString()).ImageUri;
                    break;
                case "1":
                    BlauerStapelImageUri = new Card(Farbe.Kreuz.ToString(), Wert.Acht.ToString()).ImageUri;
                    break;
                case "2":
                    GruenerStapelImageUri = new Card(Farbe.Karo.ToString(), Wert.Dame.ToString()).ImageUri;
                    break;
                case "3":
                    RoterStapelImageUri = new Card(Farbe.Karo.ToString(), Wert.Dame.ToString()).ImageUri;
                    break;
                case "4":
                    WeißerStapelImageUri = new Card(Farbe.Karo.ToString(), Wert.Dame.ToString()).ImageUri;
                    break;
                default:
                    break;
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
                    break;
                case "Pik":
                    break;
                case "Kreuz":
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
