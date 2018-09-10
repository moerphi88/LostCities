﻿using LostCities.Service;
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
        public LostCitiesGameLogic Lcgl { get; set; }


        public MainViewModel(INavigation navigation) : base(navigation)
        {
            AblageStapelVM = new AblagestapelViewModel(null);
            //AnlegeStapelVM = new MauMauViewModel(null);
            AnlegeStapelVM = new AnlegestapelViewModel(null);
            HandVM = new HandViewModel(null);
            HandVM2 = new HandViewModel(null);
            Lcgl = new LostCitiesGameLogic(HandVM, HandVM2, AblageStapelVM, AnlegeStapelVM);

            KarteZiehenButtonText = "Karte ziehen Binding";
            AblagestapelTitleLabelText = "Ablagestapel Binding";
            AnlegestapelTitleLabelText = "Anlegestapel Binding";
            HandEinsTitleLabelText = "Hand Spieler 1";
            HandZweiTitleLabelText = "Hand Spieler 2";
            

            OnButtonPressedCommand = new Command(OnButtonPressed);
        }

        // ToDo Die Mischung Methodenaufrufe und Events gefällt mir nicht. Das sollte einmal überarbeitet werden. Für den Nachziehstapel muss ich auch ein eigenes ViewModel machen! 
        public ICommand OnButtonPressedCommand { get; }

        private void OnButtonPressed()
        {
            Lcgl.DrawHandCard();
            HandEinsTitleLabelText = "Hand Spieler 1 + ButtonClicked";
        }

        // Muss ich wirklich die Set Methode implementieren und OnPropertyCHanged aufrufen, oder gibt es eine andere Möglichkeit den Wert zu aktualisieren?! 
        #region Properties
        public String KarteZiehenButtonText { get; set; }
        public String AblagestapelTitleLabelText { get; set; }
        public String AnlegestapelTitleLabelText { get; set; }
        public String HandEinsTitleLabelText { get; set; }
        public String HandZweiTitleLabelText { get; set; }
       

        #endregion

    }
}
