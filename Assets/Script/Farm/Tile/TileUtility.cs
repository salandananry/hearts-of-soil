using UnityEngine;
using UnityEngine.Tilemaps;

public class TileUtility : MonoBehaviour
{
    private TileMapController tileMapController;
    private Tilemap targetTileMap => tileMapController.GetTargetTileMap();

    private void Awake()
    {
        tileMapController = GetComponent<TileMapController>();
    }
    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return targetTileMap.WorldToCell(worldPosition);
    }

    public Vector3 GetTileWorldPosition(Vector3Int cell, float xOffset, float yOffset)
    {
        Vector3 pos = targetTileMap.GetCellCenterWorld(cell);
        pos.x += xOffset;
        pos.y += yOffset;
        return pos;
    }

    public bool HasTile(Vector3Int cell)
    {
        return targetTileMap.HasTile(cell);
    }

    public BoundsInt GetBounds()
    {
        return targetTileMap.cellBounds;
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
