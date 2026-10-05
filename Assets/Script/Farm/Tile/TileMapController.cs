using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    private Tilemap targetTileMap;

    private void Awake()
    {
        targetTileMap = GetComponent<Tilemap>();
    }
    public void SetTile(Vector3Int targetTile, IsometricRuleTile targetSprite)
    {
        targetTileMap.SetTile(targetTile, targetSprite);
    }
    public Tilemap GetTargetTileMap()
    {
        return targetTileMap;
    }
}