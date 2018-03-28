using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LostCities.Model
{

    // Test eines Kartenstapels. Wie genau kann ich denn nu eigentlich einzelne Elemten in der Liste verändern?
    public class CardDeck
    {
        private List<Card> _list;
        Random random = new Random();

        private void InitCardDeck()
        {
            foreach(var value in Enum.GetNames(typeof(Farbe)))
                {
                foreach (int value2 in Enum.GetValues(typeof(Wert)))
                {
                    _list.Add(new Card(value, value2));
                }
            }
        }

        private void ShuffleCardDeck()
        {
            List<Card> tempList = new List<Card>();
            tempList.AddRange(_list); 

            foreach(var c in tempList)
            {
                var ran = random.Next(31);
                Card temp = _list.ElementAt(ran);
                int idx = _list.IndexOf(c);
                _list.Remove(c);
                _list.Insert(idx, temp);
                _list.RemoveAt(ran);
                _list.Insert(ran, c);
            }
        }

        public CardDeck()
        {
            _list = new List<Card>();
            InitCardDeck();
            ShuffleCardDeck();
        }

        public bool IsEmpty()
        {
            if (_list.Count <= 1) return true; //Wenn nur noch eine Karte im Deck übrig ist, kann keine ganze runde mehr gespielt werden. Darum <= 1 und nicht nur 0.
            else return false;
        }

        public Card GetFirstCard()
        {
            try
            {
                var c = _list.ElementAt(0);
                _list.RemoveAt(0);
                return c;

            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public List<Card> GetXCards(int anzahlKarten)
        {
            try
            {
                var list = new List<Card>();
                for (int i = 0; i < anzahlKarten; i++)
                {
                    list.Add(_list.ElementAt(0));
                    _list.RemoveAt(0);
                }
                return list;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }
    }
}