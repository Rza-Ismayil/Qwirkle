using System;
using System.Collections.Generic;
using System.Linq;

namespace Qwirkle
{
    class TileList
    {
        private LinkedList<Tile> list;

        public LinkedList<Tile> List { get => list; set => list = value; }

        public TileList(int variant)
        {
            char[] letters = { 'A', 'B', 'C', 'D' };
            ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.DarkYellow };

            list = new();

            for (int i = 0; i < 16 * variant; i++)
            {
                list.AddFirst(new Tile(colors[(i / 4) % 4], letters[i % 4]));
            }

        }

        public TileList()
        {
            list = new();
        }

        public void ShowTileList()
        {
            foreach (Tile tile in list)
            {
                tile.ShowTile();
            }
            Console.WriteLine();
        }

        public void RandomTake(LinkedList<Tile> target, int numberOfTile)
        {
            Random _random = new();
            int randIndex;

            for (int i = 0; i < numberOfTile; i++)
            {
                if (list.Count == 0)
                {
                    return;
                }
                randIndex = _random.Next(list.Count);
                Tile selectedTile = list.ElementAt(randIndex);
                list.Remove(selectedTile);
                target.AddFirst(selectedTile);
            }
        }

        public bool IsSequential()
        {
            if(list.Count > 4)
            {
                return false;
            }
            if(list.Count == 1)
            {
                return true;
            }
            bool isColorSame = true;
            bool isLetterSame = true;

            for(int i = 0; i < list.Count - 1; i++)
            {
                Tile element = list.ElementAt(i);
                Tile nextElement = list.ElementAt(i+1);
                if (element.Color != nextElement.Color)
                {
                    isColorSame = false;
                }
                if(element.Letter != nextElement.Letter)
                {
                    isLetterSame = false;
                }
            }

            for(int i = 0; i< list.Count -1; i++)
            {
                Tile element1 = list.ElementAt(i);
                for (int j = i + 1; j < list.Count; j++)
                {
                    Tile element2 = list.ElementAt(j);

                    if(element1.Color == element2.Color && element1.Letter == element2.Letter)
                    {
                        return false;
                    }
                }
            }

            return (isLetterSame != isColorSame);
        }
    }
}
