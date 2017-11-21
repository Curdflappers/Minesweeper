
public class Game {
    Spot[,] _spots;

    public Game(Spot[,] spots)
    {
        _spots = spots;
        foreach(Spot spot in _spots)
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
        bool[,] mines = GameInitializer.NewMines();

        for (int r = 0; r < mines.GetLength(0); r++)
            for (int c = 0; c < mines.GetLength(1); c++)
                _spots[r, c].Reset(
                    mines[r, c], 
                    GameInitializer.NeighboringMines(r, c, mines));
    }
}
