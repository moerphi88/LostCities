using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using LostCities.ViewModel;
using LostCities.Model;
using LostCities.Service;

namespace LostCities
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel vm;

        public MainPage()
        {
            InitializeComponent();
            vm = new MainViewModel(this.Navigation);
            BindingContext = vm;

            stack.BindingContext = vm.AblageStapelVM;
            HandSpielerEins.BindingContext = vm.HandVM;
            HandSpielerZwei.BindingContext = vm.HandVM2;
        }
    }
}
