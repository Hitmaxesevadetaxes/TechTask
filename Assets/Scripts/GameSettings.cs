using UnityEngine;


[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Level Settings")]
    [Tooltip("Width and Length of the spawn area (X and Z axis).")]
    public Vector2 SpawnAreaSize = new Vector2(20f, 20f);

    [Tooltip("Total number of collectable items to spawn.")]
    [Min(1)]
    public int ItemsCount = 10; 

    [Header("Player Base Stats")]
    [Tooltip("Default movement speed of the player.")]
    [Range(1f, 50f)]
    public float PlayerBaseSpeed = 5f; 

    [Tooltip("Default scale of the player (uniform scale).")]
    [Range(0.1f, 5f)]
    public float PlayerBaseSize = 1f;

    // --- YELLOW ITEM SETTINGS ---
    [Header("Speed Buff Item (Yellow)")]
    public Color SpeedItemColor = Color.yellow;

    
    [Tooltip("Score points awarded for collecting yellow item.")]
    public int SpeedItemScore = 1;

    [Tooltip("Duration of the speed boost in seconds.")]
    public float SpeedBuffDuration = 10f;

    [Tooltip("Multiplier applied to base speed (e.g., 2 for x2 speed).")]
    public float SpeedMultiplier = 2f;

    // --- BLUE ITEM SETTINGS ---
    [Header("Size Buff Item (Blue)")]
    public Color SizeItemColor = Color.blue;

    [Tooltip("Score points awarded for collecting blue item.")]
    public int SizeItemScore = 2;

    [Tooltip("Duration of the size/debuff effect in seconds.")]
    public float SizeBuffDuration = 10f;

    [Tooltip("Multiplier applied to base speed (e.g., 0.5 for half speed).")]
    public float SpeedDebuffMultiplier = 0.5f;

    [Tooltip("Multiplier applied to base size (e.g., 3 for x3 size).")]
    public float SizeMultiplier = 3f;
}