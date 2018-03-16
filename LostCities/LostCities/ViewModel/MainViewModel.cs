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
        public AblagestapelViewModel AblageStapelVM { get; set; }
        public AblagestapelViewModel AblageStapelVM2 { get; set; }

        public MainViewModel(INavigation navigation) : base(navigation)
        {
            AblageStapelVM = new AblagestapelViewModel(null);
            AblageStapelVM2 = new AblagestapelViewModel(null);
        }
    }
}
