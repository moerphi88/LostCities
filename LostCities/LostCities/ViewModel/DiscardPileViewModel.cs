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
using LostCities.Service;

namespace LostCities.ViewModel
{
    public class DiscardPileViewModel : BaseViewModel
    {
        private const string IsEnabledKeyString = "is_enabled_key_string";
        private const string DiscardPileDictKeyString = "discard_pile_dict_key_string";

        private string _gelberStapelImageUri;
        private string _roterStapelImageUri;
        private string _gruenerStapelImageUri;
        private string _blauerStapelImageUri;
        private string _weißerStapelImageUri;
        private bool _isEnabled;

        private GameDataRepository _gameDataRepository;

        private Dictionary<Farbe, List<Card>> _discardPileDict;

        //TODO Ich muss nicht ein eigenes delegate definieren. Es gibt EventHandler, die man benutzen kann. Suche nach Raise and Handle Events at mdns https://docs.microsoft.com/de-de/dotnet/standard/events/ 
        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler KarteAbheben;
        
        protected virtual void OnKarteAbheben(CardEventArgs e)
        {
            KarteAbheben?.Invoke(this, e);
        }

        public DiscardPileViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);

            Init();
        }



        private void Init()
        {
            _gameDataRepository = new GameDataRepository();

            if(_gameDataRepository.GetGameSaved() == true)
            {
                IsEnabled = _gameDataRepository.GetBool(IsEnabledKeyString);
                _discardPileDict = _gameDataRepository.GetJsonDict(DiscardPileDictKeyString);

                foreach(KeyValuePair<Farbe,List<Card>> dict in _discardPileDict)
                {
                    UpdateImageUri(dict.Key);
                }

            } else
            {
                _discardPileDict = new Dictionary<Farbe, List<Card>>
                {
                    { Farbe.Weiss, new List<Card>() },
                    { Farbe.Gruen, new List<Card>() },
                    { Farbe.Blau, new List<Card>() },
                    { Farbe.Gelb, new List<Card>() },
                    { Farbe.Rot, new List<Card>() }
                };

                foreach (KeyValuePair<Farbe, List<Card>> dict in _discardPileDict)
                {
                    UpdateImageUri(dict.Key);
                }

                PersistDict();

                //GelberStapelImageUri = "kartenhindergrund.png";
                //BlauerStapelImageUri = "kartenhindergrund.png";
                //GruenerStapelImageUri = "kartenhindergrund.png";
                //RoterStapelImageUri = "kartenhindergrund.png";
                //WeißerStapelImageUri = "kartenhindergrund.png";
            }
        }


        private async void OnButtonPressed(string value)
        {
            try
            {
                var answer = false;
                var farbe = (Farbe) Enum.Parse(typeof(Farbe), value);

                if (_discardPileDict[farbe].Count != 0)
                {
                    answer = await App.Current.MainPage.DisplayAlert(null, "Willst Du die Karte wirklich aufnehmen?", "Ja", "Nein");
                }
            
                if (answer)
                {
                    OnKarteAbheben(CreateCardEventArgs(farbe));
                    _discardPileDict[farbe].RemoveAt(_discardPileDict[farbe].Count-1);
                    UpdateImageUri(farbe);
                    PersistDict();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void PersistDict()
        {
            _gameDataRepository.SetJsonDict(DiscardPileDictKeyString, _discardPileDict);
        }

        public void KarteAblegen(Card card)
        {
            switch (card.Name)
            {
                case "Weiss":
                    _discardPileDict[Farbe.Weiss].Add(card);
                    WeißerStapelImageUri = SetImageUri(Farbe.Weiss);
                    break;
                case "Gruen":
                    _discardPileDict[Farbe.Gruen].Add(card);
                    GruenerStapelImageUri = SetImageUri(Farbe.Gruen);
                    break;
                case "Blau":
                    _discardPileDict[Farbe.Blau].Add(card);
                    BlauerStapelImageUri = SetImageUri(Farbe.Blau);
                    break;
                case "Gelb":
                    _discardPileDict[Farbe.Gelb].Add(card);
                    GelberStapelImageUri = SetImageUri(Farbe.Gelb);
                    break;
                case "Rot":
                    _discardPileDict[Farbe.Rot].Add(card);
                    RoterStapelImageUri = SetImageUri(Farbe.Rot);
                    break;
                default:
                    break;
            }

            PersistDict();
        }

        private String SetImageUri(Farbe farbe)
        {
            if (_discardPileDict[farbe].Count != 0)
            {
                return _discardPileDict[farbe].ElementAt(_discardPileDict[farbe].Count - 1).ImageUri;
            }
            else return "kartenhindergrund.png";
        }

        //TODO Dank neuer Erkenntnis, kann ich CardEventArgs auch weg rationalisieren und einfach eine Card übergeben
        private CardEventArgs CreateCardEventArgs(Farbe farbe)
        {
            var card = _discardPileDict[farbe].ElementAt(_discardPileDict[farbe].Count - 1);
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
                _gameDataRepository.SetBool(IsEnabledKeyString,_isEnabled);
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
