using System;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public event Action <Vector3Int> OnPlantCreated;
    public event Action<Vector3Int> OnSoilWatered;

    public event Action OnHarvestPlant;

    private FarmMechanics farmMechanics;

    public void Initialize(FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
    }

    #region FARM ACTION
    public void RestorePlants()
    {
        var farmUtility = farmMechanics.TileUtility;
        farmUtility.ForEachTile(targetTile =>
        {
            if (!farmUtility.HasTile(targetTile)) return;
            if (!HasPlant(targetTile)) return;

            OnPlantCreated?.Invoke(targetTile);
        });
    }
    public void SpawnNewPlant(Vector3Int targetTile)
    {
        var farmUtility = farmMechanics.TileUtility;

        if (!farmUtility.HasTile(targetTile)) return;
        if (HasPlant(targetTile)) return;

        SetHasPlant(targetTile, true);
        SetGrowthStage(targetTile, 0);
        OnPlantCreated?.Invoke(targetTile);
    }
    public void HarvestPlant(Vector3Int targetTile)
    {
        SetHasPlant(targetTile, false);
        SetGrowthStage(targetTile, 0);
        OnHarvestPlant?.Invoke();
    }
    public void RestoreWateredSoil()
    {
        var farmUtility = farmMechanics.TileUtility;
        farmUtility.ForEachTile(targetTile =>
        {
            if (!farmUtility.HasTile(targetTile)) return;
            if (!IsWatered(targetTile)) return;

            OnSoilWatered?.Invoke(targetTile);
        });
    }
    public void WaterSoil(Vector3Int targetTile)
    {
        var farmUtility = farmMechanics.TileUtility;

        if (!farmUtility.HasTile(targetTile)) return;
        if (!HasPlant(targetTile)) return;
        if (IsWatered(targetTile)) return;

        SetHasWater(targetTile, true);
        OnSoilWatered?.Invoke(targetTile);
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
}