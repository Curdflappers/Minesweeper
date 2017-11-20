using UnityEngine;
using System.Collections;
using System;

public class SpotButton : MonoBehaviour {
    Spot _spot;

    public Spot Spot
    {
        get { return _spot; }
        set { _spot = value; }
    }

    public void Sweep()
    {
        _spot.TrySweep();
    }
}
