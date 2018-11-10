using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using LostCities.ViewModel;
using LostCities.Model;
using LostCities.Service;
using System.Diagnostics;

namespace LostCities
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _mainViewModel;
        private double _cardWidth = 75.0;
        private double _cardHeight = 105.0;

        public MainPage()
        {
            InitializeComponent();

            _mainViewModel = new MainViewModel(this.Navigation);
            BindingContext = _mainViewModel;

            DiscardPile.BindingContext = _mainViewModel.DiscardPileVM;
            Anlegestapel.BindingContext = _mainViewModel.AnlegeStapelVM;
            Anlegestapel2.BindingContext = _mainViewModel.AnlegeStapel2VM;

            _mainViewModel.AnlegeStapelVM.AddedCardToStack += OnAddedCardToStack;
            _mainViewModel.AnlegeStapel2VM.AddedCardToStack += OnAddedCardToStack;

            //SpielAnweisung.BindingContext = _mainViewModel.Lcgl;
            KarteZiehenButton.BindingContext = _mainViewModel.Lcgl;

            HandSpielerEins.BindingContext = _mainViewModel.HandVM;
            CreateHandView(HandSpielerEins, _mainViewModel.HandVM);

            HandSpielerZwei.BindingContext = _mainViewModel.HandVM2;
            CreateHandView(HandSpielerZwei, _mainViewModel.HandVM2);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            //Hier kommen die Werte abhängig von landscape und portrait rein. Wie kann ich das ändern bzw. die Werte normieren? 1288 * 776
            var screenWidth = width;
            var screenHeight = height;
            var orientation = width > height ? "landscape" : "portrait";

            _cardWidth = ((screenWidth / 2) - 55) / 8;//orientation == "landscape" ? ((screenWidth) / 2 / 8) : ((screenWidth) / 8);
            _cardHeight = _cardWidth * 1.4;

            var stackWidth = _cardWidth * 8 + 20; //cardWidth * no. Cards * Padding left and right
            var handStackHeight = _cardHeight;
            var discardStackHeight = _cardWidth;
            var pointStackHeight = _cardHeight + 9 * _cardHeight / 10;

            UpdateSize(HandSpielerEins);
            UpdateSize(HandSpielerZwei);
            UpdateSize(DiscardPile);

            KarteZiehenButton.WidthRequest = _cardWidth;
            KarteZiehenButton.HeightRequest = _cardHeight;
        }

        private void UpdateSize(StackLayout stack)
        {
            foreach(var child in stack.Children)
            {
                Image image = (Image)child;

                image.WidthRequest = _cardWidth;
                image.HeightRequest = _cardHeight;
            }
        }

        private void OnAddedCardToStack(object sender, EventArgs e)
        {
            CreateAnlegeStapelView();
        }

        private void CreateAnlegeStapelView()
        {
            var vm = (AnlegestapelViewModel)_mainViewModel.AnlegeStapelVM;
            AddCardsToStapelView(AbsoluteLayoutName, (AnlegestapelViewModel)_mainViewModel.AnlegeStapelVM);
        }

        private void AddCardsToStapelView(AbsoluteLayout layout, AnlegestapelViewModel vm)
        {
            //https://docs.microsoft.com/de-de/xamarin/xamarin-forms/user-interface/layouts/absolute-layout 

            double horizontalExtraSpacing = _cardWidth + 50;

            var horizontalOffsetCnt = 0;
            foreach (KeyValuePair<Farbe, List<Card>> entry in vm.Stapel)
            {
                var verticalOffsetCnt = 0;
                foreach (var c in entry.Value)
                {
                    var image = new Image { Source = c.ImageUri };
                    AbsoluteLayout.SetLayoutBounds(image, new Rectangle(horizontalExtraSpacing + (horizontalOffsetCnt * _cardWidth) + 5, verticalOffsetCnt * .17 * _cardHeight, _cardWidth, _cardHeight));
                    layout.Children.Add(image);
                    verticalOffsetCnt++;
                }
                horizontalOffsetCnt++;
            }
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
                img.WidthRequest = _cardWidth;
                img.HeightRequest = _cardHeight;
                //Creating TapGestureRecognizers https://www.c-sharpcorner.com/UploadFile/e04e9a/xamarin-forms-image-button-recipe/   
                var tapImage = new TapGestureRecognizer
                {
                    Command = handViewModel.OnButtonPressedCommand,
                    CommandParameter = i.ToString()
                };
                //Associating tap events to the image buttons    
                img.GestureRecognizers.Add(tapImage);
                i++;
                layout.Children.Add(img);
            }
        }
    }
}
