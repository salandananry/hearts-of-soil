using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private PlayerController playerController;

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
        PlayerController playerController)
    {
        this.farmMechanics = farmMechanics;
        this.playerController = playerController;
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
    private void HandlePlayerPlant(Vector2 mousePos)
    {
        if (farmMechanics.FarmModeController.CurrentFarmMode != FarmMode.Plant) return;

        Vector3Int targetTile = farmMechanics.SoilTileUtility.WorldToCell(mousePos);
        farmMechanics.FarmManager.SpawnNewPlant(targetTile);
    }
    private void OnSeedPlantHandler(Vector3Int targetTile)
    {
        GameObject plant = Instantiate(
            plantPrefab,
            farmMechanics.SoilTileUtility.GetTileWorldPosition(targetTile, 0f, 0f),
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
            farmMechanics.SoilTileUtility.GetTileWorldPosition(
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
        playerController.OnPlayerInteract += HandlePlayerPlant;

        farmMechanics.FarmManager.OnPlantCreated += OnSeedPlantHandler;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += UpdatePlantVisual;
        farmMechanics.FarmManager.OnHarvestPlant += RemovePlantAt;

    }
    private void OnDestroy()
    {
        playerController.OnPlayerInteract -= HandlePlayerPlant;

        farmMechanics.FarmManager.OnPlantCreated -= OnSeedPlantHandler;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= UpdatePlantVisual;
        farmMechanics.FarmManager.OnHarvestPlant -= RemovePlantAt;
    }
    #endregion
}