using UnityEngine;

[System.Serializable]
public class FarmMechanics
{
    public TileManager TileManager;
    public TileUtility TileUtility;
    public PlantGrowthManager PlantGrowthManager;
    public TileWaterer TileWaterer;
    public SeedPlanter SeedPlanter;
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

    [SerializeField] private TileStatus tileStatus;
    [SerializeField] private SeedPlanter seedPlanter;
    [SerializeField] private TileWaterer tileWaterer;

    private void Awake()
    {
        tileStatus.Initialize(
            farmMechanics,
            playerController,
            interactionHint
        );

        seedPlanter.Initalize(farmMechanics);
        tileWaterer.Initialize(farmMechanics);
    }
}