using UnityEngine;

public class SpotButton : MonoBehaviour {
    Spot _spot;

    public Spot Spot
    {
        get { return _spot; }
        set
        {
            _spot = value;
        }
    }

    /// <summary>
    /// Interact with the given spot based on the game mode
    /// </summary>
    public void OnClick()
    {
        _spot.Game.HandleSpotClicked(_spot);
    }
}
