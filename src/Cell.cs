using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwirkle
{
    class Cell
    {
        private int row;
        private int col;
        private Tile cellTile = new(ConsoleColor.White, '_');

        public int Row { get => row; set => row = value; }
        public int Col { get => col; set => col = value; }
        public Tile CellTile { get => cellTile; set => cellTile = value; }


        public Cell(int row, int col, Tile cellTile)
        {
            this.row = row;
            this.col = col;
            this.cellTile = cellTile;
        }
    }
}
