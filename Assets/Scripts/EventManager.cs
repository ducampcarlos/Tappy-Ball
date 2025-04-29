using System;

/// <summary>
/// Centralized events for decoupling game flow, scoring, power-ups, and magnet effects.
/// </summary>
public static class EventManager
{
    public static Action OnGameStart;
    public static Action OnGameOver;
    public static Action OnScore;
    public static Action OnCollect;
    public static Action OnCollectableMiss;
    public static Action<float> OnMagnetStart;  // radius parameter
    public static Action OnMagnetEnd;
}