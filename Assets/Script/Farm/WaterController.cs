using UnityEngine;

public class WaterController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private TileStatusController tileStatusController;

    [Header("Tile Sprites:")]
    [SerializeField] private IsometricRuleTile drySoil;
    [SerializeField] private IsometricRuleTile wetSoil;
    #endregion

    #region INITIALIZE
    public void Initialize(
        FarmMechanics farmMechanics,
        TileStatusController tileStatusController)
    {
        this.farmMechanics = farmMechanics;
        this.tileStatusController = tileStatusController;
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
    public void WaterSelectedTile()
    {
        Vector3Int targetTile = tileStatusController.CurrentViewedTile;
        farmMechanics.FarmManager.WaterSoil(targetTile);
    }
    #region EVENT HANDLERS
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