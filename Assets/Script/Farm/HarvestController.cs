using UnityEngine;

public class HarvestController : MonoBehaviour
{
    #region FIELDS
    private FarmMechanics farmMechanics;
    private PlayerController playerController;
    #endregion

    #region INITIALIZE
    public void Initialize(
        FarmMechanics farmMechanics,
        PlayerController playerController)
    {
        this.farmMechanics = farmMechanics;
        this.playerController = playerController;
        SubscribeEvents();
    }
    #endregion

    #region EVENT HANDLERS
    private void HandlePlayerHarvest(Vector2 mousePos)
    {
        if (farmMechanics.FarmModeController.CurrentFarmMode != FarmMode.Harvest) return;

        Vector3Int targetTile = farmMechanics.SoilTileUtility.WorldToCell(mousePos);
        farmMechanics.FarmManager.HarvestPlant(targetTile);
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        playerController.OnPlayerInteract += HandlePlayerHarvest;
    }
    private void OnDestroy()
    {
        playerController.OnPlayerInteract -= HandlePlayerHarvest;
    }
    #endregion
}