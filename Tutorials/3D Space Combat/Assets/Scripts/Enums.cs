using UnityEngine;
using System.Collections;

public class Enums : ScriptableObject {

    public enum WARP_DRIVE_STATE
    {
        off,
        charging,
        on,
        waitingForCommand
    }

    public enum MINI_GAME_RESULT
    {
        perfect,
        good,
        bad
    }
}
