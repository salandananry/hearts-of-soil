using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileStatusController : MonoBehaviour
{
    #region FIELDS
    [Header("Script reference:")]
    private FarmMechanics farmMechanics;
    private PlayerController playerController;


    [Header("Status panel")]
    [SerializeField] private UIPanelTransition statusPanel;
    [SerializeField] private Image soilImagePreview;
    [SerializeField] private Image plantImagePreview;
    [SerializeField] private TextMeshProUGUI growthStageCount;

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
    private void HandlePlayerInspect(Vector2 mousePos)
    {
        if (farmMechanics.FarmModeController.CurrentFarmMode != FarmMode.Inspect) return;

        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(mousePos);

        if (!farmMechanics.TileUtility.HasTile(targetTile)) return;

        currentViewedTile = targetTile;

        RefreshTileStatus(targetTile);
        statusPanel.Open();
    }
    private void RefreshTileStatus(Vector3Int targetTile)
    {
        if (targetTile != currentViewedTile) return;

        RefreshSoilPreview(targetTile);
        RefreshPlantPreview(targetTile);
        RefreshGrowthStageCount(targetTile);
    }
    private void RefreshSoilPreview(Vector3Int targetTile)
    {
        soilImagePreview.sprite =
            farmMechanics.FarmManager.IsWatered(targetTile)
            ? wetTile
            : dryTile;
    }
    private void RefreshPlantPreview(Vector3Int targetTile)
    {
        bool hasPlant = farmMechanics.FarmManager.HasPlant(targetTile);

        plantImagePreview.gameObject.SetActive(hasPlant);

        if (!hasPlant) return;

        Sprite newSprite = farmMechanics.PlantGrowthManager.GetPlantSpriteAtTile(targetTile);
        plantImagePreview.sprite = newSprite;
    }
    private void RefreshGrowthStageCount(Vector3Int targetTile)
    {
        int growthStage = farmMechanics.PlantGrowthManager.GetGrowthStage(targetTile);
        switch (growthStage)
        {
            case 0:
                growthStageCount.text = "Planted (Not Ready)";
                break;
            case 1:
                growthStageCount.text = "Seedling (Not Ready)";
                break;
            case 2:
                growthStageCount.text = "Growing (Not Ready)";
                break;
            case 3:
                growthStageCount.text = "Mature (Ready to Harvest)";
                break;

            default:
                growthStageCount.text = "Nothing Planted";
                break;
        }
    }
    private void OnFarmModeChangeHandler(FarmMode farmMode)
    {
        if(farmMode != FarmMode.Inspect && statusPanel.gameObject.activeInHierarchy)
        {
            statusPanel.Close();
        }
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        playerController.OnPlayerInteract += HandlePlayerInspect;

        farmMechanics.FarmManager.OnPlantCreated += RefreshTileStatus;
        farmMechanics.FarmManager.OnSoilWatered += RefreshTileStatus;
        farmMechanics.FarmManager.OnHarvestPlant += RefreshTileStatus;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += RefreshTileStatus;
        farmMechanics.FarmModeController.OnFarmModeChange += OnFarmModeChangeHandler;
    }
    private void OnDestroy()
    {
        playerController.OnPlayerInteract -= HandlePlayerInspect;

        farmMechanics.FarmManager.OnPlantCreated -= RefreshTileStatus;
        farmMechanics.FarmManager.OnSoilWatered -= RefreshTileStatus;
        farmMechanics.FarmManager.OnHarvestPlant -= RefreshTileStatus;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= RefreshTileStatus;
        farmMechanics.FarmModeController.OnFarmModeChange -= OnFarmModeChangeHandler;
    }
    #endregion
}