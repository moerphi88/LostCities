namespace LostCities.Model
{
    public class Card
    {
        private string _name;
        private string _zahl;
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

        public string Zahl
        {
            get { return _zahl; }
            set { _zahl = value; }
        }

        public Card() {
            Name = Farbe.Herz.ToString();
            Zahl = Wert.Acht.ToString();
            ImageUri = this.ToString().ToLower() + ".png";
        }

        public Card(string name, string zahl) {
            Name = name;
            Zahl = zahl;
            ImageUri = this.ToString().ToLower() + ".png";
        }

        public override string ToString()
        {
            return Name + "" + Zahl;
        }
    }
}