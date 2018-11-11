using System;
using LostCities.Model;
using LostCities.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LostCities.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CountPoints()
        {
            var sut = new AnlegestapelViewModel(null);
            sut.KarteAnlegen(new Card(Farbe.Blau.ToString(), (int)Wert.Zwei));
        }
    }
}
