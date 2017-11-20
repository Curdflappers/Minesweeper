using System;

public class Spot {
    bool _mine;
    bool _revealed;
    bool _flagged;
    int _neighboringMines;

    public bool Mine { get { return _mine; } }
    public bool Revealed { get { return _revealed; } }
    public bool Flagged { get { return _flagged; } }
    public int NeighboringMines { get { return _neighboringMines; } }

    public Spot(bool mine, int neighboringMines)
    {
        _mine = mine;
        _revealed = false;
        _flagged = false;
        _neighboringMines = neighboringMines;
    }

    /// <summary>
    /// Attempt to sweep the given location
    /// If flagged, do nothing
    /// If unrevealed, reveal
    /// </summary>
    public void TrySweep()
    {
        if(_flagged) { return; }
        if(!_revealed) // any revealed location has already been swept
        {
            _revealed = true;
            RaiseStateChanged(new SpotEventArgs(_mine));
        }
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
}
