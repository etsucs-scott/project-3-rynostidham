using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Core;
/// <summary>
/// Provides board dimensions and mine counts
/// for each board size.
/// </summary>

public static class BoardConfig
{
    public static (int rows, int cols, int mines) GetConfig(BoardSize size)
    {
        return size switch
        {
            BoardSize.Small8x8 => (8, 8, 10),
            BoardSize.Medium12x12 => (12, 12, 25),
            BoardSize.Large16x16 => (16, 16, 40),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
