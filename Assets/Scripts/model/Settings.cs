using System;

public class Settings
{
    public static int Rows = 16, Cols = 16, Mines = 40;
    const int MAX_ROWS = 20, MAX_COLS = 40;
    public static bool SweepMode = true;

    public static void UpdateField(string field, int value)
    {
        int newValue = 0;
        switch (field)
        {
            case ("rows"):
                if(value > 0 && value <= MAX_ROWS)
                {
                    Rows = value;
                }
                newValue = Rows;
                break;
            case ("cols"):
                if (value > 0 && value <= MAX_COLS)
                {
                    Cols = value;
                }
                newValue = Cols;
                break;
            case ("mines"):
                if (value > 0 && value < Rows * Cols)
                {
                    Mines = value;
                }
                newValue = Mines;
                break;
            case ("mode"):
                SweepMode = value != 0;
                newValue = SweepMode ? 1 : 0;
                break;
        }

        RaiseFieldChanged(new SettingsEventArgs(field, newValue));
    }

    public static event EventHandler<SettingsEventArgs> FieldChanged;
    protected static void RaiseFieldChanged(SettingsEventArgs e)
    {
        if (FieldChanged != null)
        {
            FieldChanged(null, e);
        }
    }
}
