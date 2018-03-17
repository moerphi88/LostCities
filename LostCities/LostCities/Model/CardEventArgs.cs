using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostCities.Model
{
    public class CardEventArgs : EventArgs
    {
        public CardEventArgs(Card card)
        {
            Card = card;
        }

        public Card Card { get; }
    }
}
