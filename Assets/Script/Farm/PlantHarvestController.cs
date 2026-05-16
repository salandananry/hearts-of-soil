using UnityEngine;
using UnityEngine.UI;

public class PlantHarvestController : MonoBehaviour
{
    [SerializeField] private GameObject playerPosition;
    [SerializeField] private Button harvestButton;

    private FarmMechanics farmMechanics;

    public void Initialize(FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
        InitialButtonSetUp();
        SubscribeEvents();
    }
    #region SET BUTTON INTERACTABLE
    private void InitialButtonSetUp()
    {
        var pos = playerPosition.transform.position;
        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(pos);
        SetButtonInteractable(targetTile);
    }
    private void SetButtonInteractable(Vector3Int targetTile)
    {
        bool isFullyGrown = farmMechanics.FarmManager.IsFullyGrown(targetTile);

        harvestButton.interactable = isFullyGrown;
        harvestButton.onClick.RemoveAllListeners();

        if (isFullyGrown)
        {
            harvestButton.onClick.AddListener(() => HarvestPlant(targetTile));
        }
    }
    #endregion
    private void HarvestPlant(Vector3Int targetTile)
    {
        farmMechanics.FarmManager.HarvestPlant(targetTile);
    }
    #region SUBSCRIBE AND UNSUBSCRIBE EVENT
    private void SubscribeEvents()
    {
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += SetButtonInteractable;
    }
    private void OnDestroy()
    {
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= SetButtonInteractable;
    }
    #endregion
}