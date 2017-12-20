namespace Kartenspiel
{

    public class Card
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _zahl;

        public string Zahl
        {
            get { return _zahl; }
            set { _zahl = value; }
        }

        public Card() {
            Name = Farbe.Herz.ToString();
            Zahl = Wert.Acht.ToString();
        }

        //Was genau macht man? Benutzt man die Properties oder benutzt man die Variablen?
        public Card(string name, string zahl) {
            Name = name;
            Zahl = zahl;
        }

        public override string ToString()
        {
            return Name + "" + Zahl;
        }
    }
}