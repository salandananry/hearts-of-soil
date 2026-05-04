using UnityEngine;

public class TileManager : MonoBehaviour
{
    public void SetHasPlant(Vector3Int targetTile, bool hasPlant)
    {
        var save = SaveManager.Instance.CurrentSave;

        var tile = save.tiles.Find(t => t.position == targetTile);

        if (tile != null)
        {
            tile.hasPlant = hasPlant;
        }
        else
        {
            save.tiles.Add(new TileSaveData
            {
                position = targetTile,
                hasPlant = hasPlant
            });
        }

        SaveManager.Instance.SaveGame();
    }
    public bool HasPlant(Vector3Int targetTile)
    {
        var save = SaveManager.Instance.CurrentSave;
        var tile = save.tiles.Find(t => t.position == targetTile);

        if (tile == null) return false;

        return tile.hasPlant;
    }
    public bool IsWatered(Vector3Int targetTile)
    {
        var save = SaveManager.Instance.CurrentSave;
        var tile = save.tiles.Find(t => t.position == targetTile);

        if (tile == null) return false;

        return tile.isWatered;
    }
}