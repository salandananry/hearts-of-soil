using System;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    #region FIELDS
    public event Action <Vector3Int> OnPlantCreated;
    public event Action <Vector3Int> OnSoilWatered;

    public event Action <Vector3Int> OnHarvestPlant;

    private FarmMechanics farmMechanics;
    #endregion

    #region INITIALIZE
    public void Initialize(FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
    }
    #endregion

    #region FARM ACTION
    public void RestorePlants()
    {
        var farmUtility = farmMechanics.farmTileUtility;
        farmUtility.ForEachTile(targetTile =>
        {
            if (!farmUtility.HasTile(targetTile)) return;
            if (!HasPlant(targetTile)) return;

            OnPlantCreated?.Invoke(targetTile);
        });
    }
    public void SpawnNewPlant(Vector3Int targetTile)
    {
        var farmUtility = farmMechanics.farmTileUtility;

        if (!farmUtility.HasTile(targetTile)) return;
        if (HasPlant(targetTile)) return;

        SetHasPlant(targetTile, true);
        SetGrowthStage(targetTile, 0);
        OnPlantCreated?.Invoke(targetTile);
    }
    public void RestoreWateredSoil()
    {
        var farmUtility = farmMechanics.farmTileUtility;
        farmUtility.ForEachTile(targetTile =>
        {
            if (!farmUtility.HasTile(targetTile)) return;
            if (!IsWatered(targetTile)) return;

            OnSoilWatered?.Invoke(targetTile);
        });
    }
    public void WaterSoil(Vector3Int targetTile)
    {
        var farmUtility = farmMechanics.farmTileUtility;

        if (!farmUtility.HasTile(targetTile)) return;
        if (!HasPlant(targetTile)) return;
        if (IsWatered(targetTile)) return;

        SetHasWater(targetTile, true);
        OnSoilWatered?.Invoke(targetTile);
    }
    public void HarvestPlant(Vector3Int targetTile)
    {
        var farmUtility = farmMechanics.farmTileUtility;

        if (!farmUtility.HasTile(targetTile)) return;
        if (!HasPlant(targetTile)) return;
        if (!IsFullyGrown(targetTile)) return;

        SetHasPlant(targetTile, false);
        SetHasWater(targetTile, false);
        SetGrowthStage(targetTile, 0);
        SetGrowthTimer(targetTile, 0);
        SetFullyGrown(targetTile, false);
        OnHarvestPlant?.Invoke(targetTile);
    }
    #endregion

    #region SET
    public void SetHasPlant(Vector3Int targetTile, bool hasPlant)
    {
        GetOrCreateTile(targetTile).hasPlant = hasPlant;
    }

    public void SetHasWater(Vector3Int targetTile, bool hasWater)
    {
        GetOrCreateTile(targetTile).isWatered = hasWater;
    }
    public void SetGrowthStage(Vector3Int targetTile, int growthStage)
    {
        GetOrCreateTile(targetTile).growthStage = growthStage;
    }
    public void SetGrowthTimer(Vector3Int targetTile, float growthTimer)
    {
        GetOrCreateTile(targetTile).growthTimer = growthTimer;
    }
    public void SetFullyGrown(Vector3Int targetTile, bool isFullyGrown)
    {
        GetOrCreateTile(targetTile).isFullyGrown = isFullyGrown;
    }
    #endregion

    #region CHECK

    public bool HasPlant(Vector3Int targetTile)
    {
        var save = SaveManager.instance.CurrentSave;

        return save.tileDict.TryGetValue(targetTile, out var tile) &&
               tile.hasPlant;
    }

    public bool IsWatered(Vector3Int targetTile)
    {
        var save = SaveManager.instance.CurrentSave;

        return save.tileDict.TryGetValue(targetTile, out var tile) &&
               tile.isWatered;
    }
    public bool IsFullyGrown(Vector3Int targetTile)
    {
        var save = SaveManager.instance.CurrentSave;

        return save.tileDict.TryGetValue(targetTile, out var tile) &&
               tile.isFullyGrown;
    }
    #endregion

    #region HELPER
    private TileSaveData GetOrCreateTile(Vector3Int targetTile)
    {
        var save = SaveManager.instance.CurrentSave;
        if (!save.tileDict.TryGetValue(targetTile, out var tile))
        {
            tile = new TileSaveData
            {
                position = targetTile
            };

            save.tileDict[targetTile] = tile;
        }
        return tile;
    }
    #endregion
}