using Minesweeper.Core;
using Xunit;

namespace Minesweeper.Tests;

public class WinLossTests
{
    [Fact]
    public void HittingMine_SetsStateToLost()
    {
        var game = new MinesweeperGame(BoardSize.Small8x8, seed: 5);

        // Find a mine
        int mr = -1, mc = -1;
        for (int r = 0; r < game.Board.Rows && mr == -1; r++)
        {
            for (int c = 0; c < game.Board.Columns; c++)
            {
                if (game.Board.GetTile(r, c).IsMine)
                {
                    mr = r;
                    mc = c;
                    break;
                }
            }
        }

        Assert.NotEqual(-1, mr);

        game.Reveal(mr, mc);

        Assert.Equal(GameState.Lost, game.State);
    }

    [Fact]
    public void RevealingAllNonMines_SetsStateToWon()
    {
        var game = new MinesweeperGame(BoardSize.Small8x8, seed: 7);

        // Reveal all non-mine tiles
        for (int r = 0; r < game.Board.Rows; r++)
        {
            for (int c = 0; c < game.Board.Columns; c++)
            {
                var t = game.Board.GetTile(r, c);
                if (!t.IsMine)
                {
                    game.Reveal(r, c);
                }
            }
        }

        Assert.Equal(GameState.Won, game.State);
    }

    [Fact]
    public void FlaggedTile_CannotBeRevealed()
    {
        var game = new MinesweeperGame(BoardSize.Small8x8, seed: 10);

        game.ToggleFlag(0, 0);
        var before = game.Board.GetTile(0, 0).IsRevealed;

        game.Reveal(0, 0);

        var after = game.Board.GetTile(0, 0).IsRevealed;

        Assert.False(before);
        Assert.False(after);
    }
}