using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSession : MonoBehaviour
{
    [Header("Config")]
    public GameConfig config;
    public GameObject itemPrefab;
    public PlayerController player;

    private int _collectedCount;
    private int _currentGlobalScore;
    private float _startTime;
    private List<GameObject> _activeItems = new List<GameObject>();

    private void OnEnable()
    {
        Debug.Log("GameSession: Enabled and Listening for Events.");
        GameEvents.OnGameStart += StartLevel;
        GameEvents.OnGameRestart += RestartLevel;
        GameEvents.OnScoreChanged += HandleScoreChange;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= StartLevel;
        GameEvents.OnGameRestart -= RestartLevel;
        GameEvents.OnScoreChanged -= HandleScoreChange;
    }

    public void StartLevel()
    {
        Debug.Log("GameSession: EVENT RECEIVED! Starting Level...");

        // 1. Check References
        if (config == null)
        {
            Debug.LogError("CRITICAL ERROR: GameConfig is MISSING in GameSession Inspector!");
            return;
        }
        if (itemPrefab == null)
        {
            Debug.LogError("CRITICAL ERROR: Item Prefab is MISSING in GameSession Inspector!");
            return;
        }

        // 2. Reset State
        _collectedCount = 0;
        _currentGlobalScore = 0;
        _startTime = Time.time;

        if (player != null) player.ResetStats();

        // 3. Spawn
        ClearItems();
        SpawnItems();
    }

    private void SpawnItems()
    {
        Debug.Log($"GameSession: Attempting to spawn {config.ItemsCount} items...");

        for (int i = 0; i < config.ItemsCount; i++)
        {
            float rangeX = config.SpawnAreaSize.x / 2f;
            float rangeZ = config.SpawnAreaSize.y / 2f;

            Vector3 pos = new Vector3(
                Random.Range(-rangeX, rangeX),
                0.5f,
                Random.Range(-rangeZ, rangeZ)
            );

            GameObject newItem = Instantiate(itemPrefab, pos, Quaternion.identity);
            _activeItems.Add(newItem);

            // Configure
            CollectableItem itemScript = newItem.GetComponent<CollectableItem>();
            if (itemScript == null) itemScript = newItem.AddComponent<CollectableItem>();

            ItemType type = Random.value > 0.5f ? ItemType.SpeedBuff : ItemType.SizeBuff;
            itemScript.Configure(type, config);
        }

        Debug.Log("GameSession: Items Spawned Successfully!");
    }

    private void RestartLevel() { StartLevel(); }

    private void HandleScoreChange(int amount)
    {
        _collectedCount++;
        _currentGlobalScore += amount;
        if (_collectedCount >= config.ItemsCount) FinishLevel();
    }

    private void FinishLevel()
    {
        GameEvents.OnAllTargetsCollected?.Invoke();
    }

    private void ClearItems()
    {
        foreach (var item in _activeItems) if (item != null) Destroy(item);
        _activeItems.Clear();
    }
}