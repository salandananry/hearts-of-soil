using UnityEngine;

public class WaterController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private TileStatusController farmTileStatusController;

    [Header("Tile Sprites:")]
    [SerializeField] private IsometricRuleTile drySoil;
    [SerializeField] private IsometricRuleTile wetSoil;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;
    #endregion

    #region INITIALIZE
    public void Initialize(
        FarmMechanics farmMechanics,
        TileStatusController farmTileStatusController)
    {
        this.farmMechanics = farmMechanics;
        this.farmTileStatusController = farmTileStatusController;
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
    public void HandlePlayerWater()
    {
        if (!farmTileStatusController.CurrentViewedCell.HasValue)
        {
            return;
        }
        Vector3Int targetTile = farmTileStatusController.CurrentViewedCell.Value;
        farmMechanics.FarmManager.WaterSoil(targetTile);
    }
    private void OnSoilWateredHandler(Vector3Int targetTile)
    {
        farmMechanics.farmTileStatusController.SetTile(targetTile, wetSoil);
    }
    private void OnPlantHarvestHandler(Vector3Int targetTile)
    {
        farmMechanics.farmTileStatusController.SetTile(targetTile, drySoil);
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        farmMechanics.FarmManager.OnSoilWatered += OnSoilWateredHandler;
        farmMechanics.FarmManager.OnHarvestPlant += OnPlantHarvestHandler;
    }
    private void OnDestroy()
    {
        farmMechanics.FarmManager.OnSoilWatered -= OnSoilWateredHandler;
        farmMechanics.FarmManager.OnHarvestPlant -= OnPlantHarvestHandler;
    }
    #endregion
}