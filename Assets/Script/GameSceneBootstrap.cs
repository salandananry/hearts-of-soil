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
}
public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Core Farm System")]
    [SerializeField] private FarmMechanics farmMechanics;

    [Header("Player References")]
    [SerializeField] private PlayerController playerController;

    [Header("UI References")]
    [SerializeField] private InteractionHint interactionHint;

    [Header("Gameplay Controllers")]
    [SerializeField] private FarmManager farmManager;
    [SerializeField] private TileStatusController tileStatus;
    [SerializeField] private PlantController plantController;
    [SerializeField] private WaterController waterController;
    [SerializeField] private PlantHarvestController plantHarvestController;

    private void Awake()
    {
        farmManager.Initialize(farmMechanics);
        tileStatus.Initialize(
            farmMechanics,
            playerController,
            interactionHint
        );

        plantController.Initalize(farmMechanics);
        waterController.Initialize(farmMechanics);
        plantHarvestController.Initialize(farmMechanics);
    }
}