using LostCities.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LostCities.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private const int HandCards = 8;

        public DiscardPileViewModel DiscardPileVM { get; set; }
        public IStapel AnlegeStapelVM { get; set; }
        public IStapel AnlegeStapel2VM { get; set; }
        public HandViewModel HandVM { get; set; }
        public HandViewModel HandVM2 { get; set; }
        public LostCitiesGameLogic Lcgl { get; set; }



        public MainViewModel(INavigation navigation) : base(navigation)
        {
            DiscardPileVM = new DiscardPileViewModel(null);
            //AnlegeStapelVM = new MauMauViewModel(null);
            AnlegeStapelVM = new AnlegestapelViewModel(null);
            AnlegeStapel2VM = new AnlegestapelViewModel(null);
            HandVM = new HandViewModel(null);
            HandVM2 = new HandViewModel(null);
            Lcgl = new LostCitiesGameLogic(HandVM, HandVM2, DiscardPileVM, AnlegeStapelVM, AnlegeStapel2VM);

            OnKarteZiehenButtonPressedCommand = new Command(OnButtonPressed);

            HandVM.GetHandCards(Lcgl.CardDeck.GetXCards(HandCards));
            HandVM2.GetHandCards(Lcgl.CardDeck.GetXCards(HandCards));
            
            // Eventbinding
            //TODO die Events müssen noch wieder abgemeldet werden. Ggf. im Destruktor?!
            HandVM.PlayCard += Lcgl.OnPlayCard;
            HandVM2.PlayCard += Lcgl.OnPlayCard;

            Lcgl.StatusChangedEvent += OnStatusChanged;

            DiscardPileVM.KarteAbheben += Lcgl.OnKarteAbheben;
            KarteZiehenButtonIsEnabled = true;
            
        }

        private bool _karteZiehenButtonIsEnabeld;
        public bool KarteZiehenButtonIsEnabled
        {
            get
            {
                return _karteZiehenButtonIsEnabeld;
            }
            set
            {
                _karteZiehenButtonIsEnabeld = value;
                OnPropertyChanged();
            }
        }

        private string _countCard = "";
        public string CountCard
        {
            get
            {
                return _countCard;
            }
            set
            {
                _countCard = value;
                if (int.Parse(_countCard) <= 5) OnPropertyChanged();
            }
        }

        public ICommand OnKarteZiehenButtonPressedCommand { get; }
        private void OnButtonPressed()
        {
            Lcgl.DrawHandCard();
            CountCard = Lcgl.CardDeck.CountCard.ToString();
        }

        public void OnStatusChanged(object sender, EventArgs e)
        {
            Update();            
        }

        public override void Update()
        {
            base.Update();
            Debug.WriteLine("MainViewModel: Update(): {0}",Lcgl.GameStatus.ToString());
        }
    }
}
