using UnityEngine;

public class GameInitializer : MonoBehaviour {

    public static int Rows = 10, Cols = 19;
    Game _game;

    /// <summary>
    /// The chance that any given location is a mine
    /// </summary>
    const float MINE_CHANCE = 0.2f;

    void Start()
    {
        _game = new Game(Rows, Cols, (int)(MINE_CHANCE * Rows * Cols));
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
        GameObject.Find("Mines Left Text").
            GetComponent<MinesLeftView>().Game = _game;
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
        float sideLength = Mathf.Min(parentRect.width / Cols, parentRect.height / Rows);
        float sizeMult = sideLength / 100;

        spotT.localScale = new Vector3(sizeMult, sizeMult, 1);
        Vector3 offset = new Vector3(parentRect.xMin, parentRect.yMax);
        float epsilon = 0.0001f;
        if(Mathf.Abs(sideLength - parentRect.width / Cols) <= epsilon) // span entire width
        {
            float emptySpace = parentRect.height - sideLength * Rows;
            offset.y -= emptySpace / 2; // center vertically
        }
        else
        {
            float emptySpace = parentRect.width - sideLength * Cols;
            offset.x += emptySpace / 2;
        }
        spotT.localPosition = offset;

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
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
        go.name = "" + (row * Cols + col);

        Spot spot = _game.Spots[row, col];
        go.GetComponent<SpotButton>().Spot = spot;
        go.GetComponent<SpotView>().Spot = spot;
    }
}
