using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is always attached to an instance of the Spot prefab
/// </summary>
public class SpotView : MonoBehaviour {
    Spot _spot;
    bool neighboringUpdated = false;
	
    public Spot Spot
    {
        get
        {
            return _spot;
        }
        set
        {
            _spot = value;
            neighboringUpdated = false;
            HandleStateChanged(false);
        }
    }

    public virtual void HandleStateChanged(bool exploded)
    {
        Image image = GetComponent<Image>();
        string spriteName = "";
        if (exploded) { spriteName = "explodedMine"; }
        else
        {
            if (_spot.Flagged)
            {
                if (_spot.Revealed && !_spot.Mine)
                {
                    spriteName = "falseFlag";
                }
                else { spriteName = "flag"; }
            }
            else
            {
                if (_spot.Revealed)
                {
                    if (_spot.Mine) { spriteName = "mine"; }
                    else
                    {
                        spriteName += _spot.NeighboringMines;
                    }
                }
                else
                {
                    spriteName = "unrevealed";
                }
            }
        }

        GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
    }

    /// <summary>
    /// Shortcut to calling 
    /// transform.FindChild(name)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Image ChildImage(string name)
    {
        return transform.FindChild(name).GetComponent<Image>();
    }
}
