using Minesweeper.Core;
using Xunit;

namespace Minesweeper.Tests;

public class BoardTests
{
    [Fact]
    public void SmallBoard_HasCorrectDimensionsAndMineCount()
    {
        var board = new Board(BoardSize.Small8x8, seed: 123);

        Assert.Equal(8, board.Rows);
        Assert.Equal(8, board.Columns);

        int mines = 0;
        for (int r = 0; r < board.Rows; r++)
            for (int c = 0; c < board.Columns; c++)
                if (board.GetTile(r, c).IsMine)
                    mines++;

        Assert.Equal(10, mines);
    }

    [Fact]
    public void MediumBoard_HasCorrectMineCount()
    {
        var board = new Board(BoardSize.Medium12x12, seed: 123);

        int mines = 0;
        for (int r = 0; r < board.Rows; r++)
            for (int c = 0; c < board.Columns; c++)
                if (board.GetTile(r, c).IsMine)
                    mines++;

        Assert.Equal(25, mines);
    }

    [Fact]
    public void LargeBoard_HasCorrectMineCount()
    {
        var board = new Board(BoardSize.Large16x16, seed: 123);

        int mines = 0;
        for (int r = 0; r < board.Rows; r++)
            for (int c = 0; c < board.Columns; c++)
                if (board.GetTile(r, c).IsMine)
                    mines++;

        Assert.Equal(40, mines);
    }

    [Fact]
    public void SameSeed_ProducesSameMineLayout()
    {
        var b1 = new Board(BoardSize.Small8x8, seed: 999);
        var b2 = new Board(BoardSize.Small8x8, seed: 999);

        for (int r = 0; r < b1.Rows; r++)
        {
            for (int c = 0; c < b1.Columns; c++)
            {
                Assert.Equal(b1.GetTile(r, c).IsMine, b2.GetTile(r, c).IsMine);
            }
        }
    }
}