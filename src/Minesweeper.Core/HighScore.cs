namespace Minesweeper.Core;
/// <summary>
/// Represents a single high score entry
/// Stores baord size, time, moves, and seed.
/// </summary>
public class HighScore
{
    public BoardSize Size { get; set; }
    public int Seconds { get; set; }
    public int Moves { get; set; }
    public int Seed { get; set; }
    public DateTime Timestamp { get; set; }

    public override string ToString()
        => $"{Size},{Seconds},{Moves},{Seed},{Timestamp:o}";
}