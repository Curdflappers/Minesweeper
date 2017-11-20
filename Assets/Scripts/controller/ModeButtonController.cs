using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModeButtonController : MonoBehaviour {
   
    public void UpdateImage()
    {
        string path;
        Color color;
        if (GameController.SweepMode)
        {
            path = "Sprites/mine";
            color = Color.white;
        }
        else
        {
            path = "Sprites/flag";
            color = Color.red;
        }
        Image image = transform.FindChild("Mode Image").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>(path);
        image.color = color;
    }
}
