using UnityEngine;

public class GameInitializer : MonoBehaviour {

    public int Rows, Cols;
    public int SideLength;
    bool[,] _mines;
    Spot[,] _spots;
    Game _game;

    /// <summary>
    /// The chance that any given location is a mine
    /// </summary>
    const float MINE_CHANCE = 0.2f;

    private void Awake()
    {
        _spots = new Spot[Rows, Cols];
        _mines = new bool[Rows, Cols];
    }

    void Start()
    {
        CreateMines();
        PlaceSpots();
        _game = new Game(_spots);
        //PrintMines();
    }

    /// <summary>
    /// Populate the minefield
    /// </summary>
    void CreateMines()
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Cols; c++)
            {
                float value = Random.Range(0, 1F);
                _mines[r, c] = value < MINE_CHANCE;
            }
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

        spotT.localScale = new Vector3(1, 1, 1);
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
                spotT.localPosition += new Vector3(SideLength, 0);
            }

            // move to next row: reset x, update y
            spotT.localPosition =
                new Vector3(
                    parentRect.xMin, 
                    spotT.localPosition.y - SideLength);
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

        Spot spot = new Spot(_mines[row, col], NeighboringMines(row, col));
        go.GetComponent<SpotButton>().Spot = spot;
        go.GetComponent<SpotView>().Spot = spot;

        _spots[row, col] = spot;
    }

    int NeighboringMines(int row, int col)
    {
        int neighbors = 0;
        for(int r = row - 1; r <= row + 1; r++)
        {
            for (int c = col - 1; c <= col + 1; c++)
            {
                if((r == row && c == col) // do not check self
                   || !ValidLoc(r, c)) // do not check invalid locs
                {
                    continue;
                }
                else if (_mines[r, c]) { neighbors++; }
            }
        }
        return neighbors;
    }

    bool ValidLoc(int row, int col)
    {
        return row >= 0 && row < _mines.GetLength(0)
            && col >= 0 && col < _mines.GetLength(1);
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
