using System;
using LostCities.Model;
using LostCities.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LostCities.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private AnlegestapelViewModel GetAnlegestapelWithCards(Farbe farbe, int anzahl, AnlegestapelViewModel vm = null)
        {
            AnlegestapelViewModel sut;
            if (null != vm) sut = vm;
            else sut = new AnlegestapelViewModel(null);
            int[] values = (int[])Enum.GetValues(typeof(Wert));
            for (int i = 1; i < anzahl+1; i++)
            {
                sut.KarteAnlegen(new Card(farbe.ToString(), values[i]));
            }
            return sut;
        }

        [TestMethod]
        public void CountPointsToBeMinus18()
        {
            var sut = GetAnlegestapelWithCards(Farbe.Blau, 1);
            Assert.IsTrue(sut.CountPoints() == -18);
        }

        [TestMethod]
        public void CountPointsToBeStillZero()
        {
            var sut = GetAnlegestapelWithCards(Farbe.Blau, 5);

            Assert.IsTrue(sut.CountPoints() == 0);
        }

        [TestMethod]
        public void CountPointsToBe34()
        {
            var sut = GetAnlegestapelWithCards(Farbe.Blau, 9);

            Assert.IsTrue(sut.CountPoints() == 34);
        }

        [TestMethod]
        public void CountPointsToBe68()
        {
            var sut = GetAnlegestapelWithCards(Farbe.Blau, 9);
            sut = GetAnlegestapelWithCards(Farbe.Weiss, 9, sut);

            Assert.IsTrue(sut.CountPoints() == 68);
        }

        [TestMethod]
        public void CountPointsToBe16()
        {
            var sut = GetAnlegestapelWithCards(Farbe.Blau, 9);
            sut = GetAnlegestapelWithCards(Farbe.Weiss, 1, sut);

            Assert.IsTrue(sut.CountPoints() == 16);
        }

        [TestMethod]
        public void CountPointsWhenOnlyHandsArePlayed()
        {
            var sut = new AnlegestapelViewModel(null);

            sut.KarteAnlegen(new Card(Farbe.Blau.ToString(), (int)Wert.Hand));          
            Assert.IsTrue(sut.CountPoints() == -40);

            sut.KarteAnlegen(new Card(Farbe.Blau.ToString(), (int)Wert.Hand));
            Assert.IsTrue(sut.CountPoints() == -60);

            sut.KarteAnlegen(new Card(Farbe.Blau.ToString(), (int)Wert.Hand));
            Assert.IsTrue(sut.CountPoints() == -80);
        }

        [TestMethod]
        public void CountPointsToBe48()
        {
            var sut = new AnlegestapelViewModel(null);

            sut.KarteAnlegen(new Card(Farbe.Blau.ToString(), (int)Wert.Hand));
            sut = GetAnlegestapelWithCards(Farbe.Blau, 8, sut);

            Assert.IsTrue(sut.CountPoints() == 48);
        }

        [TestMethod]
        public void CountPointsToBe136()
        {
            var sut = new AnlegestapelViewModel(null);

            sut.KarteAnlegen(new Card(Farbe.Blau.ToString(), (int)Wert.Hand));
            sut = GetAnlegestapelWithCards(Farbe.Blau, 9, sut);

            sut.KarteAnlegen(new Card(Farbe.Weiss.ToString(), (int)Wert.Hand));
            sut = GetAnlegestapelWithCards(Farbe.Weiss, 9, sut);

            Assert.IsTrue(sut.CountPoints() == 136);
        }
    }
}
