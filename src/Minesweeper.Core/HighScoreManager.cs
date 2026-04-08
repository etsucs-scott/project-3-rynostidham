using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Minesweeper.Core;

public class HighScoreManager
{
    private readonly string _filePath;

    public HighScoreManager(string filePath)
    {
        _filePath = filePath;
    }

    public List<HighScore> Load()
    {
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
                // If creation fails, return empty list but don't crash
                return scores;
            }
        }

        try
        {
            var lines = File.ReadAllLines(_filePath).Skip(1); // skip header

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length != 5)
                    continue; // skip malformed lines

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
            // If reading fails, return empty list but don't crash
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
            // Do not crash — console UI will show an error
        }
    }
}