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
        private bool _isEnabled;

        private Dictionary<Farbe, List<Card>> _ablagestapel;

        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler KarteAbheben;
        
        protected virtual void OnKarteAbheben(CardEventArgs e)
        {
            KarteAbheben?.Invoke(this, e);
        }

        public AblagestapelViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);

            _ablagestapel = new Dictionary<Farbe, List<Card>>();
            _ablagestapel.Add(Farbe.Weiss, new List<Card>());
            _ablagestapel.Add(Farbe.Gruen, new List<Card>());
            _ablagestapel.Add(Farbe.Blau, new List<Card>());
            _ablagestapel.Add(Farbe.Gelb, new List<Card>());
            _ablagestapel.Add(Farbe.Rot, new List<Card>());

            GelberStapelImageUri = "kartenhindergrund.png";
            BlauerStapelImageUri = "kartenhindergrund.png";
            GruenerStapelImageUri = "kartenhindergrund.png";
            RoterStapelImageUri = "kartenhindergrund.png";
            WeißerStapelImageUri = "kartenhindergrund.png";
        }

        async void OnButtonPressed(string value)
        {
            try
            {
                var answer = false;
                var farbe = Farbe.Weiss;
                switch (value)
                {
                case "0":
                    farbe = Farbe.Weiss;
                    break;
                case "1":
                    farbe = Farbe.Gruen;
                    break;
                case "2":
                    farbe = Farbe.Blau;
                    break;
                case "3":
                    farbe = Farbe.Gelb;
                    break;
                case "4":
                    farbe = Farbe.Rot;
                    break;
                }

                if (_ablagestapel[farbe].Count != 0)
                {
                    answer = await App.Current.MainPage.DisplayAlert(null, "Willst Du die Karte wirklich aufnehmen?", "Ja", "Nein");
                }
            
                if (answer)
                {
                    OnKarteAbheben(CreateCardEventArgs(farbe));
                    _ablagestapel[farbe].RemoveAt(_ablagestapel[farbe].Count-1);
                    UpdateImageUri(farbe);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void KarteAblegen(Card card)
        {
            switch (card.Name)
            {
                case "Weiss":
                    _ablagestapel[Farbe.Weiss].Add(card);
                    WeißerStapelImageUri = SetImageUri(Farbe.Weiss);
                    break;
                case "Gruen":
                    _ablagestapel[Farbe.Gruen].Add(card);
                    GruenerStapelImageUri = SetImageUri(Farbe.Gruen);
                    break;
                case "Blau":
                    _ablagestapel[Farbe.Blau].Add(card);
                    BlauerStapelImageUri = SetImageUri(Farbe.Blau);
                    break;
                case "Gelb":
                    _ablagestapel[Farbe.Gelb].Add(card);
                    GelberStapelImageUri = SetImageUri(Farbe.Gelb);
                    break;
                case "Rot":
                    _ablagestapel[Farbe.Rot].Add(card);
                    RoterStapelImageUri = SetImageUri(Farbe.Rot);
                    break;
                default:
                    break;
            }
        }

        private String SetImageUri(Farbe farbe)
        {
            if (_ablagestapel[farbe].Count != 0)
            {
                return _ablagestapel[farbe].ElementAt(_ablagestapel[farbe].Count - 1).ImageUri;
            }
            else return "kartenhindergrund.png";
        }

        //TODO Dank neuer Erkenntnis, kann ich CardEventArgs auch weg rationalisieren und einfach eine Card übergeben
        private CardEventArgs CreateCardEventArgs(Farbe farbe)
        {
            var card = _ablagestapel[farbe].ElementAt(_ablagestapel[farbe].Count - 1);
            return new CardEventArgs(card);
        }

        private void UpdateImageUri(Farbe farbe)
        {
            switch (farbe)
            {
                case Farbe.Weiss:
                    WeißerStapelImageUri = SetImageUri(Farbe.Weiss);
                    break;
                case Farbe.Gruen:
                    GruenerStapelImageUri = SetImageUri(Farbe.Gruen);
                    break;
                case Farbe.Blau:
                    BlauerStapelImageUri = SetImageUri(Farbe.Blau);
                    break;
                case Farbe.Gelb:
                    GelberStapelImageUri = SetImageUri(Farbe.Gelb);
                    break;
                case Farbe.Rot:
                    RoterStapelImageUri = SetImageUri(Farbe.Rot);
                    break;
                default:
                    break;
            }
        }

        public void DisableDrawing()
        {
            IsEnabled = false;
        }

        public void EnableDrawing()
        {
            IsEnabled = true;
        }

        #region Properties

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnButtonPressedCommand { get; }

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

        #endregion



    }
}
