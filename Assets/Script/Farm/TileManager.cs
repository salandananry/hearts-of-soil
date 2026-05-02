using UnityEngine;

public class TileManager : MonoBehaviour
{
    private void Start()
    {
        
    }
    public void SetTileAvailability(bool hasPlant , Vector3Int tilePosition)
    {
        var save = SaveManager.Instance.CurrentSave;

        var tile = save.tiles.Find(t => t.position == tilePosition);

        if (tile != null)
        {
            tile.hasPlant = hasPlant;
        }
        else
        {
            save.tiles.Add(new TileSaveData
            {
                position = tilePosition,
                hasPlant = hasPlant
            });
        }

        SaveManager.Instance.SaveGame();
    }
    public bool HasPlant(Vector3Int tilePosition)
    {
        var save = SaveManager.Instance.CurrentSave;

        var tile = save.tiles.Find(t => t.position == tilePosition);

        if (tile == null)
            return false;

        return tile.hasPlant;
    }
}