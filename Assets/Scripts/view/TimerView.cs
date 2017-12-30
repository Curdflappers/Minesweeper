using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour {
    bool on;
    float time;

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

    public void HandleGameStateChanged(Game game)
    {
        on = !game.NewGame && !game.GameOver;
        if(game.NewGame)
        {
            time = 0;
            GetComponent<Text>().text = "00:00";
        }
    }
}
