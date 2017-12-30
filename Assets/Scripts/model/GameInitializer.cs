using UnityEngine;

public class GameInitializer : MonoBehaviour {
    Game _game;

    void Start()
    {
        Settings.UpdateField("mode", 1); // start in sweep mode always
        _game = new Game(Settings.Rows, Settings.Cols, Settings.Mines);
        PlaceSpots();
        ConnectUI();
    }

    /// <summary>
    /// Connect UI to listen to game logic
    /// </summary>
    void ConnectUI()
    {
        GameObject.Find("Reset Button").
            GetComponent<GameController>().Game = _game;

        _game.MinesLeftView = 
            GameObject.Find("Mines Left Text").GetComponent<MinesLeftView>();

        _game.TimerView = 
            GameObject.Find("Timer Text").GetComponent<TimerView>();
    }

    /// <summary>
    /// Place the spots on the screen
    /// </summary>
    void PlaceSpots()
    {
        // set up the first spot
        GameObject spotButton = Resources.Load<GameObject>("Prefabs/Spot");
        spotButton = (GameObject)Instantiate(spotButton, transform);

        Transform spotT = spotButton.transform;
        Rect parentRect = GetComponent<RectTransform>().rect;
        float sideLength = Mathf.Min(
            parentRect.width / Settings.Cols, 
            parentRect.height / Settings.Rows);
        float sizeMult = sideLength / 100;

        spotT.localScale = new Vector3(sizeMult, sizeMult, 1);
        Vector3 offset = new Vector3(parentRect.xMin, parentRect.yMax);
        float epsilon = 0.0001f;
        if(Mathf.Abs(sideLength - parentRect.width / Settings.Cols) <= epsilon)
        {
            float emptySpace = parentRect.height - sideLength * Settings.Rows;
            offset.y -= emptySpace / 2; // center vertically
        }
        else
        {
            float emptySpace = parentRect.width - sideLength * Settings.Cols;
            offset.x += emptySpace / 2;
        }
        spotT.localPosition = offset;

        for (int r = 0; r < Settings.Rows; r++)
        {
            for (int c = 0; c < Settings.Cols; c++)
            {
                // stamp it down
                GameObject currentSpot =
                    (GameObject)Instantiate(spotButton, transform);
                PopulateSpot(currentSpot, r, c);

                // move to next col
                spotT.localPosition += new Vector3(sideLength, 0);
            }

            // move to next row: reset x, update y
            spotT.localPosition =
                new Vector3(offset.x, spotT.localPosition.y - sideLength);
        }

        Destroy(spotButton);
    }
 
    /// <summary>
    /// Populates <paramref name="go"/> with its corresponding assets, 
    /// namely its images and state (mine or not)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    void PopulateSpot(GameObject go, int row, int col)
    {
        go.name = "" + (row * Settings.Cols + col);

        Spot spot = _game.Spots[row, col];
        go.GetComponent<SpotButton>().Spot = spot;
        go.GetComponent<SpotView>().Spot = spot;
        spot.View = go.GetComponent<SpotView>();
    }
}
