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
    [SerializeField] private UIPanelTransition statusPopUp;
    [SerializeField] private UIPanelTransition statusPanel;
    [SerializeField] private Image soilImagePreview;
    [SerializeField] private Image plantImagePreview;
    [SerializeField] private TextMeshProUGUI growthStageCount;
    [SerializeField] private TextMeshProUGUI growthStatus;
    [SerializeField] private TextMeshProUGUI tileCoordinate;

    [Header("Soil variants:")]
    [SerializeField] private Sprite dryTile;
    [SerializeField] private Sprite wetTile;

    private Vector3Int? currentViewedCell;
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
    private void Update()
    {
        Vector3Int playerCell =
    farmMechanics.SoilTileUtility.WorldToCell(playerController.transform.position);

        if (!farmMechanics.SoilTileUtility.HasTile(playerCell))
        {
            if (currentViewedCell.HasValue)
            {
                currentViewedCell = null;
                statusPopUp.Close();
                statusPanel.Close();
            }

            return;
        }

        if (currentViewedCell.HasValue &&
            playerCell == currentViewedCell.Value)
        {
            return;
        }

        currentViewedCell = playerCell;

        Vector3 targetPosition =
            farmMechanics.SoilTileUtility.GetTileWorldPosition(
                playerCell,
                0.40f,
                0.75f
            );

        statusPopUp.transform.position = targetPosition;
        statusPopUp.Open();
        statusPanel.Close();
    }
    #region EVENT HANDLERS
    private void HandlePlayerInspect(Vector2 mousePos)
    {
        //if (farmMechanics.FarmModeController.CurrentFarmMode != FarmMode.Inspect) return;

        Vector3Int targetTile = farmMechanics.SoilTileUtility.WorldToCell(playerController.transform.position);

        if (!farmMechanics.SoilTileUtility.HasTile(targetTile)) return;

        //currentViewedCell = targetTile;

        RefreshTileStatus(targetTile);
        statusPanel.Open();
        statusPopUp.Close();
    }
    private void RefreshTileStatus(Vector3Int targetTile)
    {
        if (targetTile != currentViewedCell) return;

        RefreshSoilPreview(targetTile);
        RefreshPlantPreview(targetTile);
        RefreshGrowthStageCount(targetTile);
        RefreshTileCoordinates(targetTile);
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
        if (!farmMechanics.FarmManager.HasPlant(targetTile))
        {
            growthStageCount.text = "Stage: 0/4";
            growthStatus.text = "Status: No Plant";
            return;
        }
        if (!farmMechanics.FarmManager.IsWatered(targetTile))
        {
            growthStageCount.text = "Stage: 0/4";
            growthStatus.text = "Status: Not Growing (Needs Water)";
            return;
        }

        int growthStage = farmMechanics.PlantGrowthManager.GetGrowthStage(targetTile);

        switch (growthStage)
        {
            case 0:
                growthStageCount.text = "Stage: 1/4";
                growthStatus.text = "Status: Growing";
                break;
            case 1:
                growthStageCount.text = "Stage: 2/4";
                growthStatus.text = "Status: Growing";
                break;
            case 2:
                growthStageCount.text = "Stage: 3/4";
                growthStatus.text = "Status: Growing";
                break;
            case 3:
                growthStageCount.text = "Stage: 4/4";
                growthStatus.text = "Status: Mature";
                break;
        }
    }
    private void RefreshTileCoordinates(Vector3Int targetTile)
    {
        tileCoordinate.text = $"Tile: ({targetTile.x},{targetTile.y})";
    }
    private void OnFarmModeChangeHandler(FarmMode farmMode)
    {
        if(farmMode != FarmMode.Inspect && statusPanel.gameObject.activeInHierarchy)
        {
            //statusPanel.Close();
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