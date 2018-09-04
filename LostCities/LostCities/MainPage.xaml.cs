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
        private MainViewModel _mainViewModel;

        public MainPage()
        {
            InitializeComponent();

            _mainViewModel = new MainViewModel(this.Navigation);
            BindingContext = _mainViewModel;

            Ablegestapel.BindingContext = _mainViewModel.AblageStapelVM;
            Anlegestapel.BindingContext = _mainViewModel.AnlegeStapelVM;
            HandSpielerEins.BindingContext = _mainViewModel.HandVM;
            HandSpielerZwei.BindingContext = _mainViewModel.HandVM2;

            CreateHandView(HandSpielerEins, _mainViewModel.HandVM);
            CreateHandView(HandSpielerZwei, _mainViewModel.HandVM2);
        }

        // Creates a button for each 
        private void CreateHandView(StackLayout layout, HandViewModel handViewModel)
        {
            var i = 0;
            foreach(HandCard handCard in handViewModel.HandCards)
            {
                var btn = new Button
                {
                    BindingContext = handCard
                };
                btn.SetBinding(IsEnabledProperty, "IsEnabled");
                btn.SetBinding(IsVisibleProperty, "IsVisible");
                btn.SetBinding(Button.ImageProperty, "ImageUri");
                btn.Command = handViewModel.OnButtonPressedCommand;
                btn.CommandParameter = i.ToString();
                btn.WidthRequest = 50;
                btn.HeightRequest = 70;
                i++;
                // <Button WidthRequest="50" HeightRequest="70" IsVisible="{Binding IsVisibleDritteHandKarte}" Image="{Binding DritteHandKarteImageUri}" Command="{Binding OnButtonPressedCommand}" CommandParameter="2"/>
                layout.Children.Add(btn);
            }
        }
    }
}
