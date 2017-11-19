using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour {

    public int Rows, Cols;
    public int SideLength;

	void Start () {

        // set up the first spot
        GameObject spot = Resources.Load<GameObject>("Prefabs/Spot");
        spot = (GameObject)Instantiate(spot, transform);
        RectTransform spotT = spot.GetComponent<RectTransform>();
        spotT.localScale = new Vector3(1, 1, 1);
        Rect canvasRect = GetComponent<RectTransform>().rect;
        Vector3 offset = new Vector3(canvasRect.xMin, canvasRect.yMax);
        spotT.localPosition = offset;

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                Instantiate(spot, transform); // stamp it down
                spotT.localPosition += new Vector3(SideLength, 0); // move to next col
            }

            // move to next row: reset x, update y
            spotT.localPosition = new Vector3(canvasRect.xMin, spotT.localPosition.y - SideLength);
        }

        Destroy(spot);
	}
}
