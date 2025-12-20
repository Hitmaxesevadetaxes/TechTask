using System;

public static class GameEvents
{
    // Logic Events
    public static Action<int> OnScoreChanged;
    public static Action OnAllTargetsCollected;

    // Game Flow Events
    public static Action OnGameStart;
    public static Action OnGameRestart;
}