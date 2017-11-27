using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour {

    public string Field;

	void Start () {
        Settings.FieldChanged += HandleFieldChanged;
        SetInitialView();
	}

    void SetInitialView()
    {
        int value = 0;
        switch(Field)
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

    void HandleFieldChanged(object o, SettingsEventArgs e)
    {
        if(e.Field.Equals(Field))
        {
            GetComponent<InputField>().text = "" + e.NewValue;
        }
    }

    private void OnDestroy()
    {
        Settings.FieldChanged -= HandleFieldChanged;
    }
}
