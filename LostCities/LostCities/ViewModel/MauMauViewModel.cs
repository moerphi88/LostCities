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
    public class MauMauViewModel : BaseViewModel, IStapel
    {
        private string _shownCardImageUri;

        //TODO Hier könnte ich ein dict einführen. Key wäre die Farbe. und value eine Liste von Cards. Nur die oberste würde angezeigt werden.
        private List<Card> _stapel;

        public Card GetTopCard()
        {
            return _stapel[_stapel.Count - 1];
        }

        public MauMauViewModel(INavigation navigation) : base(navigation)
        {
            _stapel = new List<Card>();
            ShownCardImageUri = "kartenhindergrund.png";
        }
        
        public void KarteAnlegen(Card card)
        {
            _stapel.Add(card);
            UpdateView();
        }

        private void UpdateView()
        {
            ShownCardImageUri = _stapel[_stapel.Count - 1].ImageUri;
        }

        #region Properties

        public String ShownCardImageUri
        { 
            get
            {
                return _shownCardImageUri;
            }

            set
            {
                _shownCardImageUri = value;
                OnPropertyChanged();
            }
        }

        #endregion

    }
}
