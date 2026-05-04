using UnityEngine;
using UnityEngine.Tilemaps;

public class SeedPlanter : MonoBehaviour
{
    [SerializeField] private Tilemap soilTileMap;
    [SerializeField] private TileCheck tileCheck;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private GameObject seedPrefab;

    private void Start()
    {
        SpawnExistingPlants();
    }
    private void SpawnExistingPlants()
    {
        BoundsInt bounds = soilTileMap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int targetTile = new Vector3Int(x, y, 0);

                if (!HasTile(targetTile)) continue;
                if (!HasPlant(targetTile)) continue;

                SpawnSeed(targetTile);
            }
        }
    }
    public void PlantSeed(Vector2 position)
    {
        Vector3Int targetTile = GetTileAtPosition(position);

        if (!HasTile(targetTile)) return;
        if (HasPlant(targetTile)) return;

        SpawnSeed(targetTile);
        tileManager.SetHasPlant(targetTile, true);
    }
    private Vector3 SpawnPosition(Vector3Int targetTile)
    {
        Vector3 spawnPos = soilTileMap.GetCellCenterWorld(targetTile);
        spawnPos.y += 0.1f;

        return spawnPos;
    }
    private void SpawnSeed(Vector3Int targetTile)
    {
        Instantiate(seedPrefab, SpawnPosition(targetTile), Quaternion.identity, transform);
    }
    private bool HasTile(Vector3Int targetTile)
    {
        return soilTileMap.HasTile(targetTile);
    }
    private bool HasPlant(Vector3Int targetTile)
    {
        return tileManager.HasPlant(targetTile);
    }
    public Vector3Int GetTileAtPosition(Vector3 playerPosition)
    {
        return soilTileMap.WorldToCell(playerPosition);
    }
}