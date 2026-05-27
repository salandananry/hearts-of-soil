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
    private void PlantSeed(Vector2 mousePos)
    {
        if (farmMechanics.FarmModeController.CurrentFarmMode != FarmMode.Plant) return;

        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(mousePos);
        farmMechanics.FarmManager.SpawnNewPlant(targetTile);
    }
    private void OnSeedPlantHandler(Vector3Int targetTile)
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
        Sprite newSprite = farmMechanics.PlantGrowthManager.GetPlantSpriteAtTile(targetTile);
        if (newSprite == null) return;
        spriteRenderer.sprite = newSprite;
    }
    private void OnPlantHarvestHandler(Vector3Int targetTile)
    {
        RemovePlantAt(targetTile);
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
        playerController.OnPlayerInteract += PlantSeed;

        farmMechanics.FarmManager.OnPlantCreated += OnSeedPlantHandler;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += UpdatePlantSprite;
        farmMechanics.FarmManager.OnHarvestPlant += OnPlantHarvestHandler;

    }
    private void OnDestroy()
    {
        playerController.OnPlayerInteract -= PlantSeed;

        farmMechanics.FarmManager.OnPlantCreated -= OnSeedPlantHandler;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= UpdatePlantSprite;
        farmMechanics.FarmManager.OnHarvestPlant -= OnPlantHarvestHandler;
    }
    #endregion
}