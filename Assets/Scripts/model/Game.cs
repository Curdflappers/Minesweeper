
using System.Collections.Generic;
using UnityEngine;

public class Game {
    Spot[,] _spots;
    int _rows, _cols;
    int _mineCount;

    public Spot[,] Spots
    {
        get
        {
            if(_spots == null) { return null; }
            Spot[,] spots =  new Spot[_spots.GetLength(0), _spots.GetLength(1)];
            for (int r = 0; r < spots.GetLength(0); r++)
                for (int c = 0; c < spots.GetLength(1); c++)
                    spots[r, c] = _spots[r, c];
            return spots;
        }
    }

    /// <summary>
    /// Creates a new game, creating its own array of spots
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="cols"></param>
    public Game(int rows, int cols, int mineCount)
    {
        _rows = rows;
        _cols = cols;
        _mineCount = mineCount;
        _spots = new Spot[_rows, _cols];
        bool[,] mines = NewMines();
        for(int r = 0; r < _rows; r++)
            for(int c = 0; c < _cols; c++)
            {
                int neighboringMines = NeighboringMines(r, c, mines);
                _spots[r, c] = new Spot(mines[r, c], neighboringMines, r, c);
            }
        AttachSpots();
    }

    private int NeighboringMines(int row, int col, bool[,] mines)
    {
        int neighbors = 0;
        for (int r = row - 1; r <= row + 1; r++)
        {
            for (int c = col - 1; c <= col + 1; c++)
            {
                if ((r == row && c == col) // do not check self
                   || !ValidLoc(r, c, mines)) // do not check invalid locs
                {
                    continue;
                }
                else if (mines[r, c]) { neighbors++; }
            }
        }
        return neighbors;
    }
        
    bool ValidLoc(int row, int col, bool[,] mines)
    {
        return row >= 0 && row < mines.GetLength(0)
            && col >= 0 && col < mines.GetLength(1);
    }

    public bool[,] NewMines()
    {
        List<Location> locs = new List<Location>();


        bool[,] mines = new bool[_rows, _cols];
        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                locs.Add(new Location(r, c));

        for(int i = 0; i < _mineCount && locs.Count > 0; i++)
        {
            int index = Random.Range(0, locs.Count);
            Location loc = locs[index];
            mines[loc.Row, loc.Col] = true;
            locs.RemoveAt(index);
        }

        return mines;
    }

    /// <summary>
    /// Creates a new game, assuming <paramref name="spots"/> is a populated
    /// array of spots
    /// </summary>
    /// <param name="spots"></param>
    public Game(Spot[,] spots)
    {
        _spots = spots;
    }

    /// <summary>
    /// Listen to each element of _spots
    /// </summary>
    private void AttachSpots()
    {
        foreach (Spot spot in _spots)
        {
            spot.StateChanged += HandleExplosion;
            spot.Clicked += HandleSpotClicked;
        }
    }

    public virtual void HandleExplosion(object o, SpotEventArgs e)
    {
        Spot currSpot = (Spot)o;
        if (e.Exploded)
        {
            foreach (Spot spot in _spots) { spot.Reveal(); }
        }
        else if(currSpot.Revealed)
        {
            // sweep all neighbors if this has no neighboring mines
            if(currSpot.NeighboringMines == 0)
            {
                for(int r = currSpot.Row - 1; r <= currSpot.Row + 1; r++)
                    for(int c = currSpot.Col - 1; c <= currSpot.Col + 1; c++)
                        if (ValidLoc(r, c)) { _spots[r, c].TrySweep(); }
            }
        }
    }

    public virtual void HandleSpotClicked(object o, SpotEventArgs e)
    {
        Spot spot = (Spot)o;

        if (spot.Revealed)
        {
            // if flagged neighbors cancel out neighboring mine
            // sweep neighbors, regardless of game mode
            if (NeighboringFlags(spot) >= spot.NeighboringMines)
                TrySweepNeighbors(spot);
            return;
        }

        if (GameController.SweepMode)
        {
            spot.TrySweep();
        }
        else
        {
            spot.Flag();
        }
    }

    void TrySweepNeighbors(Spot spot)
    {
        for (int r = spot.Row - 1; r <= spot.Row + 1; r++)
            for (int c = spot.Col - 1; c <= spot.Col + 1; c++)
                if (ValidLoc(r, c))
                    _spots[r, c].TrySweep();
    }

    int NeighboringFlags(Spot spot)
    {
        int flags = 0;
        for (int r = spot.Row - 1; r <= spot.Row + 1; r++)
            for (int c = spot.Col - 1; c <= spot.Col + 1; c++)
                if (ValidLoc(r, c))
                    flags += _spots[r, c].Flagged ? 1 : 0;
        return flags;
    }

    bool ValidLoc(int row, int col)
    {
        return row >= 0 && row < _spots.GetLength(0)
            && col >= 0 && col < _spots.GetLength(1);
    }

    public void Reset()
    {
        bool[,] mines = NewMines();

        for (int r = 0; r < mines.GetLength(0); r++)
            for (int c = 0; c < mines.GetLength(1); c++)
                _spots[r, c].Reset(mines[r, c], NeighboringMines(r, c, mines));
    }

    private class Location
    {
        public int Row, Col;
        public Location(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}
