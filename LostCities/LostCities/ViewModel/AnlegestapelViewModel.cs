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
    public class AnlegestapelViewModel : BaseViewModel, IStapel
    {
        public Dictionary<Farbe, List<Card>> Stapel { get; set; }

        public event EventHandler AddedCardToStack;

        public AnlegestapelViewModel(INavigation navigation) : base(navigation)
        {
            Stapel = new Dictionary<Farbe, List<Card>>
            {
                { Farbe.Weiss, new List<Card>() },
                { Farbe.Gruen, new List<Card>() },
                { Farbe.Blau, new List<Card>() },
                { Farbe.Gelb, new List<Card>() },
                { Farbe.Rot, new List<Card>()  }
            };
        }
        
        //TODO An dieser Stelle muss ich das enum Farbe auswerten anstatt einen string. Refactoring
        public void KarteAnlegen(Card card)
        {
            switch (card.Name)
            {
                case "Weiss":
                    Stapel[Farbe.Weiss].Add(card);
                    break;
                case "Gruen":
                    Stapel[Farbe.Gruen].Add(card);
                    break;
                case "Blau":
                    Stapel[Farbe.Blau].Add(card);
                    break;
                case "Gelb":
                    Stapel[Farbe.Gelb].Add(card);
                    break;
                case "Rot":
                    Stapel[Farbe.Rot].Add(card);
                    break;
                default:
                    break;
            }

            AddedCardToStack?.Invoke(this,null);
        }

        public List<Card> GetTopCards()
        {
            var list = new List<Card>();
            foreach (Farbe farbe in Enum.GetValues(typeof(Farbe))) {
                if (Stapel[farbe].Count != 0)
                {
                    list.Add(Stapel[farbe][Stapel[farbe].Count - 1]);
                }
            }
            return list;
        }
    }
}
