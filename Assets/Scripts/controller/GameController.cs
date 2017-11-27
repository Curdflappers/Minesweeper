using UnityEngine;

public class GameController : MonoBehaviour
{
    Game _game;
    public Game Game { set { _game = value; } }
    
    public void Reset()
    {
        _game.Reset();
    }
}
