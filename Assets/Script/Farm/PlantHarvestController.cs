using UnityEngine;

public class PlantHarvestController : MonoBehaviour
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
    private void HarvestPlant(Vector2 mousePos)
    {
        if (farmMechanics.FarmModeController.CurrentFarmMode != FarmMode.Harvest) return;

        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(mousePos);
        farmMechanics.FarmManager.HarvestPlant(targetTile);
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        playerController.OnPlayerInteract += HarvestPlant;
    }
    private void OnDestroy()
    {
        playerController.OnPlayerInteract -= HarvestPlant;
    }
    #endregion
}