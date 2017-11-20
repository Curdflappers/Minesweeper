
public class Game {
    Spot[,] _spots;

    public Game(Spot[,] spots)
    {
        _spots = spots;
        foreach(Spot spot in _spots)
        {
            spot.StateChanged += HandleExplosion;
        }
    }

    public virtual void HandleExplosion(object o, SpotEventArgs e)
    {
        if (e.Exploded)
        {
            foreach (Spot spot in _spots) { spot.Reveal(); }
        }
    }
}
