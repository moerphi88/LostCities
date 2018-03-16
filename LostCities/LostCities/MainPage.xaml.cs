using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using LostCities.ViewModel;

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
            stack2.BindingContext = vm.HandVM;
        }
    }
}
