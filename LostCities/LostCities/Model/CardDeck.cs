using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartenspiel
{

    // Test eines Kartenstapels. Wie genau kann ich denn nu eigentlich einzelne Elemten in der Liste verändern?
    public class CardDeck
    {
        List<Card> list;
        Random random = new Random();

        private void InitCardDeck()
        {
            foreach(var value in Enum.GetNames(typeof(Farbe)))
                {
                foreach (var value2 in Enum.GetNames(typeof(Wert)))
                {
                    list.Add(new Card(value, value2));
                }
            }
        }

        public void ShuffleCardDeck()
        {
            List<Card> tempList = new List<Card>();
            tempList.AddRange(list); 

            foreach(var c in tempList)
            {
                var ran = random.Next(31);
                Card temp = list.ElementAt(ran);
                int idx = list.IndexOf(c);
                list.Remove(c);
                list.Insert(idx, temp);
                list.RemoveAt(ran);
                list.Insert(ran, c);
            }
        }

        public CardDeck()
        {
            list = new List<Card>();
            InitCardDeck();
            ShuffleCardDeck();
        }

        public bool isEmpty()
        {
            if (list.Count <= 1) return true; //Wenn nur noch eine Karte im Deck übrig ist, kann keine ganze runde mehr gespielt werden. Darum <= 1 und nicht nur 0.
            else return false;
        }

        public Card GetFirstCard()
        {
            Card c = new Card();
            try
            {
                c = list.ElementAt(0);
                list.RemoveAt(0);
                return c;

            } catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return null;
        }
    }
}