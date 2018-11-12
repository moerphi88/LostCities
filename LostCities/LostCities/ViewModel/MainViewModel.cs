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

        //Warum sind die eigentlich public und als Properties?
        public DiscardPileViewModel DiscardPileVM { get; set; }
        public IStapel AnlegeStapelVM { get; set; }
        public IStapel AnlegeStapel2VM { get; set; }
        public HandViewModel HandVM { get; set; }
        public HandViewModel HandVM2 { get; set; }
        public LostCitiesGameLogic Lcgl { get; set; }
        private GameDataRepository _gameDataRepository;


        public MainViewModel(INavigation navigation) : base(navigation)
        {
            //TODO Hier beim erstellen muss ich checken, ob ein altes Spiel vorhanden ist. Kann ich ja auch mit einer Preference machen.
            //

            DiscardPileVM = new DiscardPileViewModel(null);
            AnlegeStapelVM = new AnlegestapelViewModel(null);
            AnlegeStapel2VM = new AnlegestapelViewModel(null);
            HandVM = new HandViewModel(null);
            HandVM2 = new HandViewModel(null);
            Lcgl = new LostCitiesGameLogic(HandVM, HandVM2, DiscardPileVM, AnlegeStapelVM, AnlegeStapel2VM);



            _gameDataRepository = new GameDataRepository();

            //Beim ersten Starten kommt hier noch "default" zurück
            _gameDataRepository.GetMyKey();

            _gameDataRepository.SetMyKey("HansWurst");

        }
    }
}
