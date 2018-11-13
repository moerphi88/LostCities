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
        private string _titleLabelText;

        public int Rotation
        {
            get { return _rotation; }
            set { _rotation = value; OnPropertyChanged(); }
        }

        //public bool DialogIsVisible
        //{
        //    get { return DialogIsVisible; }
        //    set { OnPropertyChanged(); }
        //}

        public string TitleLabelText
        {
            get { return _titleLabelText; }
            set { _titleLabelText = value; OnPropertyChanged(); }
        }

        //public string SubtitleLabelText
        //{
        //    get { return TitleLabelText; }
        //    set { OnPropertyChanged(); }
        //}

        //public bool SubtitleLabelIsVisible
        //{
        //    get { return SubtitleLabelIsVisible; }
        //    set { OnPropertyChanged(); }
        //}
        //public string ButtonOneText
        //{
        //    get { return TitleLabelText; }
        //    set { OnPropertyChanged(); }
        //}
        //public bool ButtonOneIsVisible
        //{
        //    get { return TitleLabelText; }
        //    set { OnPropertyChanged(); }
        //}
        //public string ButtonTwoText
        //{
        //    get { return TitleLabelText; }
        //    set { OnPropertyChanged(); }
        //}
        //public bool ButtonTwoIsVisible
        //{
        //    get { return TitleLabelText; }
        //    set { OnPropertyChanged(); }
        //}
        //public string ButtonThreeText
        //{
        //    get { return TitleLabelText; }
        //    set { OnPropertyChanged(); }
        //}
        //public bool ButtonThreeIsVisible
        //{
        //    get { return TitleLabelText; }
        //    set { OnPropertyChanged(); }
        //}

        public PopupDialogViewModel(INavigation navigation) : base(navigation)
        {

        }
    }
}
