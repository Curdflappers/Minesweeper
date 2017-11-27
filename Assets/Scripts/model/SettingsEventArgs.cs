using System;

public class SettingsEventArgs : EventArgs {

    public string Field;
    public int NewValue;

	public SettingsEventArgs(string field, int newValue)
    {
        Field = field;
        NewValue = newValue;
    }
}
