using UnityEngine;
using UnityEngine.Tilemaps;

public class TileUtility : MonoBehaviour
{
    [SerializeField] private TileMapController tileMapController;

    private Tilemap SoilTileMap => tileMapController.GetSoilTileMap();

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return SoilTileMap.WorldToCell(worldPosition);
    }

    public Vector3 CellToWorldCenter(Vector3Int cell)
    {
        return SoilTileMap.GetCellCenterWorld(cell);
    }

    public Vector3 GetSpawnPosition(Vector3Int cell, float yOffset = 0.1f)
    {
        Vector3 pos = SoilTileMap.GetCellCenterWorld(cell);
        pos.y += yOffset;
        return pos;
    }

    public bool HasTile(Vector3Int cell)
    {
        return SoilTileMap.HasTile(cell);
    }

    public BoundsInt GetBounds()
    {
        return SoilTileMap.cellBounds;
    }
    public void ForEachTile(System.Action<Vector3Int> action)
    {
        BoundsInt bounds = GetBounds();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                action(new Vector3Int(x, y, 0));
            }
        }
    }
}
