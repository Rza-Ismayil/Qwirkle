using System;
using System.Collections.Generic;
using System.Linq;

namespace Qwirkle
{
    class Board
    {
        private int row;
        private int col;
        private Cell [,] table;

        public int Row { get => row; set => row = value; }
        public int Col { get => col; set => col = value; }
        public Cell [,] Table { get => table; set => table = value; }

        public Board(int row, int col)
        {

            this.row = row;
            this.col = col;
            table = new Cell[row, col];
            for(int i=0; i<row; i++)
            {
                for(int j=0; j<col; j++)
                {
                    table[i, j] = new(i, j, new(ConsoleColor.White, '—'));
                }
            }

        }

        public void SetCenter(TileList bag)
        {
            Random _random = new();
            int randIndex = _random.Next(bag.List.Count);
            Tile randomTile = bag.List.ElementAt(randIndex);
            bag.List.Remove(randomTile);
            table[5, 5] = new(5, 5, randomTile);
        }

        public void ShowBoard()
        {
            Console.Write("  ");
            for (int i = 0; i < row; i++)
            {
                Console.Write(' ' + i.ToString());
            }
            Console.WriteLine();
            for (int i = 0; i < row; i++)
            {
                Console.Write(i.ToString() + ' ');
                if (i < 10)
                    Console.Write(' ');
                for (int j = 0; j < col; j++)
                {
                    table[i, j].CellTile.ShowTile();
                }
                Console.WriteLine();
            }
        }

        public bool BelongToBoard(int _row, int _col)
        {
            if (_row >= 0 && _col >= 0 && _row < row && _col < col)
            {
                return true;
            }
            return false;
        }

        public bool IsCellEmpty(Cell cell)
        {
            if (BelongToBoard(cell.Row, cell.Col))
            {
                if (table[cell.Row, cell.Col].CellTile.Letter == '—')
                {
                    return true;
                }
                Program.ShowColoredLine(ConsoleColor.Red, "Cell is already full! ");
            }
            else
            {
                Program.ShowColoredLine(ConsoleColor.Red, "Out of board borders! ");
            }
            return false;
        }

        public bool IsTileAddable(Cell cell)
        {
            if (IsCellEmpty(cell) && HasNeighbors(cell))
            {
                TileList colSequence = CollectSequenceC(cell);
                TileList rowSequence = CollectSequenceR(cell);

                if (colSequence.IsSequential() && rowSequence.IsSequential())
                {
                    return true;
                }
                else
                {
                    Program.ShowColoredLine(ConsoleColor.Red, "Game rule violation!");
                    return false;
                }
            }
            else
            {
                Program.ShowColoredLine(ConsoleColor.Red, "Can't add to board!");
                return false;
            }
        }

        public void AddToBoard(Cell cell, Player player)
        {

            TileList colSequence = CollectSequenceC(cell);
            TileList rowSequence = CollectSequenceR(cell);
            table[cell.Row, cell.Col].CellTile = cell.CellTile;
        }

        public void CalculateScores(LinkedList<Cell> placedCells, char commonAxis, Player player)
        {
            if (commonAxis == 'c')
            {
                TileList colSequence = CollectSequenceC(placedCells.First());
                player.CalculateScore(colSequence);

                foreach (Cell cell in placedCells)
                {
                    TileList rowSequence = CollectSequenceR(cell);
                    player.CalculateScore(rowSequence);
                }
            }
            else
            {
                TileList rowSequence = CollectSequenceR(placedCells.First());
                player.CalculateScore(rowSequence);

                foreach (Cell cell in placedCells)
                {
                    TileList colSequence = CollectSequenceC(cell);
                    player.CalculateScore(colSequence);
                }
            }
        }

        public bool HasNeighbors(Cell cell)
        {
            int _row = cell.Row;
            int _col = cell.Col;
            if (BelongToBoard(_row + 1, _col))
            {
                if (table[_row + 1, _col].CellTile.Letter != '—')
                {
                    return true;
                }
            }
            if (BelongToBoard(_row - 1, _col))
            {
                if (table[_row - 1, _col].CellTile.Letter != '—')
                {
                    return true;
                }
            }
            if (BelongToBoard(_row, _col + 1))
            {
                if (table[_row, _col + 1].CellTile.Letter != '—')
                {
                    return true;
                }
            }
            if (BelongToBoard(_row, _col - 1))
            {
                if (table[_row, _col - 1].CellTile.Letter != '—')
                {
                    return true;
                }
            }
            Program.ShowColoredLine(ConsoleColor.Red, "The Cell should have at least one neighbor!");
            return false;
        }

        public TileList CollectSequenceC(Cell cell)
        {
            TileList result = new();
            result.List.AddFirst(cell.CellTile);

            for (int i = cell.Row + 1; i < row; i++)
            {
                if (table[i, cell.Col].CellTile.Letter == '—')
                    break;
                result.List.AddLast(table[i, cell.Col].CellTile);
            }

            for (int i = cell.Row - 1; i >= 0; i--)
            {
                if (table[i, cell.Col].CellTile.Letter == '—')
                    break;
                result.List.AddFirst(table[i, cell.Col].CellTile);
            }
            return result;
        }

        public TileList CollectSequenceR(Cell cell)
        {
            TileList result = new();
            result.List.AddFirst(cell.CellTile);

            for (int i = cell.Col + 1; i < col; i++)
            {
                if (table[cell.Row, i].CellTile.Letter == '—')
                    break;
                result.List.AddLast(table[cell.Row, i].CellTile);
            }

            for (int i = cell.Col -1; i >= 0; i--)
            {
                if (table[cell.Row, i].CellTile.Letter == '—')
                    break;
                result.List.AddFirst(table[cell.Row, i].CellTile);
            }
            return result;
        }

    }
}
