using UnityEngine;

public class TileManager : MonoBehaviour
{
    public void SetTileAvailability(bool hasPlant , Vector3Int tilePosition)
    {
        var save = SaveManager.Instance.CurrentSave;

        var tile = save.tiles.Find(t => t.position == tilePosition);

        if (tile != null)
        {
            tile.isEmpty = hasPlant;
        }
        else
        {
            save.tiles.Add(new TileSaveData
            {
                position = tilePosition,
                isEmpty = hasPlant
            });
        }

        SaveManager.Instance.SaveGame();
    }
    public bool HasPlant(Vector3Int tilePosition)
    {
        var save = SaveManager.Instance.CurrentSave;
        var tile = save.tiles.Find(t => t.position == tilePosition);

        if (tile == null) return false;

        return tile.isEmpty;
    }
    public bool IsWatered(Vector3Int tilePosition)
    {
        var save = SaveManager.Instance.CurrentSave;
        var tile = save.tiles.Find(t => t.position == tilePosition);

        if (tile == null) return false;

        return tile.isWatered;
    }
}