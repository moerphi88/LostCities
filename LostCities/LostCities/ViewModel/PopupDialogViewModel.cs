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
    public class PopupDialogViewModel : BaseViewModel
    {
        private int _rotation;
        private bool _dialogIsVisible;

        private string _titleLabelText;

        private string _subtitleLabelText;
        private bool _subtitleLabelIsVisible;

        private bool _buttonOneIsVisible;
        private string _buttonOneText;

        private bool _buttonTwoIsVisible;
        private string _buttonTwoText;

        private bool _buttonThreeIsVisible;
        private string _buttonThreeText;


        public ICommand OnFirstButtonPressed { get; private set; }
        public ICommand OnSecondButtonPressed { get; private set; }
        public ICommand OnThirdButtonPressed { get; }

        public int Rotation
        {
            get { return _rotation; }
            set { _rotation = value; OnPropertyChanged(); }
        }

        public bool DialogIsVisible
        {
            get { return _dialogIsVisible; }
            set { _dialogIsVisible = value; OnPropertyChanged(); }
        }

        public string TitleLabelText
        {
            get { return _titleLabelText; }
            set { _titleLabelText = value; OnPropertyChanged(); }
        }

        public string SubtitleLabelText
        {
            get { return _subtitleLabelText; }
            set { _subtitleLabelText = value; OnPropertyChanged(); }
        }

        public bool SubtitleLabelIsVisible
        {
            get { return _subtitleLabelIsVisible; }
            set { _subtitleLabelIsVisible = value; OnPropertyChanged(); }
        }

        public string ButtonOneText
        {
            get { return _buttonOneText; }
            set { _buttonOneText = value; OnPropertyChanged(); }
        }

        public bool ButtonOneIsVisible
        {
            get { return _buttonOneIsVisible; }
            set { _buttonOneIsVisible = value; OnPropertyChanged(); }
        }
        
        public string ButtonTwoText
        {
            get { return _buttonTwoText; }
            set { _buttonTwoText = value; OnPropertyChanged(); }
        }

        public bool ButtonTwoIsVisible
        {
            get { return _buttonTwoIsVisible; }
            set { _buttonTwoIsVisible = value; OnPropertyChanged(); }
        }

        public string ButtonThreeText
        {
            get { return _buttonThreeText; }
            set { _buttonThreeText = value; OnPropertyChanged(); }
        }
        public bool ButtonThreeIsVisible
        {
            get { return _buttonThreeIsVisible; }
            set { _buttonThreeIsVisible = value;  OnPropertyChanged(); }
        }

        public PopupDialogViewModel(INavigation navigation) : base(navigation)
        {
            OnFirstButtonPressed = new Command(() => { DialogIsVisible = false; });
            OnSecondButtonPressed = new Command(() => { DialogIsVisible = false; });
            OnThirdButtonPressed = new Command(() => { DialogIsVisible = false; });
        }

        public void CreatePopupDialog(string title, string subtitle = null, string buttonOne = null, string buttonTwo = null, string buttonThree = null)
        {
            DialogIsVisible = true;
            TitleLabelText = title;

            if (subtitle != null)
            {
                SubtitleLabelText = subtitle;
                SubtitleLabelIsVisible = true;
            }
            else
            {
                SubtitleLabelText = string.Empty;
                SubtitleLabelIsVisible = false;
            }

            if (buttonOne != null)
            {
                ButtonOneText = buttonTwo;
                ButtonOneIsVisible = true;
            }
            else
            {
                ButtonOneText = string.Empty;
                ButtonOneIsVisible = false;
            }

            if (buttonTwo != null){
                ButtonTwoText = buttonTwo;
                ButtonTwoIsVisible = true;
            }
            else {
                ButtonTwoText = string.Empty;
                ButtonTwoIsVisible = false;
            }

            if (buttonThree != null)
            {
                ButtonThreeText = buttonTwo;
                ButtonThreeIsVisible = true;
            }
            else
            {
                ButtonThreeText = string.Empty;
                ButtonThreeIsVisible = false;
            }            
        }
    }
}
