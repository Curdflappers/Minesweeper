using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    public string Field;

    public void UpdateField()
    {
        string value = transform.FindChild("Text").GetComponent<Text>().text;
        bool success = false;
        int previousValue = 0;

        switch(Field)
        {
            case ("rows"):
                previousValue = GameInitializer.Rows;
                success = int.TryParse(value, out GameInitializer.Rows);
                if (!success
                    || GameInitializer.Rows < 1
                    || GameInitializer.Rows > 20)
                {
                    GameInitializer.Rows = previousValue;
                    success = false;
                }
                break;
            case ("cols"):
                previousValue = GameInitializer.Cols;
                success = int.TryParse(value, out GameInitializer.Cols);
                if (!success
                    || GameInitializer.Cols < 1
                    || GameInitializer.Cols > 40)
                {
                    GameInitializer.Cols = previousValue;
                    success = false;
                }
                break;
            case ("mines"):
                previousValue = GameInitializer.Mines;
                success = int.TryParse(value, out GameInitializer.Mines);
                if (!success
                    || GameInitializer.Mines < 1
                    || GameInitializer.Mines >= GameInitializer.Rows * GameInitializer.Cols)
                {
                    GameInitializer.Mines = previousValue;
                    success = false;
                }
                break;
        }

        if(!success) { UpdateText(previousValue); }
    }

    private void UpdateText(int value)
    {
        GetComponent<InputField>().text = "" + value;
    }

    private void Start()
    {
        int value = 0;
        switch (Field)
        {
            case ("rows"):
                value = GameInitializer.Rows;
                break;
            case ("cols"):
                value = GameInitializer.Cols;
                break;
            case ("mines"):
                value = GameInitializer.Mines;
                break;
        }
        GetComponent<InputField>().text = "" + value;
    }
}
