using UnityEngine;
using UnityEngine.Tilemaps;

public class SeedPlanter : MonoBehaviour
{
    [SerializeField] private Tilemap soilTileMap;
    [SerializeField] private TileCheck tileCheck;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private GameObject seedPrefab;
    private GameObject currentSeed;
    public void GoPlant(Vector2 position)
    {
        Vector3Int cell = soilTileMap.WorldToCell(position);

        if (!soilTileMap.HasTile(cell)) return;
        if (tileCheck.HasPlant(cell)) return;

        Vector3 spawnPos = soilTileMap.GetCellCenterWorld(cell);
        spawnPos.y += 0.1f;

        currentSeed = Instantiate(seedPrefab, transform);
        currentSeed.transform.position = spawnPos;

        UpdateTileStatus(cell);
    }
    private void UpdateTileStatus(Vector3Int position)
    {
        tileManager.SetTileAvailability(true, position);
    }
}