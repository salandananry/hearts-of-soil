using UnityEngine;

public class HarvestController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private TileStatusController tileStatusController;
    #endregion

    #region INITIALIZE
    public void Initialize(
        FarmMechanics farmMechanics,
        TileStatusController tileStatusController)
    {
        this.farmMechanics = farmMechanics;
        this.tileStatusController = tileStatusController;
    }
    #endregion
    public void HarvestSelectedTile()
    {
        Vector3Int targetTile = tileStatusController.CurrentViewedTile;
        farmMechanics.FarmManager.HarvestPlant(targetTile);
    }
}