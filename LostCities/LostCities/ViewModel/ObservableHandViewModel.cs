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
using System.Collections.ObjectModel;

namespace LostCities.ViewModel
{
    public class ObservableHandViewModel : BaseViewModel
    {
        //Gibt es hier vllt. eine andere Lösung. Ich möchte ja eine feste Länge haben. Oder lösche ich die View-Elemente je nach Inhalt dieser Liste?! 
        private ObservableCollection<HandCard> _cardList;

        public delegate void CardEventHandler(object sender, CardEventArgs e);
        public event CardEventHandler KarteAnlegen;
        public event CardEventHandler KarteAblegen;

        public ObservableHandViewModel(INavigation navigation) : base(navigation)
        {
            OnButtonPressedCommand = new Command<string>(OnButtonPressed);
            _cardList = new ObservableCollection<HandCard>();
            for(int i = 0; i < 3; i++)
            {
                _cardList.Add(new HandCard { ImageUri = null, IsVisible = false });
            }
        }

        protected virtual void OnKarteAnlegen(CardEventArgs e)
        {
            KarteAnlegen?.Invoke(this, e);
        }

        protected virtual void OnKarteAblegen(CardEventArgs e)
        {
            KarteAblegen?.Invoke(this, e);
        }

        public ICommand OnButtonPressedCommand { get; }

        async void OnButtonPressed(string value)
        {
            var buttons = new String[] { "Karte ablegen", "Karte anlegen" };
            var answer = await App.Current.MainPage.DisplayActionSheet("Initialrunde beendet!", null, "Cancel",buttons );
            //todo hier die Antworten des ActionsSheets abfragen und Aktion ausführen.
            
            if (null != answer)
            {
                if (answer != "Cancel")
                {
                    //Hier muss try-catch etc. noch hin. Und der Index muss überprüft werden
                    var index = int.Parse(value);
                    var CardEventArgs = new CardEventArgs(new Card { Name = "Herz", Zahl = "Acht" }); //_cardList[index]); TODO Das Model HandCard muss eine Card enthalten anstatt nur der ImageUri 
                    _cardList.RemoveAt(index);
                    UpdateView(index);

                    switch (answer)
                    {
                        case "Karte anlegen":
                            OnKarteAnlegen(new CardEventArgs(null));
                            break;
                        case "Karte ablegen":
                            OnKarteAblegen(CardEventArgs);
                            break;
                    }
                }
            }

            
            //switch (value)
            //{
            //    case "0":
            //        gelberstapelimageuri = new card(farbe.herz.tostring(), wert.dame.tostring()).imageuri;
            //        break;
            //    case "1":
            //        gruenerstapelimageuri = new card(farbe.karo.tostring(), wert.dame.tostring()).imageuri;
            //        break;
            //    case "2":
            //        roterstapelimageuri = new card(farbe.karo.tostring(), wert.dame.tostring()).imageuri;
            //        break;
            //    default:
            //        break;
            //}
        }

        public void GetHandCards(List<Card> cardList)
        {            
            try
            {
                if (cardList.Count != _cardList.Count)
                {
                    //todo Es ist nicht sichergestellt, dass keine IndexOutOfBoundException geworfen wird
                }
                else
                {
                    var i = 0;
                    foreach (Card card in cardList)
                    {
                        _cardList[i].ImageUri = card.ImageUri;
                        _cardList[i].IsVisible = true; //todo Setzen des IsVisible müsste anhand einer Validierung von ImageUri im Model passieren
                        i++;
                    }
                }
                //UpdateViewModel();
            } catch (IndexOutOfRangeException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
              
        //private void UpdateViewModel()
        //{
        //    try
        //    {
        //        ErsteHandKarteImageUri = _cardList[0].ImageUri;
        //        ZweiteHandKarteImageUri = _cardList[1].ImageUri;
        //        DritteHandKarteImageUri = _cardList[2].ImageUri;
        //    }
        //    catch (IndexOutOfRangeException e)
        //    {
        //        Debug.WriteLine(e.Message);
        //    }
        //}

        private void UpdateView(int value)
        {
            _cardList[value].ImageUri = null;
            _cardList[value].IsVisible = false;
        }
    }
}
