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
        //TODO Hier könnte ich ein dict einführen. Key wäre die Farbe. und value eine Liste von Cards. Nur die oberste würde angezeigt werden.
        private List<Card> _cardList;
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
            _ablagestapel.Add(Farbe.Herz, new List<Card>());
            _ablagestapel.Add(Farbe.Karo, new List<Card>());
            _ablagestapel.Add(Farbe.Kreuz, new List<Card>());
            _ablagestapel.Add(Farbe.Pik, new List<Card>());

            GelberStapelImageUri = "kartenhindergrund.png";
            BlauerStapelImageUri = "kartenhindergrund.png";
            GruenerStapelImageUri = "kartenhindergrund.png";
            RoterStapelImageUri = "kartenhindergrund.png";
            WeißerStapelImageUri = "kartenhindergrund.png";
        }

        public ICommand OnButtonPressedCommand { get; }

        async void OnButtonPressed(string value)
        {
            var answer = await App.Current.MainPage.DisplayAlert(null,"Willst Du die Karte wirklich aufnehmen?", "Ja", "Nein");

            try
            {
                if (answer)
                {
                    var farbe = Farbe.Herz;
                    switch (value)
                    {
                        case "0":
                            farbe = Farbe.Herz;
                            break;
                        case "1":
                            farbe = Farbe.Karo;
                            break;
                        case "2":
                            farbe = Farbe.Pik;
                            break;
                        case "3":
                            farbe = Farbe.Kreuz;
                            break;
                    }
                    if (_ablagestapel[farbe].Count != 0)
                    {
                        OnKarteAbheben(CreateCardEventArgs(farbe));
                        _ablagestapel[farbe].RemoveAt(_ablagestapel[farbe].Count-1);
                        UpdateImageUri(farbe);
                    }
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
                case "Herz":
                    _ablagestapel[Farbe.Herz].Add(card);
                    GelberStapelImageUri = SetImageUri(Farbe.Herz);
                    break;
                case "Karo":
                    _ablagestapel[Farbe.Karo].Add(card);
                    BlauerStapelImageUri = SetImageUri(Farbe.Karo);
                    break;
                case "Pik":
                    _ablagestapel[Farbe.Pik].Add(card);
                    GruenerStapelImageUri = SetImageUri(Farbe.Pik);
                    break;
                case "Kreuz":
                    _ablagestapel[Farbe.Kreuz].Add(card);
                    RoterStapelImageUri = SetImageUri(Farbe.Kreuz);
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

        private String SetImageUri(Farbe farbe)
        {
            if (_ablagestapel[farbe].Count != 0)
            {
                return _ablagestapel[farbe].ElementAt(_ablagestapel[farbe].Count - 1).ImageUri;
            }
            else return "kartenhindergrund.png";
        }

        private CardEventArgs CreateCardEventArgs(Farbe farbe)
        {
            var card = _ablagestapel[farbe].ElementAt(_ablagestapel[farbe].Count - 1);
            return new CardEventArgs(card);
        }

        private void UpdateImageUri(Farbe farbe)
        {
            switch (farbe)
            {
                case Farbe.Herz:
                    GelberStapelImageUri = SetImageUri(Farbe.Herz);
                    break;
                case Farbe.Karo:
                    BlauerStapelImageUri = SetImageUri(Farbe.Karo);
                    break;
                case Farbe.Pik:
                    GruenerStapelImageUri = SetImageUri(Farbe.Pik);
                    break;
                case Farbe.Kreuz:
                    RoterStapelImageUri = SetImageUri(Farbe.Kreuz);
                    break;
                default:
                    break;
            }
        }

    }
}
