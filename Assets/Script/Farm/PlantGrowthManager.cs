using System;
using UnityEngine;

public class PlantGrowthManager : MonoBehaviour
{
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
        var tiles = SaveManager.instance.CurrentSave.tileDict.Values;
        foreach (var tile in tiles)
        {
            if (!tile.hasPlant) continue;
            if (!tile.isWatered) continue;

            tile.growthTimer += Time.deltaTime;

            if (tile.growthTimer < growthInterval) continue;
            if (tile.growthStage >= plantMaxStage) continue;

            tile.growthTimer = 0f;
            tile.growthStage++;

            if (tile.growthStage >= plantMaxStage)
            {
                tile.isFullyGrown = true;         
            }
            else
            {
                tile.isFullyGrown = false;
            }
            OnGrowthStageChanged?.Invoke(tile.position);
        }
    }
    public Sprite GetPlantSprite(int growthStage)
    {
        if (growthStage < 0 || growthStage >= plantStageVariants.Length)
            return null;

        return plantStageVariants[growthStage];
    }
    public int GetGrowthStage(Vector3Int targetTile)
    {
        var save = SaveManager.instance.CurrentSave;

        if (save.tileDict.TryGetValue(targetTile, out var tile))
            return tile.growthStage;

        return 0;
    }
    public Sprite GetPlantSprite(Vector3Int targetTile)
    {
        return GetPlantSprite(GetGrowthStage(targetTile));
    }
}