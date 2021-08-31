using System;


namespace Qwirkle
{
    class Program
    {
        public static void ShowColoredLine(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        static void Main()
        {
            Game game = new();

            while(!game.IsGameOver())
            {
                for (int i = 0; i < game.NumberOfPlayer; i++)
                {
                    game.OneTurn(i);
                }
            }
            game.DisplayAll();

            game.CongratulateWinner();

        }
    }
}
