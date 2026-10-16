using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private TextMeshProUGUI tileCoordinate;

    [Header("Dialogue panel")]
    [SerializeField] private UIPanelTransition dialoguePanel;
    [SerializeField] private DialogueController dialogueController;

    [Header("Soil variants:")]
    [SerializeField] private Sprite dryTile;
    [SerializeField] private Sprite wetTile;

    private Vector3Int? currentViewedCell;
    public Vector3Int? CurrentViewedCell => currentViewedCell;
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
    farmMechanics.farmTileUtility.WorldToCell(playerController.transform.position);

        if (!farmMechanics.farmTileUtility.HasTile(playerCell))
        {
            if (currentViewedCell.HasValue)
            {
                currentViewedCell = null;
                statusPopUp.Close();
                dialoguePanel.Close();
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
            farmMechanics.farmTileUtility.GetTileWorldPosition(
                playerCell,
                0.40f,
                0.75f
            );

        statusPopUp.transform.position = targetPosition;
        statusPopUp.Open();
        statusPanel.Close();
        dialoguePanel.Close();
    }
    #region EVENT HANDLERS
    private void HandlePlayerInspect()
    {
        Vector3Int targetTile = farmMechanics.farmTileUtility.WorldToCell(playerController.transform.position);

        if (!farmMechanics.farmTileUtility.HasTile(targetTile)) return;
        RefreshTileStatus(targetTile);
        statusPanel.Open();
        dialoguePanel.Open();
        statusPopUp.Close();
    }
    private void RefreshTileStatus(Vector3Int targetTile)
    {
        if (targetTile != currentViewedCell) return;

        RefreshSoilPreview(targetTile);
        RefreshPlantPreview(targetTile);
        RefreshTileCoordinates(targetTile);
        RefreshText(targetTile);
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
    private void RefreshText(Vector3Int targetTile)
    {
        dialoguePanel.Open();
        if (!farmMechanics.FarmManager.HasPlant(targetTile))
        {
            dialogueController.ShowText("An empty plot... I should plant a seed here.");
            return;
        }

        if (!farmMechanics.FarmManager.IsWatered(targetTile))
        {
            dialogueController.ShowText("The soil is dry... I should water this plant.");
            return;
        }

        int growthStage = farmMechanics.PlantGrowthManager.GetGrowthStage(targetTile);

        switch (growthStage)
        {
            case 0:
                dialogueController.ShowText("A little seedling is starting to grow.\n (Stage: 1/4)");
                break;

            case 1:
                dialogueController.ShowText("It's growing nicely.\n (Stage: 2/4)");
                break;

            case 2:
                dialogueController.ShowText("It's getting bigger... almost there!\n (Stage: 3/4)");
                break;

            case 3:
                dialogueController.ShowText("It's fully grown! Time to harvest.\n (Stage: 4/4)");
                break;

            default:
                dialogueController.ShowText("I'm not sure what's growing here.");
                break;
        }
    }
    private void RefreshTileCoordinates(Vector3Int targetTile)
    {
        tileCoordinate.text = $"Tile: ({targetTile.x},{targetTile.y})";
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
    }
    private void OnDestroy()
    {
        playerController.OnPlayerInteract -= HandlePlayerInspect;

        farmMechanics.FarmManager.OnPlantCreated -= RefreshTileStatus;
        farmMechanics.FarmManager.OnSoilWatered -= RefreshTileStatus;
        farmMechanics.FarmManager.OnHarvestPlant -= RefreshTileStatus;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= RefreshTileStatus;
    }
    #endregion
}