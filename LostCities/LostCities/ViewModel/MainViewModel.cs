using LostCities.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            HandVM.GetHandCards(Lcgl.CardDeck.GetXCards(HandCards));
            HandVM2.GetHandCards(Lcgl.CardDeck.GetXCards(HandCards));
            
            // Eventbinding
            //TODO die Events müssen noch wieder abgemeldet werden. Ggf. im Destruktor?!
            HandVM.PlayCard += Lcgl.OnPlayCard;
            HandVM2.PlayCard += Lcgl.OnPlayCard;

            DiscardPileVM.KarteAbheben += Lcgl.OnKarteAbheben;
        }
    }
}
