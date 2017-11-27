using UnityEngine;

public class ModeButtonController : MonoBehaviour {
    
    public void ToggleSweepMode()
    {
        int value = Settings.SweepMode ? 0 : 1;
        Settings.UpdateField("mode", value);
    }
}
