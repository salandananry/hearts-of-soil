using UnityEngine;
using UnityEngine.UI;

public class TileStatusController : MonoBehaviour
{
    #region FIELDS
    [Header("Script reference:")]
    private FarmMechanics farmMechanics;
    private PlayerController playerController;


    [Header("Status panel")]
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private Image soilImagePreview;
    [SerializeField] private Image plantImagePreview;

    [Header("Soil variants:")]
    [SerializeField] private Sprite dryTile;
    [SerializeField] private Sprite wetTile;

    private Vector3Int currentViewedTile;
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
    private void ShowTileStatusAt(Vector2 mousePos)
    {
        statusPanel.SetActive(false);
        if (farmMechanics.FarmModeController.CurrentFarmMode != FarmMode.Inspect) return;

        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(mousePos);

        if (!farmMechanics.TileUtility.HasTile(targetTile)) return;

        currentViewedTile = targetTile;

        UpdateTileStatusUI(targetTile);
        statusPanel.SetActive(true);
    }
    private void UpdateTileStatusUI(Vector3Int targetTile)
    {
        if (targetTile != currentViewedTile) return;

        UpdateSoilPreview(targetTile);
        UpdatePlantPreview(targetTile);
    }
    private void UpdateSoilPreview(Vector3Int targetTile)
    {
        soilImagePreview.sprite =
            farmMechanics.FarmManager.IsWatered(targetTile)
            ? wetTile
            : dryTile;
    }
    private void UpdatePlantPreview(Vector3Int targetTile)
    {
        if (targetTile != currentViewedTile) return;

        bool hasPlant = farmMechanics.FarmManager.HasPlant(targetTile);

        plantImagePreview.gameObject.SetActive(hasPlant);

        if (!hasPlant) return;

        Sprite newSprite = farmMechanics.PlantGrowthManager.GetPlantSpriteAtTile(targetTile);
        plantImagePreview.sprite = newSprite;
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        playerController.OnPlayerInteract += ShowTileStatusAt;

        farmMechanics.FarmManager.OnPlantCreated += UpdateTileStatusUI;
        farmMechanics.FarmManager.OnSoilWatered += UpdateTileStatusUI;
        farmMechanics.FarmManager.OnHarvestPlant += UpdateTileStatusUI;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += UpdateTileStatusUI;
    }
    private void OnDestroy()
    {
        playerController.OnPlayerInteract -= ShowTileStatusAt;

        farmMechanics.FarmManager.OnPlantCreated -= UpdateTileStatusUI;
        farmMechanics.FarmManager.OnSoilWatered -= UpdateTileStatusUI;
        farmMechanics.FarmManager.OnHarvestPlant -= UpdateTileStatusUI;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= UpdateTileStatusUI;
    }
    #endregion
}