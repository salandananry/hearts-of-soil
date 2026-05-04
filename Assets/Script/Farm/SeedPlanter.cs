using UnityEngine;
using UnityEngine.Tilemaps;

public class SeedPlanter : MonoBehaviour
{
    [SerializeField] private Tilemap soilTileMap;
    [SerializeField] private TileCheck tileCheck;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private GameObject seedPrefab;
    private GameObject currentSeed;

    private void Start()
    { /*
        check each tile if:
        has soil tile
        has plant

        if has plant, then spawn a seed to that target cell
        */
    }
    public void PlantSeed(Vector2 position)
    {
        Vector3Int targetTile = tileCheck.GetTileAtPosition(position);

        if (!tileCheck.HasSoilTile(targetTile)) return;
        if (tileCheck.HasPlant(targetTile)) return;

        currentSeed = Instantiate(seedPrefab, transform);
        currentSeed.transform.position = SpawnPosition(targetTile);

        UpdateTileStatus(targetTile);
    }
    private void UpdateTileStatus(Vector3Int targetTile)
    {
        tileManager.SetTileAvailability(true, targetTile);
    }
    private Vector3 SpawnPosition(Vector3Int targetTile)
    {
        Vector3 spawnPos = soilTileMap.GetCellCenterWorld(targetTile);
        spawnPos.y += 0.1f;
        
        return spawnPos;
    }
}