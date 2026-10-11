using UnityEngine;

public class HarvestController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private TileStatusController farmTileStatusController;
    #endregion

    #region INITIALIZE
    public void Initialize(
        FarmMechanics farmMechanics,
        TileStatusController farmTileStatusController)
    {
        this.farmMechanics = farmMechanics;
        this.farmTileStatusController = farmTileStatusController;
    }
    #endregion

    #region EVENT HANDLERS
    public void HandlePlayerHarvest()
    {
        if (!farmTileStatusController.CurrentViewedCell.HasValue)
        {
            return;
        }
        Vector3Int targetTile = farmTileStatusController.CurrentViewedCell.Value;
        farmMechanics.FarmManager.HarvestPlant(targetTile);
    }
    #endregion
}