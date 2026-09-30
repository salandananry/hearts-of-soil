using System;
using UnityEngine;

[System.Serializable]
public class FarmMechanics
{
    public FarmManager FarmManager;
    public TileUtility TileUtility;
    public PlantGrowthManager PlantGrowthManager;
    public WaterController waterController;
    public PlantController plantController;
    public TileMapController TileMapController;
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
    [SerializeField] private HarvestController harvestController;
    [SerializeField] private FarmModeController farmModeController;

    private void Awake()
    {
        farmManager.Initialize(farmMechanics);
        tileStatus.Initialize(farmMechanics, playerController);
        plantController.Initalize(farmMechanics, tileStatus);
        waterController.Initialize(farmMechanics, tileStatus);
        harvestController.Initialize(farmMechanics, tileStatus);
        farmModeController.Initialize(plantController,waterController, harvestController);
    }
}