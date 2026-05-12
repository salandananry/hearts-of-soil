using UnityEngine;

public class TileManager : MonoBehaviour
{
    #region SET

    public void SetHasPlant(Vector3Int targetTile, bool hasPlant)
    {
        var save = SaveManager.instance.CurrentSave;

        if (!save.tileDict.TryGetValue(targetTile, out var tile))
        {
            tile = new TileSaveData { position = targetTile };
            save.tileDict[targetTile] = tile;
        }

        tile.hasPlant = hasPlant;

        //SaveManager.Instance.SaveGame();
    }

    public void SetHasWater(Vector3Int targetTile, bool hasWater)
    {
        var save = SaveManager.instance.CurrentSave;

        if (!save.tileDict.TryGetValue(targetTile, out var tile))
        {
            tile = new TileSaveData { position = targetTile };
            save.tileDict[targetTile] = tile;
        }

        tile.isWatered = hasWater;

        //SaveManager.Instance.SaveGame();
    }

    public void SetGrowthStage(Vector3Int targetTile, int growthStage)
    {
        var save = SaveManager.instance.CurrentSave;

        if (!save.tileDict.TryGetValue(targetTile, out var tile))
        {
            tile = new TileSaveData { position = targetTile };
            save.tileDict[targetTile] = tile;
        }

        tile.growthStage = growthStage;

        //SaveManager.Instance.SaveGame();
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

    #endregion

    #region GET

    public int GetGrowthStage(Vector3Int targetTile)
    {
        var save = SaveManager.instance.CurrentSave;

        if (save.tileDict.TryGetValue(targetTile, out var tile))
            return tile.growthStage;

        return 0;
    }

    #endregion
}