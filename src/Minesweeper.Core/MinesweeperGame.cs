namespace Minesweeper.Core;

public class MinesweeperGame
{
    public Board Board { get; }
    public GameState State { get; private set; } = GameState.InProgress;
    public int Seed { get; }
    public BoardSize Size { get; }

    private readonly DateTime _start;
    public int Moves { get; private set; }

    public MinesweeperGame(BoardSize size, int seed)
    {
        Size = size;
        Seed = seed;
        Board = new Board(size, seed);
        _start = DateTime.UtcNow;
    }

    public void Reveal(int r, int c)
    {
        if (State != GameState.InProgress) return;

        Moves++;

        bool hit = Board.Reveal(r, c);

        if (hit)
        {
            State = GameState.Lost;
            return;
        }

        if (Board.AllNonMinesRevealed())
            State = GameState.Won;
    }

    public void ToggleFlag(int r, int c)
    {
        if (State != GameState.InProgress) return;

        Moves++;
        Board.ToggleFlag(r, c);
    }

    public int GetElapsedSeconds()
        => (int)(DateTime.UtcNow - _start).TotalSeconds;
}