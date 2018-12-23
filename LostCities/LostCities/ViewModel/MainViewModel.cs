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

        public PopupDialogViewModel PopupDialogViewModel { get; set; }



        public void CardAblegen()
        {
            var hand = GetActiveHand() as HandViewModel;
            hand.HideDrawnCard();
            DiscardPileVM.KarteAblegen(hand.SelectedCard);
            PopupDialogViewModel.DialogIsVisible = false;
            Lcgl.GameStateMachine();
        }

        public void CardAnlegen()
        {
            var hand = GetActiveHand() as HandViewModel;
            hand.HideDrawnCard();
            GetActiveAnlegestapel().KarteAnlegen(hand.SelectedCard);
            PopupDialogViewModel.DialogIsVisible = false;
            Lcgl.GameStateMachine();
        }


        public MainViewModel(INavigation navigation) : base(navigation)
        {
            DiscardPileVM = new DiscardPileViewModel(null);
            //AnlegeStapelVM = new MauMauViewModel(null);
            AnlegeStapelVM = new AnlegestapelViewModel(null);
            AnlegeStapel2VM = new AnlegestapelViewModel(null);
            HandVM = new HandViewModel(null);
            HandVM2 = new HandViewModel(null);
            Lcgl = new LostCitiesGameLogic(HandVM, HandVM2, DiscardPileVM, AnlegeStapelVM, AnlegeStapel2VM);

            PopupDialogViewModel = new PopupDialogViewModel(null);
            PopupDialogViewModel.SetCommand(new Command(() => { CardAblegen(); }), new Command(() => { CardAnlegen(); }));

            OnKarteZiehenButtonPressedCommand = new Command(OnButtonPressed);

            HandVM.GetHandCards(Lcgl.CardDeck.GetXCards(HandCards));
            HandVM2.GetHandCards(Lcgl.CardDeck.GetXCards(HandCards));
            
            // Eventbinding
            //TODO die Events müssen noch wieder abgemeldet werden. Ggf. im Destruktor?!
            HandVM.PlayCard += Lcgl.OnPlayCard;
            HandVM2.PlayCard += Lcgl.OnPlayCard;

            HandVM.SelectedCardEvent += OnCardSelected;
            HandVM2.SelectedCardEvent += OnCardSelected;

            Lcgl.StatusChangedEvent += OnStatusChanged;

            DiscardPileVM.KarteAbheben += Lcgl.OnKarteAbheben;
            KarteZiehenButtonIsEnabled = true; // TODO Am Ende löschen

            StartGame();
        }

        private void OnCardSelected(object sender, EventArgs e)
        {
            var handViewModel = sender as HandViewModel;
            ShowDialog();
        }

        private void ShowDialog()
        {
            PopupDialogViewModel.Rotation = GetActivePlayer() == Player.PlayerOne ? 180 : 0;
            PopupDialogViewModel.CreatePopupDialog("Spieler X ist am Zug!", "Test", "Ablegen", "Anlegen", "CANCEL");            
        }

        private void StartGame()
        {
            Lcgl.InitGame();            
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

            switch (Lcgl.GameStatus)
            {
                case GameStatus.Idle:
                    HandVM.DisableHand();
                    HandVM2.DisableHand();
                    DiscardPileVM.DisableDrawing();
                    KarteZiehenButtonIsEnabled = false; //TODO Am Ende Kommentar wegnehmen!
                    break;
                case GameStatus.PlayerOnePlayCard:
                    HandVM.EnableHand();
                    HandVM2.DisableHand();
                    break;
                case GameStatus.PlayerTwoPlayCard:
                    HandVM2.EnableHand();
                    HandVM.DisableHand();
                    break;
                case GameStatus.PlayerOneDrawCard:
                case GameStatus.PlayerTwoDrawCard:
                    HandVM.DisableHand();
                    HandVM2.DisableHand();
                    DiscardPileVM.EnableDrawing();
                    KarteZiehenButtonIsEnabled = true; //TODO Am Ende Kommentar wegnehmen!
                    break;
                case GameStatus.GameOver:
                    PopupDialogViewModel.CreatePopupDialog("Das Spiel ist zu Ende", Lcgl.ShowWinnerWithPoints(), null, null, "OK");
                    break;
                default:
                    break;
            }
        }


        #region Helper

        // TODO Kind of helper, that return the active player. Is it possible to have this logic directly in GameLogic?!
        private Player GetActivePlayer()
        {
            if (Lcgl.GameStatus == GameStatus.PlayerOneDrawCard || Lcgl.GameStatus == GameStatus.PlayerOnePlayCard)
            {
                return Player.PlayerOne;
            }
            else
            {
                return Player.PlayerTwo;
            }
        }

        private HandViewModel GetActiveHand()
        {
            if (GetActivePlayer() == Player.PlayerOne)
            {
                return HandVM;
            }
            else
            {
                return HandVM2;
            }
        }

        private IStapel GetActiveAnlegestapel()
        {
            if (GetActivePlayer() == Player.PlayerOne)
            {
                return AnlegeStapelVM;
            }
            else
            {
                return AnlegeStapel2VM;
            }
        }

        #endregion

    }
}
