using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OrigamiEvents
{
    public delegate void GameEvent();
    public static GameEvent onPlayerSpawned_Event;
    public static GameEvent onPlayerDeath_Event;

    public delegate void StateChangeEvent(OrigamiState _state);
    public static StateChangeEvent onStateChange_Event;

    public static void SendStateChangeEvent(OrigamiState _state)
    {
        if (onStateChange_Event != null)
            onStateChange_Event(_state);
    }

    public static void SendPlayerDeathEvent()
    {
        if (onPlayerDeath_Event != null)
            onPlayerDeath_Event();
    }
}

