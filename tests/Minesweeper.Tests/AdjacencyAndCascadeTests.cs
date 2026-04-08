using Minesweeper.Core;
using Xunit;

namespace Minesweeper.Tests;

public class AdjacencyAndCascadeTests
{
    [Fact]
    public void AdjacentCounts_AreConsistentWithMines()
    {
        var board = new Board(BoardSize.Small8x8, seed: 42);

        for (int r = 0; r < board.Rows; r++)
        {
            for (int c = 0; c < board.Columns; c++)
            {
                var tile = board.GetTile(r, c);

                if (tile.IsMine)
                    continue;

                int expected = 0;
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if (dr == 0 && dc == 0) continue;
                        int nr = r + dr;
                        int nc = c + dc;
                        if (nr >= 0 && nr < board.Rows && nc >= 0 && nc < board.Columns)
                        {
                            if (board.GetTile(nr, nc).IsMine)
                                expected++;
                        }
                    }
                }

                Assert.Equal(expected, tile.AdjacentMines);
            }
        }
    }

    [Fact]
    public void RevealZeroTile_CascadesToNeighbors()
    {
        var game = new MinesweeperGame(BoardSize.Small8x8, seed: 1);

        // Find a tile with AdjacentMines == 0
        int zr = -1, zc = -1;
        for (int r = 0; r < game.Board.Rows && zr == -1; r++)
        {
            for (int c = 0; c < game.Board.Columns; c++)
            {
                var t = game.Board.GetTile(r, c);
                if (!t.IsMine && t.AdjacentMines == 0)
                {
                    zr = r;
                    zc = c;
                    break;
                }
            }
        }

        Assert.NotEqual(-1, zr); // ensure we found one

        game.Reveal(zr, zc);

        // After cascade, the zero region and its border should be revealed
        int revealed = 0;
        for (int r = 0; r < game.Board.Rows; r++)
            for (int c = 0; c < game.Board.Columns; c++)
                if (game.Board.GetTile(r, c).IsRevealed)
                    revealed++;

        Assert.True(revealed > 1);
    }
}