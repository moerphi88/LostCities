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

            DiscardPile.BindingContext = _mainViewModel.DiscardPileVM;
            Anlegestapel.BindingContext = _mainViewModel.AnlegeStapelVM;
            Anlegestapel2.BindingContext = _mainViewModel.AnlegeStapel2VM;

            SpielAnweisung.BindingContext = _mainViewModel.Lcgl;
            KarteZiehenButton.BindingContext = _mainViewModel.Lcgl;

            HandSpielerEins.BindingContext = _mainViewModel.HandVM;
            CreateHandView(HandSpielerEins, _mainViewModel.HandVM);

            HandSpielerZwei.BindingContext = _mainViewModel.HandVM2;           
            CreateHandView(HandSpielerZwei, _mainViewModel.HandVM2);
        }

        // Creates a button for each handcard on a players hand and adds it to the stacklayout
        private void CreateHandView(StackLayout layout, HandViewModel handViewModel)
        {
            var i = 0;
            foreach(HandCard handCard in handViewModel.HandCards)
            {
                var img = new Image
                {
                    BindingContext = handCard
                };
                img.SetBinding(IsEnabledProperty, "IsEnabled");
                img.SetBinding(IsVisibleProperty, "IsVisible");
                img.SetBinding(Image.SourceProperty, "ImageUri");

                img.WidthRequest = 100;
                img.HeightRequest = 140;
                //Creating TapGestureRecognizers https://www.c-sharpcorner.com/UploadFile/e04e9a/xamarin-forms-image-button-recipe/   
                var tapImage = new TapGestureRecognizer
                {
                    Command = handViewModel.OnButtonPressedCommand,
                    CommandParameter = i.ToString(),
                    //BindingContext = handCard
                };
                //tapImage.SetBinding(IsEnabledProperty, "IsEnabled");
                //Associating tap events to the image buttons    
                img.GestureRecognizers.Add(tapImage);
                i++;
                layout.Children.Add(img);

                //var btn = new Button
                //{
                //    BindingContext = handCard
                //};
                //btn.SetBinding(IsEnabledProperty, "IsEnabled");
                //btn.SetBinding(IsVisibleProperty, "IsVisible");
                //btn.SetBinding(Button.ImageProperty, "ImageUri");
                //btn.Command = handViewModel.OnButtonPressedCommand;
                //btn.CommandParameter = i.ToString();
                //btn.WidthRequest = 50;
                //btn.HeightRequest = 70;
                //i++;
                //// <Button WidthRequest="50" HeightRequest="70" IsVisible="{Binding IsVisibleDritteHandKarte}" Image="{Binding DritteHandKarteImageUri}" Command="{Binding OnButtonPressedCommand}" CommandParameter="2"/>
                //layout.Children.Add(btn);


            }
        }
    }
}
