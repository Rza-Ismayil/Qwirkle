using System;

namespace Qwirkle
{
    class Player
    {
        private int score;
        private string name;
        private TileList deck;

        public int Score { get => score; set => score = value; }
        public string Name { get => name; set => name = value; }
        public TileList Deck { get => deck; set => deck = value; }

        public Player(string name)
        {
            score = 0;
            this.name = name;
            this.deck = new();
        }

        public void CalculateScore(TileList sequence)
        {
            int point = sequence.List.Count;

            if (point > 1)
            {
                if (point == 4)
                    score += point;
                score += point;
            }
        }

        public void ShowScore()
        {
            Console.WriteLine("Score of {0}: {1}", name, score);
        }
    }
}
