using Minesweeper.Core;

namespace Minesweeper.ConsoleApp;

public static class BoardRenderer
{
    public static void RenderBoard(Board board)
    {
        Console.WriteLine();

        // Column numbers
        Console.Write("   ");
        for (int c = 0; c < board.Columns; c++)
            Console.Write($"{c,2} ");
        Console.WriteLine();

        for (int r = 0; r < board.Rows; r++)
        {
            Console.Write($"{r,2} ");

            for (int c = 0; c < board.Columns; c++)
            {
                var t = board.GetTile(r, c);

                if (!t.IsRevealed)
                {
                    Console.Write(t.IsFlagged ? " F " : " # ");
                }
                else if (t.IsMine)
                {
                    Console.Write(" * ");
                }
                else if (t.AdjacentMines == 0)
                {
                    Console.Write(" . ");
                }
                else
                {
                    Console.Write($" {t.AdjacentMines} ");
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}