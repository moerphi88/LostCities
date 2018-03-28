namespace LostCities.Model
{
    public class Card
    {
        private string _name;
        private Wert _zahl;
        private string _imageUri;

        public string ImageUri
        {
            get { return _imageUri; }
            set { _imageUri = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }      

        public Wert Zahl
        {
            get { return _zahl; }
            set { _zahl = value; }
        }

        public Card() {
            Name = Farbe.Weiss.ToString();
            Zahl = Wert.Acht;
            ImageUri = this.ToString().ToLower() + ".png";
        }

        public Card(string name, string zahl) {
            Name = name;
            Zahl = (Wert)int.Parse(zahl);
            ImageUri = this.ToString().ToLower() + ".png";
        }

        public Card(string name, int zahl)
        {
            Name = name;
            Zahl = (Wert)zahl;
            ImageUri = this.ToString().ToLower() + ".png";
        }

        public override string ToString()
        {
            return Name + "" + Zahl;
        }
    }
}