using System;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    private FarmMechanics farmMechanics;

    [Header("Plant prefab:")]
    [SerializeField] private GameObject plantPrefab;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;

    public event Action<Vector3Int> OnTilePlanted;

    private Dictionary<Vector3Int, GameObject> spawnedPlants =
    new Dictionary<Vector3Int, GameObject>();
    public void Initalize(
        FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
        SpawnExistingPlants();
        SubscribeEvents();
    }
    #region SPAWN EXISTING PLANTS
    private void SpawnExistingPlants()
    {
        farmMechanics.TileUtility.ForEachTile(targetTile =>
        {
            if (!farmMechanics.TileUtility.HasTile(targetTile)) return;
            if (!farmMechanics.TileManager.HasPlant(targetTile)) return;

            SpawnSeed(targetTile);
        });
    }
    #endregion

    #region PLANTING MECHANIC
    public void PlantSeed()
    {
        Vector3 pos = playerObject.transform.position;
        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(pos);

        if (!farmMechanics.TileUtility.HasTile(targetTile)) return;
        if (farmMechanics.TileManager.HasPlant(targetTile)) return;

        farmMechanics.TileManager.SetHasPlant(targetTile, true);
        farmMechanics.TileManager.SetGrowthStage(targetTile, 0);

        SpawnSeed(targetTile);

        OnTilePlanted?.Invoke(targetTile);
    }
    #endregion
    private void SpawnSeed(Vector3Int targetTile)
    {
        GameObject plant = Instantiate(
            plantPrefab,
            farmMechanics.TileUtility.GetTileWorldPosition(targetTile, 0f, 0f),
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

        int growthStage = farmMechanics.TileManager.GetGrowthStage(targetTile);
        Sprite sprite = farmMechanics.PlantGrowthManager.GetPlantSprite(growthStage);

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
    private void SubscribeEvents()
    {
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += UpdatePlantSprite;
    }
    private void OnDestroy()
    {
        if (farmMechanics == null) return;

        if(farmMechanics.PlantGrowthManager != null)
           farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= UpdatePlantSprite;
    }
}