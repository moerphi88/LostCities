using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LostCities.Service
{
    class GameDataRepository
    {
        public GameDataRepository()
        {
        }

        public void SetMyKey(string value)
        {
            Set("my_key","Hallo");
        }

        public string GetMyKey()
        {
            return Get("my_key");
        }

        private void Set(string key, string value)
        {
            Preferences.Set(key, value);
            Debug.WriteLine("Set");
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
