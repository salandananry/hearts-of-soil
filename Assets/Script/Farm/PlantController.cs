using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private TileStatusController tileStatusController;

    [Header("Plant prefab:")]
    [SerializeField] private GameObject plantPrefab;

    private Dictionary<Vector3Int, GameObject> spawnedPlants =
    new Dictionary<Vector3Int, GameObject>();
    #endregion

    #region INITIALIZE
    public void Initalize(
        FarmMechanics farmMechanics,
        TileStatusController tileStatusController)
    {
        this.farmMechanics = farmMechanics;
        this.tileStatusController = tileStatusController;
        SubscribeEvents();
        LoadExistingPlants();
    }
    #endregion

    #region LOAD EXISTING STATE
    private void LoadExistingPlants()
    {
        farmMechanics.FarmManager.RestorePlants();
    }
    #endregion
    public void PlantSelectedTile()
    {
        Vector3Int targetTile = tileStatusController.CurrentViewedTile;
        farmMechanics.FarmManager.SpawnNewPlant(targetTile);
    }
    #region EVENT HANDLERS
    private void OnSeedPlantHandler(Vector3Int targetTile)
    {
        GameObject plant = Instantiate(
            plantPrefab,
            farmMechanics.TileUtility.GetTileWorldPosition(targetTile, 0f, 0f),
            Quaternion.identity,
            transform
         );

        spawnedPlants[targetTile] = plant;
        UpdatePlantVisual(targetTile);
    }
    private void UpdatePlantVisual(Vector3Int targetTile)
    {
        if (!spawnedPlants.TryGetValue(targetTile, out GameObject plant))
            return;

        GameObject newPlant =
            farmMechanics.PlantGrowthManager.GetPlantPrefabAtTile(targetTile);

        if (newPlant == null)
            return;

        Vector3 position =
            farmMechanics.TileUtility.GetTileWorldPosition(
                targetTile,
                0f,
                0f
            );

        Destroy(plant);

        GameObject newPlantInstance = Instantiate(
            newPlant,
            position,
            Quaternion.identity,
            transform
        );

        spawnedPlants[targetTile] = newPlantInstance;
    }
    private void RemovePlantAt(Vector3Int targetTile)
    {
        if (!spawnedPlants.TryGetValue(targetTile, out GameObject plant))
            return;

        Destroy(plant);
        spawnedPlants.Remove(targetTile);
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        farmMechanics.FarmManager.OnPlantCreated += OnSeedPlantHandler;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += UpdatePlantVisual;
        farmMechanics.FarmManager.OnHarvestPlant += RemovePlantAt;
    }
    private void OnDestroy()
    {
        farmMechanics.FarmManager.OnPlantCreated -= OnSeedPlantHandler;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= UpdatePlantVisual;
        farmMechanics.FarmManager.OnHarvestPlant -= RemovePlantAt;
    }
    #endregion
}