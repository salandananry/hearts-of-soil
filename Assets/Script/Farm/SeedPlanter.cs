using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    [Header("Script reference:")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileUtility tileUtility;

    [Header("Seed prefab:")]
    [SerializeField] private GameObject seedPrefab;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;

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
    public void PlantSeed()
    {
        Vector3 pos = playerObject.transform.position;
        Vector3Int targetTile = tileUtility.WorldToCell(pos);

        if (!HasTile(targetTile)) return;
        if (HasPlant(targetTile)) return;

        SpawnSeed(targetTile);
        tileManager.SetHasPlant(targetTile, true);
    }
    private void SpawnSeed(Vector3Int targetTile)
    {
        Instantiate(seedPrefab, tileUtility.GetTileWorldPosition(targetTile, 0f, 0.1f), Quaternion.identity, transform);
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