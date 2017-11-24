using UnityEngine;
using UnityEngine.UI;

public class MinesLeftView : MonoBehaviour {
    Game _game;

    public Game Game
    {
        set
        {
            // Remove old game
            if (_game != null)
            {
                _game.MinesLeftChanged -= HandleMinesLeftChanged;
            }

            // Add new game
            _game = value;
            if (_game != null)
            {
                _game.MinesLeftChanged += HandleMinesLeftChanged;
                HandleMinesLeftChanged(_game, null);
            }
        }
    }

    void HandleMinesLeftChanged(object o, GameEventArgs e)
    {
        Game game = (Game)o;
        GetComponent<Text>().text = "" + game.MinesLeft;
    }

}
