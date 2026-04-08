using Minesweeper.Core;
using Minesweeper.ConsoleApp;

Console.WriteLine("=== MINESWEEPER ===");

while (true)
{
    // --- MENU ---
    Console.WriteLine();
    Console.WriteLine("Choose board size:");
    Console.WriteLine("1) 8x8");
    Console.WriteLine("2) 12x12");
    Console.WriteLine("3) 16x16");
    Console.WriteLine("Q) Quit");

    Console.Write("> ");
    string? menuInput = Console.ReadLine();

    if (menuInput?.Trim().ToLower() == "q")
        break;

    if (!int.TryParse(menuInput, out int choice) || choice < 1 || choice > 3)
    {
        Console.WriteLine("Invalid choice.");
        continue;
    }

    BoardSize size = choice switch
    {
        1 => BoardSize.Small8x8,
        2 => BoardSize.Medium12x12,
        3 => BoardSize.Large16x16,
        _ => BoardSize.Small8x8
    };

    // --- SEED INPUT ---
    Console.Write("Seed (blank = time): ");
    string? seedInput = Console.ReadLine();

    int seed;
    if (string.IsNullOrWhiteSpace(seedInput))
        seed = Environment.TickCount;
    else
    {
        while (!int.TryParse(seedInput, out seed))
        {
            Console.Write("Invalid seed. Enter an integer: ");
            seedInput = Console.ReadLine();
        }
    }

    Console.WriteLine($"Using seed: {seed}");

    // --- CREATE GAME ---
    var game = new MinesweeperGame(size, seed);
    var manager = new HighScoreManager("data/highscores.csv");

    // --- GAME LOOP ---
    while (game.State == GameState.InProgress)
    {
        BoardRenderer.RenderBoard(game.Board);

        Console.WriteLine("Commands: r row col | f row col | q");
        Console.Write("> ");
        string? input = Console.ReadLine();

        if (input == null)
            continue;

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1 && parts[0].ToLower() == "q")
            break;

        if (parts.Length != 3)
        {
            Console.WriteLine("Invalid command.");
            continue;
        }

        string cmd = parts[0].ToLower();

        if (!int.TryParse(parts[1], out int row) ||
            !int.TryParse(parts[2], out int col))
        {
            Console.WriteLine("Invalid coordinates.");
            continue;
        }

        try
        {
            if (cmd == "r")
                game.Reveal(row, col);
            else if (cmd == "f")
                game.ToggleFlag(row, col);
            else
                Console.WriteLine("Unknown command.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // --- GAME END ---
    BoardRenderer.RenderBoard(game.Board);

    if (game.State == GameState.Won)
    {
        Console.WriteLine("You won!");

        manager.AddScore(new HighScore
        {
            Size = game.Size,
            Seconds = game.GetElapsedSeconds(),
            Moves = game.Moves,
            Seed = game.Seed,
            Timestamp = DateTime.UtcNow
        });

        Console.WriteLine("High score saved.");
    }
    else if (game.State == GameState.Lost)
    {
        Console.WriteLine("You hit a mine. Game over.");
    }

    Console.WriteLine("Returning to menu...");
}