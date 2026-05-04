using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    [SerializeField] private Tilemap soilTileMap;

    public void SetTileSprite(Vector3Int targetTile, IsometricRuleTile targetSprite)
    {
        soilTileMap.SetTile(targetTile, targetSprite);
    }
    public bool HasTile(Vector3Int targetTile)
    {
        return soilTileMap.HasTile(targetTile);
    }
    public Tilemap GetSoilTileMap()
    {
        return soilTileMap;
    }
}
