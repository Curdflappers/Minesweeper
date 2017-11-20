using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SpotButton : MonoBehaviour {
    Spot _spot;

    public Spot Spot
    {
        get { return _spot; }
        set { _spot = value; }
    }

    /// <summary>
    /// Interact with the given spot based on the game mode
    /// </summary>
    public void Interact()
    {
        if (GameController.SweepMode)
        {
            _spot.TrySweep();
            GetComponentInChildren<Button>().interactable = !_spot.Revealed;
        }
        else
        {
            _spot.Flag();
        }
    }
}
