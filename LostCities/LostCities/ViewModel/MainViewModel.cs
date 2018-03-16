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
        public MainViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command(() => OnButtonPressed());
            Count = 0;
        }
        public ICommand OnButtonPressedCommand { get; }

        void OnButtonPressed()
        {
            Count += Count;
        }

        private string _test;
        public String Test{ 
            get
            {
                return _test;
            }

            set
            {
                _test = value;
                OnPropertyChanged();
            }
        }

        private int _count;
        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
    }
}
