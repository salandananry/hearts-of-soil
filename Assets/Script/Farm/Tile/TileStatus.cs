using UnityEngine;
using UnityEngine.UI;

public class TileStatus : MonoBehaviour
{
    [Header("Script reference:")]
    private FarmMechanics farmMechanics;
    private InteractionHint interactionHint;
    private PlayerController playerController;


    [Header("Status panel")]
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private Image tileImage;
    [SerializeField] private Image seedImage;
    [Header("Action buttons")]
    [SerializeField] private Button plantButton;
    [SerializeField] private Button waterButton;
    [SerializeField] private Button harvestButton;

    [Header("Soil variants:")]
    [SerializeField] private Sprite dryTile;
    [SerializeField] private Sprite wetTile;
    
    private Vector3Int previousTile;
    private bool hasPreviousTile;
    private Vector3Int currentViewedTile;

    public void Initialize(
        FarmMechanics farmMechanics,
        PlayerController playerController,
        InteractionHint interactionHint)
    {
        this.farmMechanics = farmMechanics;
        this.playerController = playerController;
        this.interactionHint = interactionHint;

        SubscribeEvents();
    }

    #region Interaction Hint
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ShowHint(collision.transform.position);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactionHint.HideHint();
            hasPreviousTile = false;
            statusPanel.SetActive(false);
        }
    }
    private void ShowHint(Vector2 playerPosition)
    {
        Vector3Int targetCell = farmMechanics.TileUtility.WorldToCell(playerPosition);

        if (hasPreviousTile && targetCell == previousTile) return;
        if (!farmMechanics.TileUtility.HasTile(targetCell)) return;

        previousTile = targetCell;
        hasPreviousTile = true;

        interactionHint.ShowHint(farmMechanics.TileUtility.GetTileWorldPosition(targetCell, 0f, 1.1f), "Press (e) to inspect");

        statusPanel.SetActive(false);
    }
    #endregion

    #region STATUS PANEL
    private void ShowTileStatusAt(Vector2 playerPosition)
    {
        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(playerPosition);

        if (!farmMechanics.TileUtility.HasTile(targetTile)) return;

        currentViewedTile = targetTile;
        interactionHint.HideHint();

        UpdatePreviewImage(targetTile);
        statusPanel.SetActive(true);
    }
    private void UpdatePreviewImage(Vector3Int targetTile)
    {
        if (targetTile != currentViewedTile) return;

        UpdateSoilPreview(targetTile);
        UpdatePlantPreview(targetTile);
        UpdateActionButtons(targetTile);
    }
    private void UpdateSoilPreview(Vector3Int targetTile)
    {
        tileImage.sprite =
            farmMechanics.FarmManager.IsWatered(targetTile)
            ? wetTile
            : dryTile;
    }
    private void UpdatePlantPreview(Vector3Int targetTile)
    {
        if (targetTile != currentViewedTile) return;

        bool hasPlant = farmMechanics.FarmManager.HasPlant(targetTile);

        seedImage.gameObject.SetActive(hasPlant);

        if (!hasPlant) return;

        int growthStage = farmMechanics.PlantGrowthManager.GetGrowthStage(targetTile);
        Sprite sprite = farmMechanics.PlantGrowthManager.GetPlantSprite(growthStage);

        seedImage.sprite = sprite;
    }
    private void UpdateActionButtons(Vector3Int targetTile)
    {
        bool hasPlant = farmMechanics.FarmManager.HasPlant(targetTile);
        bool isWatered = farmMechanics.FarmManager.IsWatered(targetTile);
        bool isFullyGrown = farmMechanics.FarmManager.IsFullyGrown(targetTile);

        plantButton.interactable = !hasPlant;

        waterButton.interactable =
            hasPlant &&
            !isWatered;
        harvestButton.interactable = hasPlant && isFullyGrown;
    }
    #endregion

    #region EVENTS
    private void SubscribeEvents()
    {
        farmMechanics.FarmManager.OnPlantCreated += UpdatePreviewImage;
        farmMechanics.FarmManager.OnSoilWatered += UpdatePreviewImage;
        farmMechanics.FarmManager.OnHarvestPlant += UpdatePreviewImage;
        playerController.OnPlayerInteract += ShowTileStatusAt;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged += UpdatePreviewImage;
    }
    private void OnDestroy()
    {
        farmMechanics.FarmManager.OnPlantCreated -= UpdatePreviewImage;
        farmMechanics.FarmManager.OnSoilWatered -= UpdatePreviewImage;
        farmMechanics.FarmManager.OnHarvestPlant -= UpdatePreviewImage;
        playerController.OnPlayerInteract -= ShowTileStatusAt;
        farmMechanics.PlantGrowthManager.OnGrowthStageChanged -= UpdatePreviewImage;
    }
    #endregion
}