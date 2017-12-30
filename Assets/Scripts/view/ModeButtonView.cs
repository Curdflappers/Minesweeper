using UnityEngine;
using UnityEngine.UI;

public class ModeButtonView : SettingsView {

    Sprite _sweepSprite, _flagSprite;

    internal override void Start()
    {
        _sweepSprite = Resources.Load<Sprite>("Sprites/mine");
        _flagSprite = Resources.Load<Sprite>("Sprites/flag");
        base.Start();
    }

    internal override void SetInitialView()
    {
        int value = Settings.SweepMode ? 1 : 0;
        HandleFieldChanged(null, new SettingsEventArgs("mode", value));
    }

    internal override void HandleFieldChanged(object o, SettingsEventArgs e)
    {
        if (e.Field.Equals("mode"))
        {
            bool sweepMode = e.NewValue > 0;
            Image image = 
                transform.FindChild("Mode Image").GetComponent<Image>();
            image.sprite = sweepMode ? _sweepSprite : _flagSprite;
        }
    }
}
