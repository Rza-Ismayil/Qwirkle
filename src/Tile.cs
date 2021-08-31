using System;

namespace Qwirkle
{
    class Tile
    {
        private ConsoleColor color;
        private char letter;

        public ConsoleColor Color { get => color; set => color = value; }
        public char Letter { get => letter; set => letter = value; }


        public Tile(ConsoleColor color, char letter)
        {
            this.color = color;
            this.letter = letter;
        }

        public void ShowTile()
        {
            Console.ForegroundColor = color;
            Console.Write(letter + " ");
            Console.ResetColor();
        }
    }
}
