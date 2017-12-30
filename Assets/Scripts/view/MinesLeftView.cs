using UnityEngine;
using UnityEngine.UI;

public class MinesLeftView : MonoBehaviour {

    public void HandleMinesLeftChanged(int minesLeft)
    {
        GetComponent<Text>().text = "" + minesLeft;
    }

}
