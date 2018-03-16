using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace LostCities.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {        
        public INavigation _navigation { get; }   

        public BaseViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }
        
        public virtual void Update()
        {
        }

        #region INotifyPropertyChanges Handler

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        #endregion

    }

}
