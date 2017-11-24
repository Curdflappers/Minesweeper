using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour {
    Game _game;
    bool on;
    float time;

    public Game Game
    {
        set
        {
            // Remove old game
            if (_game != null)
            {
                _game.MinesLeftChanged -= HandleGameStateChanged;
            }

            // Add new game
            _game = value;
            if (_game != null)
            {
                _game.MinesLeftChanged += HandleGameStateChanged;
                HandleGameStateChanged(_game, null);
            }
        }
    }

    private void Update()
    {
        if(on)
        {
            time += Time.deltaTime;
            int minutes = ((int)time / 60) % 60;
            int seconds = (int)time % 60;
            GetComponent<Text>().text = 
                string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void HandleGameStateChanged(object o, GameEventArgs e)
    {
        on = !_game.NewGame && !_game.GameOver;
        if(_game.NewGame)
        {
            time = 0;
            GetComponent<Text>().text = "00:00";
        }
    }
}
