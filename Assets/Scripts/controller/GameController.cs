using UnityEngine;

public class GameController : MonoBehaviour
{
    public static bool SweepMode = true;
    Game _game;
    public Game Game { set { _game = value; } }

    public void ChangeMode()
    {
        SweepMode = !SweepMode;
    }

    public void Reset()
    {
        _game.Reset();
    }
}
