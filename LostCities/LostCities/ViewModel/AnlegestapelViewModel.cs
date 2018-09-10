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
            _stapel = new Dictionary<Farbe, List<Card>>
            {
                { Farbe.Weiss, new List<Card>() },
                { Farbe.Gruen, new List<Card>() },
                { Farbe.Blau, new List<Card>() },
                { Farbe.Gelb, new List<Card>() },
                { Farbe.Rot, new List<Card>()  }
            };

            GelberStapelImageUri = "kartenhindergrund.png";
            BlauerStapelImageUri = "kartenhindergrund.png";
            GruenerStapelImageUri = "kartenhindergrund.png";
            RoterStapelImageUri = "kartenhindergrund.png";
            WeißerStapelImageUri = "kartenhindergrund.png";
        }
        
        //TODO An dieser Stelle muss ich das enum Farbe auswerten anstatt einen string. Refactoring
        public void KarteAnlegen(Card card)
        {
            //TODO Diese Logik gehört ins Model bzw, in eine Base
            switch (card.Name)
            {
                case "Weiss":
                    _stapel[Farbe.Weiss].Add(card);
                    WeißerStapelImageUri = SetImageUri(Farbe.Weiss);
                    break;
                case "Gruen":
                    _stapel[Farbe.Gruen].Add(card);
                    GruenerStapelImageUri = SetImageUri(Farbe.Gruen);
                    break;
                case "Blau":
                    _stapel[Farbe.Blau].Add(card);
                    BlauerStapelImageUri = SetImageUri(Farbe.Blau);
                    break;
                case "Gelb":
                    _stapel[Farbe.Gelb].Add(card);
                    GelberStapelImageUri = SetImageUri(Farbe.Gelb);
                    break;
                case "Rot":
                    _stapel[Farbe.Rot].Add(card);
                    RoterStapelImageUri = SetImageUri(Farbe.Rot);
                    break;
                default:
                    break;
            }
        }

        public List<Card> GetTopCards()
        {
            var list = new List<Card>();
            foreach (Farbe farbe in Enum.GetValues(typeof(Farbe))) {
                if (_stapel[farbe].Count != 0)
                {
                    list.Add(_stapel[farbe][_stapel[farbe].Count - 1]);
                }
            }
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
            //TODO Diese Logik gehört ins Model
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
