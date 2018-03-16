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
        private string _roterStapelImageUri;
        private string _gruenerStapelImageUri;

        public HandViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);
            GelberStapelImageUri = "kartenhindergrund.png";            
            GruenerStapelImageUri = null;
            RoterStapelImageUri = "kartenhindergrund.png";
        }
        public ICommand OnButtonPressedCommand { get; }

        async void OnButtonPressed(string value)
        {
            var buttons = new String[] { "Karte ablegen", "Karte anlegen" };
            var answer = await App.Current.MainPage.DisplayActionSheet("Initialrunde beendet!", "Was möchtest du mit deiner Karte anstellen?", "Cancel",buttons );
            //todo hier die Antworten des ActionsSheets abfragen und Aktion ausführen.

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
        
        public String GelberStapelImageUri
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
    }
}
