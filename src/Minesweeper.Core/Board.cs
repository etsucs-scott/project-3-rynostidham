using System;
using System.Collections.Generic;

namespace Minesweeper.Core;
/// <summary>
/// Represents the full Minesweeper board.
/// Handles mine placement, adjacency calculation,
/// tile revealing, flagging, and cascade reveal logic.
/// </summary>

public class Board
{
    private readonly Tile[,] _tiles;
    private readonly int _rows;
    private readonly int _cols;
    private readonly int _mineCount;

    public int Rows => _rows;
    public int Columns => _cols;

    public Board(BoardSize size, int seed)
    {
        var (rows, cols, mines) = BoardConfig.GetConfig(size);
        _rows = rows;
        _cols = cols;
        _mineCount = mines;

        _tiles = new Tile[_rows, _cols];
        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                _tiles[r, c] = new Tile();

        PlaceMines(seed);
        ComputeAdjacency();
    }

    public Tile GetTile(int row, int col)
    {
        ValidateCoords(row, col);
        return _tiles[row, col];
    }

    private void ValidateCoords(int r, int c)
    {
        if (r < 0 || r >= _rows || c < 0 || c >= _cols)
            throw new ArgumentOutOfRangeException();
    }

    private void PlaceMines(int seed)
    {
        var rng = new Random(seed);
        int placed = 0;

        while (placed < _mineCount)
        {
            int r = rng.Next(_rows);
            int c = rng.Next(_cols);

            if (!_tiles[r, c].IsMine)
            {
                _tiles[r, c].IsMine = true;
                placed++;
            }
        }
    }

    private void ComputeAdjacency()
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                if (_tiles[r, c].IsMine)
                {
                    _tiles[r, c].AdjacentMines = -1;
                    continue;
                }

                int count = 0;
                ForEachNeighbor(r, c, (nr, nc) =>
                {
                    if (_tiles[nr, nc].IsMine) count++;
                });

                _tiles[r, c].AdjacentMines = count;
            }
        }
    }

    private void ForEachNeighbor(int r, int c, Action<int, int> action)
    {
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;

                int nr = r + dr;
                int nc = c + dc;

                if (nr >= 0 && nr < _rows && nc >= 0 && nc < _cols)
                    action(nr, nc);
            }
        }
    }

    public bool ToggleFlag(int r, int c)
    {
        ValidateCoords(r, c);
        var t = _tiles[r, c];

        if (t.IsRevealed) return false;

        t.IsFlagged = !t.IsFlagged;
        return true;
    }

    public bool Reveal(int r, int c)
    {
        ValidateCoords(r, c);
        var t = _tiles[r, c];

        if (t.IsRevealed || t.IsFlagged)
            return false;

        t.IsRevealed = true;

        if (t.IsMine)
            return true;

        if (t.AdjacentMines == 0)
            Cascade(r, c);

        return false;
    }

    private void Cascade(int r, int c)
    {
        var stack = new Stack<(int r, int c)>();
        stack.Push((r, c));

        while (stack.Count > 0)
        {
            var (cr, cc) = stack.Pop();

            ForEachNeighbor(cr, cc, (nr, nc) =>
            {
                var t = _tiles[nr, nc];

                if (t.IsRevealed || t.IsFlagged || t.IsMine)
                    return;

                t.IsRevealed = true;

                if (t.AdjacentMines == 0)
                    stack.Push((nr, nc));
            });
        }
    }

    public bool AllNonMinesRevealed()
    {
        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                if (!_tiles[r, c].IsMine && !_tiles[r, c].IsRevealed)
                    return false;

        return true;
    }
}