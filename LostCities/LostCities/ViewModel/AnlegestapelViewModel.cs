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
using LostCities.Service;

namespace LostCities.ViewModel
{
    public class AnlegestapelViewModel : BaseViewModel, IStapel
    {
        private GameDataRepository _gameDataRepository;

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

            _gameDataRepository = new GameDataRepository();
        }
        
        public int CountPoints()
        {
            var points = 0;
            foreach(KeyValuePair<Farbe,List<Card>> entry in Stapel)
            {
                points += CalculatePointsForOneStack(entry.Value);
            }
            return points;
        }

        private int CalculatePointsForOneStack(List<Card> list)
        {
            var points = -20;
            var factor = 1;
            foreach(var c in list)
            {
                if (c.Zahl == Wert.Hand)
                {
                    factor++;
                } else
                {
                    points += (int)c.Zahl;
                }                
            }
            points *= factor;
            if (points == -20) return 0; //In this case the stack is empty. No cards of that colour played
            else return points;
        }

        //TODO An dieser Stelle muss ich das enum Farbe auswerten anstatt einen string. Refactoring. Außerdem muss hier die Logik eigentlich schon rein, dass nur aufsteigend angelegt werden kann. Als neueKarte. > alteKarte.Wert
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

        public void PersistStapel(string key)
        {
            _gameDataRepository.SetJsonDict(key, Stapel);
        }

        public void GetStapelCardsFromPersistency(string key)
        {
            Stapel = _gameDataRepository.GetJsonDict(key);
        }
    }
}
