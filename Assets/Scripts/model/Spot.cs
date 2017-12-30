using System;

public class Spot {
    bool _mine;
    bool _revealed;
    bool _flagged;
    int _neighboringMines;
    int _row, _col;
    Game _game;
    SpotView _view;

    public bool Mine { get { return _mine; } }
    public bool Revealed { get { return _revealed; } }
    public bool Flagged { get { return _flagged; } }
    public int NeighboringMines { get { return _neighboringMines; } }
    public int Row { get { return _row; } }
    public int Col { get { return _col; } }
    public Game Game { get { return _game; } }
    public SpotView View
    {
        get { return _view; }
        set
        {
            _view = value;
        }
    }

    public Spot(bool mine, int neighboringMines, int row, int col, Game game)
    {
        _mine = mine;
        _revealed = false;
        _flagged = false;
        _neighboringMines = neighboringMines;
        _row = row;
        _col = col;
        _game = game;
    }

    /// <summary>
    /// Attempt to sweep the given location
    /// If flagged, do nothing
    /// If unrevealed, reveal
    /// </summary>
    public void TrySweep()
    {
        if(_flagged || _revealed) { return; }

        _revealed = true;
        UpdateView(_mine);
    }

    /// <summary>
    /// Reveal this spot without sweeping it
    /// </summary>
    public void Reveal()
    {
        if (!_revealed)
        {
            _revealed = true;
            UpdateView(false);
        }
    }

    public void Flag()
    {
        _flagged = !_flagged;
        UpdateView(false); // never explodes here
    }
    
    protected virtual void UpdateView(bool exploded)
    {
        if(_view != null) { _view.HandleStateChanged(exploded); }
        if(_game != null) { _game.HandleSpotChanged(this, exploded); }
    }

    public void Reset(bool mine, int neighboringMines)
    {
        _revealed = false;
        _flagged = false;
        _mine = mine;
        _neighboringMines = neighboringMines;
        UpdateView(false);
    }
}
