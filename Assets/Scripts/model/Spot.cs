using System;

public class Spot {
    bool _mine;
    bool _revealed;
    bool _flagged;
    int _neighboringMines;
    int _row, _col;

    public bool Mine { get { return _mine; } }
    public bool Revealed { get { return _revealed; } }
    public bool Flagged { get { return _flagged; } }
    public int NeighboringMines { get { return _neighboringMines; } }
    public int Row { get { return _row; } }

    internal void HandleOnClick()
    {
        RaiseClicked(new SpotEventArgs(false));
    }

    public int Col { get { return _col; } }

    public Spot(bool mine, int neighboringMines, int row, int col)
    {
        _mine = mine;
        _revealed = false;
        _flagged = false;
        _neighboringMines = neighboringMines;
        _row = row;
        _col = col;
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
        RaiseStateChanged(new SpotEventArgs(_mine));
    }

    /// <summary>
    /// Reveal this spot without sweeping it
    /// </summary>
    public void Reveal()
    {
        if (!_revealed)
        {
            _revealed = true;
            RaiseStateChanged(new SpotEventArgs(false));
        }
    }

    public void Flag()
    {
        _flagged = !_flagged;
        RaiseStateChanged(new SpotEventArgs(false)); // never explodes here
    }

    public event EventHandler<SpotEventArgs> StateChanged;
    protected virtual void RaiseStateChanged(SpotEventArgs e)
    {
        if (StateChanged != null) // if there are some listeners
        {
            StateChanged(this, e); // notify all listeners
        }
    }

    public event EventHandler<SpotEventArgs> Clicked;
    protected virtual void RaiseClicked(SpotEventArgs e)
    {
        if (Clicked != null)
        {
            Clicked(this, e);
        }
    }

    public void Reset(bool mine, int neighboringMines)
    {
        _revealed = false;
        _flagged = false;
        _mine = mine;
        _neighboringMines = neighboringMines;
        RaiseStateChanged(new SpotEventArgs(false));
    }
}
