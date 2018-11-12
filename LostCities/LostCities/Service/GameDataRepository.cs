using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Newtonsoft.Json;
using LostCities.Model;

namespace LostCities.Service
{
    class GameDataRepository
    {
        public GameDataRepository()
        {
        }

        public void SetMyKey(string value)
        {
            Set("my_key",value);
        }

        public string GetMyKey()
        {
            return Get("my_key");
        }

        // Card

        public void SetJsonCard(string key,Card c = null)
        {
            if(null == c)
            {
                c = new Card();
            }            
            string json = JsonConvert.SerializeObject(c, Formatting.Indented);
            //Debug.WriteLine(json);
            Set(key, json);
        }

        public Card GetJsonCard(string key)
        {
            Card c = JsonConvert.DeserializeObject<Card>(Get(key));
            return c;
        }

        // CardList

        public void SetJsonList(string key, List<Card> cardList)
        {
            try
            {
                string json = JsonConvert.SerializeObject(cardList, Formatting.Indented);
                Set(key, json);
            } catch(ArgumentNullException e)
            {
                Debug.WriteLine("null cannot be handled here: {0}",e.Message);
            }
        }

        public List<Card> GetJsonList(string key)
        {
            List<Card> cardList = JsonConvert.DeserializeObject<List<Card>>(Get(key));
            return cardList;
        }

        // Card Dict

        public void SetJsonDict(string key, Dictionary<Farbe,List<Card>> cardDict)
        {
            try
            {
                string json = JsonConvert.SerializeObject(cardDict, Formatting.Indented);
                Set(key, json);
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine("null cannot be handled here: {0}", e.Message);
            }
        }

        public Dictionary<Farbe, List<Card>> GetJsonDict(string key)
        {
            Dictionary<Farbe, List<Card>> cardDict = JsonConvert.DeserializeObject<Dictionary<Farbe, List<Card>>>(Get(key));
            return cardDict;
        }

        // Bool

        public void SetBool(string key,bool b)
        {
            Preferences.Set(key, b);
        }

        public bool GetBool(string key)
        {
            return Preferences.Get(key, false);
        }

        // There is a game already saved

        public void SetGameSaved(bool b)
        {
            Preferences.Set("is_a_game_saved", b);
        }

        public bool GetGameSaved()
        {
            return Preferences.Get("is_a_game_saved", false);
        }

        #region private

        private void Set(string key, string value)
        {
            Preferences.Set(key, value);
            Debug.WriteLine($"Set: {value}");
        }

        private string Get(string key)
        {
            var s = Preferences.Get(key,"default");
            Debug.WriteLine($"Get value:{s} for {key} ");
            return s;         
        }

        private void Remove(string key)
        {
            Preferences.Remove(key);
            Debug.WriteLine($"Delete {key}");
        }

        private void ClearAll()
        {
            Preferences.Clear();
            Debug.WriteLine("ClearAll");
        }

        #endregion
    }
}
