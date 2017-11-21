using UnityEngine;

public class GameInitializer : MonoBehaviour {

    public static int Rows = 5, Cols = 10;
    bool[,] _mines;
    Spot[,] _spots;
    Game _game;

    /// <summary>
    /// The chance that any given location is a mine
    /// </summary>
    const float MINE_CHANCE = 0.1f;

    private void Awake()
    {
        _spots = new Spot[Rows, Cols];
    }

    void Start()
    {
        _mines = NewMines();
        PlaceSpots();
        _game = new Game(_spots);
        PopulateButtons();
        //PrintMines();
    }

    void PopulateButtons()
    {
        GameObject.Find("Reset Button").GetComponent<GameController>().Game = _game;
    }

    public static bool[,] NewMines()
    {
        bool[,] mines = new bool[Rows, Cols];
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Cols; c++)
                mines[r, c] = Random.Range(0, 1f) < MINE_CHANCE;
        return mines;
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
                new Vector3(
                    parentRect.xMin, 
                    spotT.localPosition.y - sideLength);
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

        Spot spot = new Spot(
            _mines[row, col], 
            NeighboringMines(row, col, _mines), 
            row, 
            col);
        go.GetComponent<SpotButton>().Spot = spot;
        go.GetComponent<SpotView>().Spot = spot;

        _spots[row, col] = spot;
    }

    public static int NeighboringMines(int row, int col, bool[,] mines)
    {
        int neighbors = 0;
        for(int r = row - 1; r <= row + 1; r++)
        {
            for (int c = col - 1; c <= col + 1; c++)
            {
                if((r == row && c == col) // do not check self
                   || !ValidLoc(r, c, mines)) // do not check invalid locs
                {
                    continue;
                }
                else if (mines[r, c]) { neighbors++; }
            }
        }
        return neighbors;
    }

    static bool ValidLoc(int row, int col, bool[,] mines)
    {
        return row >= 0 && row < mines.GetLength(0)
            && col >= 0 && col < mines.GetLength(1);
    }

    /// <summary>
    /// For debug purposes only, print the mines array to the console
    /// </summary>
    void PrintMines()
    {
        string str = "";
        for(int r = 0; r < _mines.GetLength(0); r++)
        {
            for(int c = 0; c < _mines.GetLength(1); c++)
            {
                str += (_mines[r, c] ? "X" : "O") + " ";
            }
            Debug.Log(str);
            str = "";
        }
    }
}
