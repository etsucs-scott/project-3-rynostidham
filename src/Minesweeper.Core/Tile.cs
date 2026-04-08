using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core;
/// <summary>
/// Represents a single tile on the Minesweeper board.
/// Stores whether it is a mine, revealed, flagged,
/// and how many adjacent mines surround it.
/// </summary>
public class Tile
{
    public bool IsMine { get; internal set; }
    public bool IsRevealed { get; internal set; }
    public bool IsFlagged { get; internal set; }
    public int AdjacentMines { get; internal set; }
}