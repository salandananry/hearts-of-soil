using UnityEngine;

public class WaterController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private PlayerController playerController;

    [Header("Tile Sprites:")]
    [SerializeField] private IsometricRuleTile drySoil;
    [SerializeField] private IsometricRuleTile wetSoil;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;
    #endregion

    #region INITIALIZE
    public void Initialize(
        FarmMechanics farmMechanics,
        PlayerController playerController)
    {
        this.farmMechanics = farmMechanics;
        this.playerController = playerController;
        SubscribeEvents();
        SpawnExistingWateredTiles();
    }
    #endregion

    #region LOAD EXISTING STATE
    private void SpawnExistingWateredTiles()
    {
        farmMechanics.FarmManager.RestoreWateredSoil();
    }
    #endregion

    #region EVENT HANDLERS
    private void HandlePlayerWater(Vector2 mousePos)
    {
        if (farmMechanics.FarmModeController.CurrentFarmMode != FarmMode.Water) return;

        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(mousePos);
        farmMechanics.FarmManager.WaterSoil(targetTile);
    }
    private void OnSoilWateredHandler(Vector3Int targetTile)
    {
        farmMechanics.TileMapController.SetTileSprite(targetTile, wetSoil);
    }
    private void OnPlantHarvestHandler(Vector3Int targetTile)
    {
        farmMechanics.TileMapController.SetTileSprite(targetTile, drySoil);
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        playerController.OnPlayerInteract += HandlePlayerWater;

        farmMechanics.FarmManager.OnSoilWatered += OnSoilWateredHandler;
        farmMechanics.FarmManager.OnHarvestPlant += OnPlantHarvestHandler;
    }
    private void OnDestroy()
    {
        playerController.OnPlayerInteract -= HandlePlayerWater;

        farmMechanics.FarmManager.OnSoilWatered -= OnSoilWateredHandler;
        farmMechanics.FarmManager.OnHarvestPlant -= OnPlantHarvestHandler;
    }
    #endregion
}