using UnityEngine;

[System.Serializable]
public class FarmMechanics
{
    public FarmManager FarmManager;
    public TileUtility SoilTileUtility;
    public PlantGrowthManager PlantGrowthManager;
    public WaterController waterController;
    public PlantController plantController;
    public TileMapController SoilTileMapController;
    public FarmModeController FarmModeController;
}
public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Core Farm System")]
    [SerializeField] private FarmMechanics farmMechanics;

    [Header("Player References")]
    [SerializeField] private PlayerController playerController;

    [Header("Gameplay Controllers")]
    [SerializeField] private FarmManager farmManager;
    [SerializeField] private TileStatusController tileStatus;
    [SerializeField] private PlantController plantController;
    [SerializeField] private WaterController waterController;
    [SerializeField] private HarvestController plantHarvestController;
    [SerializeField] private CursorController cursorController;

    private void Awake()
    {
        farmManager.Initialize(farmMechanics);
        tileStatus.Initialize(farmMechanics, playerController);
        plantController.Initalize(farmMechanics, playerController);
        waterController.Initialize(farmMechanics, playerController);
        plantHarvestController.Initialize(farmMechanics, playerController);
    }
}