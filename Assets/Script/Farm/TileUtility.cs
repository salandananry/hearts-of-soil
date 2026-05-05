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

    public Vector3 GetTileWorldPosition(Vector3Int cell, float xOffset, float yOffset)
    {
        Vector3 pos = SoilTileMap.GetCellCenterWorld(cell);
        pos.x += xOffset;
        pos.y += yOffset;
        return pos;
    }

    public bool HasTile(Vector3Int targetTile)
    {
        return SoilTileMap.HasTile(targetTile);
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
