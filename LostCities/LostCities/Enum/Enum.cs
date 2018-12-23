using System;

namespace LostCities
{
    public enum Farbe { Weiss, Gruen, Blau, Gelb, Rot };

    public enum Wert { Hand = 1, Zwei = 2, Drei = 3, Vier = 4, Fuenf = 5, Sechs = 6, Sieben = 7, Acht = 8, Neun = 9, Zehn = 10};

    public enum GameStatus { Idle, PlayerOnePlayCard, PlayerOneDrawCard, PlayerTwoPlayCard, PlayerTwoDrawCard, GameOver };

    public enum Player { PlayerOne, PlayerTwo };

}
