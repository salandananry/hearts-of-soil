using System;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    [Header("Script reference:")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileUtility tileUtility;
    [SerializeField] private PlantGrowthManager plantGrowthManager;

    [Header("Plant prefab:")]
    [SerializeField] private GameObject plantPrefab;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;

    public event Action<Vector3Int> OnTilePlanted;

    private Dictionary<Vector3Int, GameObject> spawnedPlants =
    new Dictionary<Vector3Int, GameObject>();

    #region Spawn plants from save
    private void Start()
    {
        SpawnExistingPlants();
    }
    private void OnEnable()
    {
        plantGrowthManager.OnGrowthStageChanged += UpdatePlantSprite;
    }

    private void OnDisable()
    {
        plantGrowthManager.OnGrowthStageChanged -= UpdatePlantSprite;
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

        tileManager.SetHasPlant(targetTile, true);
        tileManager.SetGrowthStage(targetTile, 0);

        SpawnSeed(targetTile);

        OnTilePlanted?.Invoke(targetTile);
    }
    #endregion
    private void SpawnSeed(Vector3Int targetTile)
    {
        GameObject plant = Instantiate(
            plantPrefab,
            tileUtility.GetTileWorldPosition(targetTile, 0f, 0f),
            Quaternion.identity,
            transform
         );

        spawnedPlants[targetTile] = plant;

        UpdatePlantSprite(targetTile);
    }
    private void UpdatePlantSprite(Vector3Int targetTile)
    {
        if (!spawnedPlants.TryGetValue(targetTile, out GameObject plant)) return;

        SpriteRenderer spriteRenderer = plant.GetComponent<SpriteRenderer>();

        int growthStage = tileManager.GetGrowthStage(targetTile);
        Sprite sprite = plantGrowthManager.GetPlantSprite(growthStage);

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}