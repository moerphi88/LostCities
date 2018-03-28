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
    public class AnlegestapelViewModel : BaseViewModel, IStapel
    {
        private string _gelberStapelImageUri;
        private string _roterStapelImageUri;
        private string _gruenerStapelImageUri;
        private string _blauerStapelImageUri;
        private string _weißerStapelImageUri;

        //TODO Hier könnte ich ein dict einführen. Key wäre die Farbe. und value eine Liste von Cards. Nur die oberste würde angezeigt werden.
        private Dictionary<Farbe, List<Card>> _stapel;

        public AnlegestapelViewModel(INavigation navigation) : base(navigation)
        {
            _stapel = new Dictionary<Farbe, List<Card>>();
            _stapel.Add(Farbe.Herz, new List<Card>());
            _stapel.Add(Farbe.Karo, new List<Card>());
            _stapel.Add(Farbe.Kreuz, new List<Card>());
            _stapel.Add(Farbe.Pik, new List<Card>());

            GelberStapelImageUri = "kartenhindergrund.png";
            BlauerStapelImageUri = "kartenhindergrund.png";
            GruenerStapelImageUri = "kartenhindergrund.png";
            RoterStapelImageUri = "kartenhindergrund.png";
            WeißerStapelImageUri = "kartenhindergrund.png";
        }
        
        public void KarteAnlegen(Card card)
        {
            switch (card.Name)
            {
                case "Herz":
                    _stapel[Farbe.Herz].Add(card);
                    GelberStapelImageUri = SetImageUri(Farbe.Herz);
                    break;
                case "Karo":
                    _stapel[Farbe.Karo].Add(card);
                    BlauerStapelImageUri = SetImageUri(Farbe.Karo);
                    break;
                case "Pik":
                    _stapel[Farbe.Pik].Add(card);
                    GruenerStapelImageUri = SetImageUri(Farbe.Pik);
                    break;
                case "Kreuz":
                    _stapel[Farbe.Kreuz].Add(card);
                    RoterStapelImageUri = SetImageUri(Farbe.Kreuz);
                    break;
                default:
                    break;
            }
        }

        public List<Card> GetTopCards()
        {
            var list = new List<Card>();
            list.Add(_stapel[Farbe.Herz][_stapel[Farbe.Herz].Count - 1]);
            list.Add(_stapel[Farbe.Kreuz][_stapel[Farbe.Herz].Count - 1]);
            list.Add(_stapel[Farbe.Pik][_stapel[Farbe.Herz].Count - 1]);
            list.Add(_stapel[Farbe.Karo][_stapel[Farbe.Herz].Count - 1]);
            return list;
        }

        private String SetImageUri(Farbe farbe)
        {
            if (_stapel[farbe].Count != 0)
            {
                return _stapel[farbe].ElementAt(_stapel[farbe].Count - 1).ImageUri;
            }
            else return "kartenhindergrund.png";
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

        #region Properties

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
