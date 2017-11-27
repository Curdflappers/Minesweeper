using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    public string Field;

    public void UpdateField()
    {
        int value;
        int.TryParse(GetComponent<InputField>().text, out value);
        Settings.UpdateField(Field, value);
    }

    private void UpdateText(int value)
    {
        GetComponent<InputField>().text = "" + value;
    }

    public void SetDifficulty()
    {
        switch (Field)
        {
            case ("beginner"):
                Settings.UpdateField("rows", 9);
                Settings.UpdateField("cols", 9);
                Settings.UpdateField("mines", 10);
                break;
            case ("intermediate"):
                Settings.UpdateField("rows", 16);
                Settings.UpdateField("cols", 16);
                Settings.UpdateField("mines", 40);
                break;
            case ("expert"):
                Settings.UpdateField("rows", 16);
                Settings.UpdateField("cols", 30);
                Settings.UpdateField("mines", 99);
                break;
            case ("endurance"):
                Settings.UpdateField("rows", 20);
                Settings.UpdateField("cols", 40);
                Settings.UpdateField("mines", 160);
                break;
        }
    }

    /// <summary>
    /// Update the text if this is an input field
    /// </summary>
    private void Start()
    {
        if(GetComponent<InputField>() == null) { return; }
        int value = 0;
        switch (Field)
        {
            case ("rows"):
                value = Settings.Rows;
                break;
            case ("cols"):
                value = Settings.Cols;
                break;
            case ("mines"):
                value = Settings.Mines;
                break;
        }
        GetComponent<InputField>().text = "" + value;
    }
}
