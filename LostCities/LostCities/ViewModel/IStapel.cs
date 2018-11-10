using LostCities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostCities.ViewModel
{
    public interface IStapel
    {
        void KarteAnlegen(Card card);
        List<Card> GetTopCards();
        event EventHandler AddedCardToStack;
        int CountPoints();
    }
}
