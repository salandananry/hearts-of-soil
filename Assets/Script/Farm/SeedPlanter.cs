using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileUtility tileUtility;
    [SerializeField] private GameObject seedPrefab;

    private void Start()
    {
        SpawnExistingPlants();
    }
    private void SpawnExistingPlants()
    {
        tileUtility.ForEachTile(targetTile =>
        {
            if (!HasTile(targetTile)) return;
            if (!HasPlant(targetTile)) return;

            SpawnSeed(targetTile);
        });
    }
    public void PlantSeed(Vector2 position)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(position);

        if (!HasTile(targetTile)) return;
        if (HasPlant(targetTile)) return;

        SpawnSeed(targetTile);
        tileManager.SetHasPlant(targetTile, true);
    }
    private void SpawnSeed(Vector3Int targetTile)
    {
        Instantiate(seedPrefab, tileUtility.GetSpawnPosition(targetTile), Quaternion.identity, transform);
    }
    private bool HasPlant(Vector3Int targetTile)
    {
        return tileManager.HasPlant(targetTile);
    }
    private bool HasTile(Vector3Int targetTile)
    {
        return tileUtility.HasTile(targetTile);
    }
}