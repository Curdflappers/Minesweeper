using System;

public class SpotEventArgs : EventArgs
{
    public bool Exploded;
    public SpotEventArgs(bool exploded)
    {
        Exploded = exploded;
    }
}

