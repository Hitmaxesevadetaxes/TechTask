using UnityEngine;

public enum ItemType { SpeedBuff, SizeBuff }

public class CollectableItem : MonoBehaviour, ICollectable
{
    private ItemType _type;
    private GameConfig _config;

    // We call this immediately after spawning to set up the item
    public void Configure(ItemType type, GameConfig config)
    {
        _type = type;
        _config = config;

        // Set Color based on type
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = (_type == ItemType.SpeedBuff)
                ? _config.SpeedItemColor
                : _config.SizeItemColor;
        }
    }

    public void Collect(PlayerController player)
    {
        if (_type == ItemType.SpeedBuff)
        {
            // Yellow Logic: Score +1, Speed x2
            GameEvents.OnScoreChanged?.Invoke(_config.SpeedItemScore);
            player.ApplySpeedBuff(_config.SpeedMultiplier, _config.SpeedBuffDuration);
        }
        else
        {
            // Blue Logic: Score +2, Speed x0.5, Size x3
            GameEvents.OnScoreChanged?.Invoke(_config.SizeItemScore);
            player.ApplySizeBuff(_config.SizeMultiplier, _config.SpeedDebuffMultiplier, _config.SizeBuffDuration);
        }

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Collect(player);
            }
        }
    }

}