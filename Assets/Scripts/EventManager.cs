using System;

/// <summary>
/// Centralized events for decoupling game flow and logic.
/// </summary>
public static class EventManager
{
    public static Action OnGameStart;
    public static Action OnGameOver;
    public static Action OnScore;
}