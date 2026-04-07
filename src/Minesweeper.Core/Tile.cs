using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core
{
    public class Tile
    {
        public bool IsMine { get; internal set; }
        public bool IsRevealed { get; internal set; }
        public bool IsFlagged { get; internal set; }
        public int AdjacentMines { get; internal set; }

    }
}
