using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LostCities.Model
{
    public class HandCard : INotifyPropertyChanged
    {
        private Card _card;
        private bool _isEnabled;
        private bool _isVisible;

        public HandCard()
        {
            Card = new Card();
            IsVisible = true;
            IsEnabled = true;
        }

        public Card Card
        {
            get
            {
                return _card;
            }

            set
            {
                _card = value;
                //Todo Muss ich an dieser Stelle wirklich _card.ImageUri setzen? Oder könnte ich einfach OnPropertyChanged("ImageUri") auslösen?
                ImageUri = _card.ImageUri;
                IsVisible = true;
            }
        }

        public string ImageUri
        {
            get
            {
                return _card.ImageUri;
            }
            set
            {
                _card.ImageUri = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanges Handler

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

    }
}
