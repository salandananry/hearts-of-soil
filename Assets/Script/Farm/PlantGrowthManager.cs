using System;
using UnityEngine;

public class PlantGrowthManager : MonoBehaviour
{
    [Header("Script reference")]
    [SerializeField] private TileManager tileManager;

    [Header("Growth settings")]
    [SerializeField] private int plantMaxStage;
    [SerializeField] private Sprite[] plantStageVariants;

    private float growthInterval = 60f;
    public event Action<Vector3Int> OnGrowthStageChanged;

    private void Update()
    {
        GrowthTimer();
    }
    private void GrowthTimer()
    {
        var tiles = SaveManager.Instance.CurrentSave.tiles;
        foreach (var tile in tiles)
        {
            if (!tile.hasPlant) continue;
            if (!tile.isWatered) continue;

            tile.growthTimer += Time.deltaTime;

            if (tile.growthTimer < growthInterval) continue;
            if (tile.growthStage >= plantMaxStage) continue;

            tile.growthTimer = 0f;
            tile.growthStage++;
            OnGrowthStageChanged?.Invoke(tile.position);
            tileManager.SetGrowthStage(tile.position, tile.growthStage);
        }
    }
    public Sprite GetPlantSprite(int growthStage)
    {
        if (growthStage < 0 || growthStage >= plantStageVariants.Length)
            return null;

        return plantStageVariants[growthStage];
    }
}