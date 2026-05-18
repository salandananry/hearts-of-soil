using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;

    [Header("Plant prefab:")]
    [SerializeField] private GameObject plantPrefab;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;

    private Dictionary<Vector3Int, GameObject> spawnedPlants =
    new Dictionary<Vector3Int, GameObject>();
    #endregion

    #region INITIALIZE
    public void Initalize(
        FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
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

    #region PLAYER ACTION
    public void PlantSeed()
    {
        Vector3 pos = playerObject.transform.position;
        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(pos);
        farmMechanics.FarmManager.SpawnNewPlant(targetTile);
    }
    #endregion

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
        UpdatePlantSprite(targetTile);
    }
    private void UpdatePlantSprite(Vector3Int targetTile)
    {
        if (!spawnedPlants.TryGetValue(targetTile, out GameObject plant)) return;

        SpriteRenderer spriteRenderer = plant.GetComponent<SpriteRenderer>();
        var newSprite = farmMechanics.PlantGrowthManager.GetPlantSprite(targetTile);
        if (newSprite == null) return;
        spriteRenderer.sprite = newSprite;
    }
    #endregion
    public GameObject GetPlant(Vector3Int targetTile)
    {
        spawnedPlants.TryGetValue(targetTile, out GameObject plant);
        return plant;
    }
    public void RemovePlant(Vector3Int targetTile)
    {
        if (!spawnedPlants.TryGetValue(targetTile, out GameObject plant))
            return;

        Destroy(plant);
        spawnedPlants.Remove(targetTile);
    }
    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        farmMechanics.FarmManager.OnPlantCreated += OnSeedPlantHandler;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += UpdatePlantSprite;

    }
    private void OnDestroy()
    {
        if (farmMechanics == null) return;

        var FM = farmMechanics.FarmManager;
        if (FM != null) FM.OnPlantCreated -= OnSeedPlantHandler;
        var PGM = farmMechanics.PlantGrowthManager;
        if (PGM != null) PGM.OnGrowthStageChanged -= UpdatePlantSprite;
    }
    #endregion
}