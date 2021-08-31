using System;
using System.Collections.Generic;
using System.Linq;


namespace Qwirkle
{
    class Game
    {
        private readonly TileList bag;
        private readonly Board board;
        private readonly Player[] players;

        public int NumberOfPlayer { get => players.Length; }

        public Game()
        {
            int numberOfPlayer = GetPlayerCount();

            bag = new(2);

            board = new(11, 11);
            board.SetCenter(bag);
            
            players = new Player[numberOfPlayer];
            SetPlayers();
        }

        public void SetPlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                Console.Write("Player {0}, please enter your name: ", i + 1);
                string name = Console.ReadLine();
                players[i] = new(name);
                bag.RandomTake(players[i].Deck.List, 6);
            }
        }

        public static int GetPlayerCount()
        {
            int number = 0;
            while (number <= 0)
            {
                Console.Write("Hello, how many player want to play?: ");
                try
                {
                    number = int.Parse(Console.ReadLine());
                    if (number <= 0)
                    {

                        Program.ShowColoredLine(ConsoleColor.Red, "Please enter the valid format (positive integer number).");
                    }
                }
                catch
                {
                    Program.ShowColoredLine(ConsoleColor.Red, "Please enter the valid format (positive integer number).");
                }
            }
            return number;
        }

        public void DisplayAll()
        {
            Program.ShowColoredLine(ConsoleColor.Green, "\n========= Qwirkle =========");

            board.ShowBoard();

            Program.ShowColoredLine(ConsoleColor.Green, "===========================\n");

            Console.WriteLine("{0} tile left in the bag.", bag.List.Count);

            foreach (Player user in players)
            {
                user.ShowScore();
            }
        }

        public void ShowPlayerDeck(int index)
        {
            Console.Write("Deck of {0}: ", players[index].Name);
            players[index].Deck.ShowTileList();
        }

        public static bool ValidInput(string input, int[] outputs)
        {

            string[] inputs = input.Split(' ');
            if (inputs.Length != 3)
            {
                Program.ShowColoredLine(ConsoleColor.Red, "Please enter the valid format (3 positive integer  number).");
                return false;
            }

            foreach (string str in inputs)
            {
                if (!int.TryParse(str, out int number))
                {
                    Program.ShowColoredLine(ConsoleColor.Red, "Please enter the valid format (3 positive integer  number).");
                    return false;
                }
                if (number < 0)
                {
                    Program.ShowColoredLine(ConsoleColor.Red, "Please enter the valid format (3 positive integer  number).");
                    return false;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                outputs[i] = int.Parse(inputs[i]);
            }

            return true;
        }

        public void OneTurn(int index)
        {
            TileList placedTiles = new();
            LinkedList<Cell> placedCells = new();
            bool turnOver;
            bool isSequenceOver = false;
            char commonAxis = 'z';
            int firstTileCol = -1;
            int firstTileRow = -1;
            int tileIndex;
            int row;
            int col;
            while (!isSequenceOver)
            {
                turnOver = false;

                while (!turnOver)
                {
                    DisplayAll();

                    ShowPlayerDeck(index);
                    Console.WriteLine("Press \"s\" to stop placing tiles or skip turn.");
                    Console.Write("Dear {0}, please choose one tile from deck and \nits position on board in the order: TileIndex Row Column : ", players[index].Name);

                    string input = Console.ReadLine();

                    if (input.Equals("s"))
                    {
                        turnOver = true;
                        isSequenceOver = true;
                        board.CalculateScores(placedCells, commonAxis, players[index]);
                        bag.RandomTake(players[index].Deck.List, 6 - players[index].Deck.List.Count);
                        continue;
                    }

                    int[] validInputs = new int[3];
                    if (ValidInput(input, validInputs))
                    {
                        tileIndex = validInputs[0];
                        row = validInputs[1];
                        col = validInputs[2];
                    }
                    else continue;


                    Cell takenCell;

                    if (tileIndex < players[index].Deck.List.Count)
                    {
                        takenCell = new(row, col, players[index].Deck.List.ElementAt(tileIndex));
                    }
                    else
                    {
                        Program.ShowColoredLine(ConsoleColor.Red, "Out of deck index!");
                        continue;
                    }

                    if (board.IsTileAddable(takenCell))
                    {
                        placedTiles.List.AddLast(takenCell.CellTile);
                        placedCells.AddLast(takenCell);

                        if(placedTiles.List.Count == 1)
                        {
                            firstTileCol = takenCell.Col;
                            firstTileRow = takenCell.Row;
                        }
                        else if(placedTiles.List.Count == 2)
                        {
                            if(firstTileCol == takenCell.Col)
                            {
                                commonAxis = 'c';
                                
                            }
                            else if(firstTileRow == takenCell.Row)
                            {
                                commonAxis = 'r';
                            }
                            else
                            {
                                Program.ShowColoredLine(ConsoleColor.Red, "\nGame rule violation!");
                                placedTiles.List.RemoveFirst();
                                placedCells.RemoveFirst();
                                continue;
                            }
                        }
                        else
                        {
                            if (commonAxis == 'c')
                            {
                                if (takenCell.Col != firstTileCol)
                                {
                                    Program.ShowColoredLine(ConsoleColor.Red, "\nGame rule violation!");
                                    placedTiles.List.RemoveFirst();
                                    placedCells.RemoveFirst();
                                    continue;
                                }
                            }
                            else if (commonAxis == 'r')
                            {
                                if (takenCell.Row != firstTileRow)
                                {
                                    Program.ShowColoredLine(ConsoleColor.Red, "\nGame rule violation!");
                                    placedTiles.List.RemoveFirst();
                                    placedCells.RemoveFirst();
                                    continue;
                                }
                            }
                        }

                        if(!placedTiles.IsSequential())
                        {
                            Program.ShowColoredLine(ConsoleColor.Red, "\nGame rule violation!");
                            placedTiles.List.RemoveFirst();
                            placedCells.RemoveFirst();
                            continue;
                        }

                        board.AddToBoard(takenCell, players[index]);
                        players[index].Deck.List.Remove(takenCell.CellTile);
                    }
                    else continue;
                    turnOver = true;
                }
            }

        }

        public bool IsGameOver()
        {
            foreach (Player player in players)
            {
                if (player.Deck.List.Count == 0)
                    return true;
            }
            return false;
        }

        public void CongratulateWinner()
        {
            int[] scores = new int[players.Length];
            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] = players[i].Score;
            }
            Console.WriteLine("Congratulations {0}, YOU WIN",players[MaxOfArray(scores)].Name);
        }

        public static int MaxOfArray(int[] array)
        {
            int maxIndex = 0;
            for (int i = 1; i < array.Length;  i++)
            {
                if(array[i] > array[maxIndex])
                {
                    maxIndex = i;
                }
            }
            return maxIndex;
        }
    }
}
