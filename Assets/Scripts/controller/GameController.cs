using UnityEngine;

public class GameController : MonoBehaviour
{
    public static bool SweepMode = true;

    public void ChangeMode()
    {
        SweepMode = !SweepMode;
    }
}
