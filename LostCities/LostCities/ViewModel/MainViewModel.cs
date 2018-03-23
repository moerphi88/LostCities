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
        public AblagestapelViewModel AblageStapelVM { get; set; }
        public IStapel AnlegeStapelVM { get; set; }
        public HandViewModel HandVM { get; set; }
        public HandViewModel HandVM2 { get; set; }
        private LostCitiesGameLogic _lcgl;

        public MainViewModel(INavigation navigation) : base(navigation)
        {
            AblageStapelVM = new AblagestapelViewModel(null);
            AnlegeStapelVM = new MauMauViewModel(null);
            HandVM = new HandViewModel(null);
            HandVM2 = new HandViewModel(null);
            _lcgl = new LostCitiesGameLogic(HandVM, HandVM2, AblageStapelVM, AnlegeStapelVM);

            OnButtonPressedCommand = new Command(OnButtonPressed);
        }

        // ToDo Die Mischung Methodenaufrufe und Events gefällt mir nicht. Das sollte einmal überarbeitet werden. Für den Nachziehstapel muss ich auch ein eigenes ViewModel machen! 
        public ICommand OnButtonPressedCommand { get; }

        private void OnButtonPressed()
        {
            _lcgl.DrawHandCard();
        }

    }
}
