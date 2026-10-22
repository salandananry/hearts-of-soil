using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private TileStatusController farmTileStatusController;

    [Header("Plant prefab:")]
    [SerializeField] private GameObject plantPrefab;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;

    private Dictionary<Vector3Int, GameObject> spawnedPlants =
    new Dictionary<Vector3Int, GameObject>();
    #endregion

    #region INITIALIZE
    public void Initalize(
        FarmMechanics farmMechanics,
        TileStatusController farmTileStatusController)
    {
        this.farmMechanics = farmMechanics;
        this.farmTileStatusController = farmTileStatusController;
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

    #region EVENT HANDLERS
    public void HandlePlayerPlant()
    {
        if (!farmTileStatusController.CurrentViewedCell.HasValue)
        {
            return;
        }
        Vector3Int targetTile = farmTileStatusController.CurrentViewedCell.Value;
        farmMechanics.FarmManager.SpawnNewPlant(targetTile);
    }
    private void OnSeedPlantHandler(Vector3Int targetTile)
    {
        GameObject plant = Instantiate(
            plantPrefab,
            farmMechanics.farmTileUtility.GetTileWorldPosition(targetTile, 0f, 0f),
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

        GameObject stagePrefab =
            farmMechanics.PlantGrowthManager.GetPlantPrefabAtTile(targetTile);

        if (stagePrefab == null)
            return;

        SpriteRenderer currentRenderer =
            plant.GetComponent<SpriteRenderer>();

        SpriteRenderer stageRenderer =
            stagePrefab.GetComponent<SpriteRenderer>();

        if (currentRenderer == null || stageRenderer == null)
            return;

        currentRenderer.sprite = stageRenderer.sprite;
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