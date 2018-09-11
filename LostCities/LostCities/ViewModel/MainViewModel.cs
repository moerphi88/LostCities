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

            DiscardPileTitleLabelText = "Ablagestapel";
            AnlegestapelTitleLabelText = "Anlegestapel";
            HandEinsTitleLabelText = "Hand Spieler 1";
            HandZweiTitleLabelText = "Hand Spieler 2";            
        }



        // Muss ich wirklich die Set Methode implementieren und OnPropertyCHanged aufrufen, oder gibt es eine andere Möglichkeit den Wert zu aktualisieren?! 
        #region Properties

        public String DiscardPileTitleLabelText { get; set; }
        public String AnlegestapelTitleLabelText { get; set; }
        public String HandEinsTitleLabelText { get; set; }
        public String HandZweiTitleLabelText { get; set; }
       

        #endregion

    }
}
