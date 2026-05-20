using UnityEngine;

public class WaterController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;

    [Header("Tile Sprites:")]
    [SerializeField] private IsometricRuleTile drySoil;
    [SerializeField] private IsometricRuleTile wetSoil;

    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;
    #endregion

    #region INITIALIZE
    public void Initialize(FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
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

    #region PLAYER ACTION
    public void WaterSoil()
    {
        Vector3 pos = playerObject.transform.position;
        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(pos);
        farmMechanics.FarmManager.WaterSoil(targetTile);
    }
    #endregion

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