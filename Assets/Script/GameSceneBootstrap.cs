using UnityEngine;

[System.Serializable]
public class FarmMechanics
{
    public FarmManager FarmManager;
    public TileUtility farmTileUtility;
    public PlantGrowthManager PlantGrowthManager;
    public WaterController waterController;
    public PlantController plantController;
    public TileMapController farmTileStatusController;
}
public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Core Farm System")]
    [SerializeField] private FarmMechanics farmMechanics;

    [Header("Player References")]
    [SerializeField] private PlayerController playerController;

    [Header("Gameplay Controllers")]
    [SerializeField] private FarmManager farmManager;
    [SerializeField] private TileStatusController FarmTileStatusController;
    [SerializeField] private PlantController plantController;
    [SerializeField] private WaterController waterController;
    [SerializeField] private HarvestController HarvestController;
    private void Start()
    {
        farmManager.Initialize(farmMechanics);
        FarmTileStatusController.Initialize(farmMechanics, playerController);
        plantController.Initalize(farmMechanics, FarmTileStatusController);
        waterController.Initialize(farmMechanics, FarmTileStatusController);
        HarvestController.Initialize(farmMechanics, FarmTileStatusController);
    }
}