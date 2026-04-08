using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Minesweeper.Core;
/// <summary>
/// Loads, saves, and sorts high scores
/// In CSV file
/// Keeps only top 5 scores per board size.
/// </summary>
public class HighScoreManager
{
    /// <summary>
    /// Loads all valid high score entires from CSV file.
    /// </summary>
    private readonly string _filePath;

    public HighScoreManager(string filePath)
    {
        _filePath = filePath;
    }

    public List<HighScore> Load()
    {
        /// <summary>
        /// Adds a new score, sorts the list.
        /// writes the top 5 into CSV file.
        /// </summary>
        var scores = new List<HighScore>();

        if (!File.Exists(_filePath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
                File.WriteAllText(_filePath, "size,seconds,moves,seed,timestamp\n");
            }
            catch
            {
                return scores;
            }
        }

        try
        {
            var lines = File.ReadAllLines(_filePath).Skip(1);

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length != 5)
                    continue;

                if (!Enum.TryParse(parts[0], out BoardSize size))
                    continue;

                if (!int.TryParse(parts[1], out int seconds))
                    continue;

                if (!int.TryParse(parts[2], out int moves))
                    continue;

                if (!int.TryParse(parts[3], out int seed))
                    continue;

                if (!DateTime.TryParse(parts[4], null, DateTimeStyles.RoundtripKind, out DateTime timestamp))
                    continue;

                scores.Add(new HighScore
                {
                    Size = size,
                    Seconds = seconds,
                    Moves = moves,
                    Seed = seed,
                    Timestamp = timestamp
                });
            }
        }
        catch
        {
            return new List<HighScore>();
        }

        return scores;
    }

    public void AddScore(HighScore score)
    {
        var scores = Load();
        scores.Add(score);

        // Keep top 5 per board size
        var filtered = scores
            .Where(s => s.Size == score.Size)
            .OrderBy(s => s.Seconds)
            .ThenBy(s => s.Moves)
            .Take(5)
            .ToList();

        // Keep other sizes untouched
        var others = scores.Where(s => s.Size != score.Size).ToList();

        var final = others.Concat(filtered).ToList();

        Save(final);
    }

    private void Save(List<HighScore> scores)
    {
        try
        {
            using var writer = new StreamWriter(_filePath, false);
            writer.WriteLine("size,seconds,moves,seed,timestamp");

            foreach (var s in scores)
            {
                writer.WriteLine(s.ToString());
            }
        }
        catch
        {
        }
    }
}