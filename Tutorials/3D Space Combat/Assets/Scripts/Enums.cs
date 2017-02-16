using UnityEngine;
using System.Collections;

public class Enums : ScriptableObject {

    public enum WarpDriveState
    {
        off,
        charging,
        on,
        waitingForCommand
    }

    public enum MiniGameResult
    {
        perfect,
        good,
        bad
    }

    public enum Allegiance
    {
        Friendly,
        Enemy,
        Inanimate
    }
}
