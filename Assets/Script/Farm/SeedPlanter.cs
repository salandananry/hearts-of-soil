using UnityEngine;
using System;

public class SeedPlanter : MonoBehaviour
{
    [Header("Script reference:")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileUtility tileUtility;

    [Header("Seed prefab:")]
    [SerializeField] private GameObject seedPrefab;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;

    public event Action<Vector3Int> OnTilePlanted;

    #region Spawn plants from save
    private void Start()
    {
        SpawnExistingPlants();
    }
    private void SpawnExistingPlants()
    {
        tileUtility.ForEachTile(targetTile =>
        {
            if (!tileUtility.HasTile(targetTile)) return;
            if (!tileManager.HasPlant(targetTile)) return;

            SpawnSeed(targetTile);
        });
    }
    #endregion

    #region Planting mechanic
    public void PlantSeed()
    {
        Vector3 pos = playerObject.transform.position;
        Vector3Int targetTile = tileUtility.WorldToCell(pos);

        if (!tileUtility.HasTile(targetTile)) return;
        if (tileManager.HasPlant(targetTile)) return;

        SpawnSeed(targetTile);
        tileManager.SetHasPlant(targetTile, true);

        OnTilePlanted?.Invoke(targetTile);
    }
    #endregion
    private void SpawnSeed(Vector3Int targetTile)
    {
        Instantiate(seedPrefab, tileUtility.GetTileWorldPosition(targetTile, 0f, 0f), Quaternion.identity, transform);
    }
}