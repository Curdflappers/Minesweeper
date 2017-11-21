using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SpotButton : MonoBehaviour {
    Spot _spot;

    public Spot Spot
    {
        get { return _spot; }
        set
        {
            _spot = value;
            _spot.StateChanged += HandleStateChanged;
        }
    }

    /// <summary>
    /// Interact with the given spot based on the game mode
    /// </summary>
    public void Interact()
    {
        if (GameController.SweepMode)
        {
            _spot.TrySweep();
        }
        else
        {
            _spot.Flag();
        }
    }

    public virtual void HandleStateChanged(object o, SpotEventArgs e)
    {
        Spot spot = (Spot)o;
        GetComponentInChildren<Button>().interactable = !_spot.Revealed;
    }
}
