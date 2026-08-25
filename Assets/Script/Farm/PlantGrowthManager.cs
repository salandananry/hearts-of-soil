using System;
using UnityEngine;

public class PlantGrowthManager : MonoBehaviour
{
    [Header("Growth settings")]
    [SerializeField] private int plantMaxStage;
    [SerializeField] private GameObject[] plantStageVariants;

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
    public GameObject GetPlantPrefabAtTile(Vector3Int targetTile)
    {
        int growthStage = GetGrowthStage(targetTile);

        if (growthStage < 0 || growthStage >= plantStageVariants.Length)
            return null;

        return plantStageVariants[growthStage];
    }
    public Sprite GetPlantSpriteAtTile(Vector3Int targetTile)
    {
        GameObject plantPrefab = GetPlantPrefabAtTile(targetTile);

        if (plantPrefab == null)
            return null;

        SpriteRenderer spriteRenderer =
            plantPrefab.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            return null;

        return spriteRenderer.sprite;
    }
    public int GetGrowthStage(Vector3Int targetTile)
    {
        var save = SaveManager.instance.CurrentSave;

        if (save.tileDict.TryGetValue(targetTile, out var tile))
            return tile.growthStage;

        return 0;
    }
}