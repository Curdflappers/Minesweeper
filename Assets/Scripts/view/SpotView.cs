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
            _spot.StateChanged += HandleStateChanged;
            neighboringUpdated = false;
            UpdateState(false);
            // TODO FIX THIS
            // TODO unlisten to other spot
            // listen to new spot
        }
    }

    public virtual void HandleStateChanged(object o, SpotEventArgs e)
    {
        UpdateState(e.Exploded);
    }

    void UpdateState(bool exploded)
    {
        ChildImage("Flag Image").enabled = _spot.Flagged;
        ChildImage("Unrevealed Image").enabled = !_spot.Revealed;
        if (!neighboringUpdated)
        {
            string path = "Sprites/";
            path += _spot.Mine ? "mine" : "" + _spot.NeighboringMines;

            ChildImage("Number Image").sprite = Resources.Load<Sprite>(path);
        }
        if(exploded)
        {
            GetComponent<Image>().color = Color.red;
        }
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
