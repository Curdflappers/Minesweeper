
using System;
using System.Collections.Generic;
using UnityEngine;

public class Game {
    Spot[,] _spots;
    int _rows, _cols;
    int _totalMines;
    int _minesLeft;
    /// <summary>
    /// The number of unflagged, unrevealed spots left in the minefield
    /// </summary>
    int _unflaggedSpots;
    bool _newGame;
    bool _gameOver;
    bool _gameWon;

    public int MinesLeft { get { return _minesLeft; } }
    public bool NewGame { get { return _newGame; } }
    public bool GameOver { get { return _gameOver; } }
    public bool GameWon { get { return _gameWon; } }

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
        _totalMines = mineCount;
        _minesLeft = mineCount;
        _spots = new Spot[_rows, _cols];
        _unflaggedSpots = _rows * _cols; // all spots unflagged
        _newGame = true;
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

        for(int i = 0; i < _totalMines && locs.Count > 0; i++)
        {
            //int index = i; // for debugging
            int index = UnityEngine.Random.Range(0, locs.Count);
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
            spot.StateChanged += HandleSpotChanged;
            spot.Clicked += HandleSpotClicked;
        }
    }

    public virtual void HandleSpotChanged(object o, SpotEventArgs e)
    {
        if(_newGame) { return; } // don't update on spot resets
        Spot currSpot = (Spot)o;
        if (e.Exploded)
        {
            _gameOver = true;
            _gameWon = false;
            foreach (Spot spot in _spots) { spot.Reveal(); }
            RaiseMinesLeftChanged(null);
            return;
        }

        if (_gameOver)
        {
            return;
        }
        _unflaggedSpots += currSpot.Flagged || currSpot.Revealed ? -1 : 1;

        // Spot was swept, but didn't explode
        if (currSpot.Revealed)
        {
            // sweep all neighbors if this has no neighboring mines
            if(currSpot.NeighboringMines == 0)
            {
                for(int r = currSpot.Row - 1; r <= currSpot.Row + 1; r++)
                    for(int c = currSpot.Col - 1; c <= currSpot.Col + 1; c++)
                        if (ValidLoc(r, c)) { _spots[r, c].TrySweep(); }
            }
        }
        else // spot flag state changed, update mines left
        {
            _minesLeft += currSpot.Flagged ? -1 : 1;
            RaiseMinesLeftChanged(new GameEventArgs());
        }
        
        if (_unflaggedSpots == _minesLeft)
        {
            _gameOver = true;
            _gameWon = true;
            foreach (Spot spot in _spots)
            {
                if (!spot.Revealed && !spot.Flagged) { spot.Flag(); }
            }
            RaiseMinesLeftChanged(new GameEventArgs());
        }
    }

    public virtual void HandleSpotClicked(object o, SpotEventArgs e)
    {
        if (_newGame)
        {
            _newGame = false;
            RaiseMinesLeftChanged(null);
        }
        Spot spot = (Spot)o;

        if (spot.Revealed)
        {
            // if flagged neighbors cancel out neighboring mine
            // sweep neighbors, regardless of game mode
            if (NeighboringFlags(spot) >= spot.NeighboringMines)
                TrySweepNeighbors(spot);
            return;
        }

        if (Settings.SweepMode)
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
        Settings.UpdateField("mode", 1); // start in sweep mode always
        _newGame = true;
        _gameOver = false;
        _gameWon = false;
        bool[,] mines = NewMines();

        for (int r = 0; r < mines.GetLength(0); r++)
            for (int c = 0; c < mines.GetLength(1); c++)
                _spots[r, c].Reset(mines[r, c], NeighboringMines(r, c, mines));
        
        _minesLeft = _totalMines;
        _unflaggedSpots = _rows * _cols;
        RaiseMinesLeftChanged(null);
    }

    public event EventHandler<GameEventArgs> MinesLeftChanged;
    protected virtual void RaiseMinesLeftChanged(GameEventArgs e)
    {
        if (MinesLeftChanged != null)
        {
            MinesLeftChanged(this, e);
        }
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
