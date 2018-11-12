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

        public void SetJsonString(Card c = null)
        {
            if(null == c)
            {
                c = new Card();
            }            
            string json = JsonConvert.SerializeObject(c, Formatting.Indented);
            Debug.WriteLine(json);
            Set("json", json);
        }

        public Card GetJsonString()
        {
            Card c = JsonConvert.DeserializeObject<Card>(Get("json"));
            return c;
        }

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
    }
}
