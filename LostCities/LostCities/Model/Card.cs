namespace LostCities.Model
{
    public class Card
    {
        private string _name;
        private string _zahl;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }      

        public string Zahl
        {
            get { return _zahl; }
            set { _zahl = value; }
        }

        public Card() {
            Name = Farbe.Herz.ToString();
            Zahl = Wert.Acht.ToString();
        }

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