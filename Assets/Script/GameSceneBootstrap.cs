using UnityEngine;

[System.Serializable]
public class FarmMechanics
{
    public FarmManager FarmManager;
    public TileUtility TileUtility;
    public PlantGrowthManager PlantGrowthManager;
    public WaterController TileWaterer;
    public PlantController SeedPlanter;
    public TileMapController TileMapController;
}
public class GameSceneBootstrap : MonoBehaviour
{
    [Header("Farm")]
    [SerializeField] private FarmMechanics farmMechanics;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("UI")]
    [SerializeField] private InteractionHint interactionHint;

    [SerializeField] private FarmManager farmManager;
    [SerializeField] private TileStatus tileStatus;
    [SerializeField] private PlantController seedPlanter;
    [SerializeField] private WaterController tileWaterer;
    [SerializeField] private PlantHarvestController plantHarvestController;

    private void Awake()
    {
        farmManager.Initialize(farmMechanics);
        tileStatus.Initialize(
            farmMechanics,
            playerController,
            interactionHint
        );

        seedPlanter.Initalize(farmMechanics);
        tileWaterer.Initialize(farmMechanics);
        plantHarvestController.Initialize(farmMechanics);
    }
}