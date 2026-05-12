using UnityEngine;
using UnityEngine.UI;

public class TileStatus : MonoBehaviour
{
    [Header("Script reference:")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileUtility tileUtility;
    [SerializeField] private InteractionHint interactionHint;
    [SerializeField] private TileWaterer tileWaterer;
    [SerializeField] private SeedPlanter seedPlanter;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlantGrowthManager plantGrowthManager;

    [Header("Status panel")]
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private Image tileImage;
    [SerializeField] private Image seedImage;
    [Header("Action buttons")]
    [SerializeField] private Button plantButton;
    [SerializeField] private Button waterButton;

    [Header("Soil variants:")]
    [SerializeField] private Sprite dryTile;
    [SerializeField] private Sprite wetTile;
    
    private Vector3Int previousTile;
    private bool hasPreviousTile;
    private Vector3Int currentViewedTile;

    private void OnEnable()
    {
        tileWaterer.OnTileWatered += UpdatePreviewImage;
        seedPlanter.OnTilePlanted += UpdatePreviewImage;
        playerController.OnPlayerInteract += ShowTileStatusAt;
        plantGrowthManager.OnGrowthStageChanged += UpdatePreviewImage;

    }
    private void OnDisable()
    {
        tileWaterer.OnTileWatered -= UpdatePreviewImage;
        seedPlanter.OnTilePlanted -= UpdatePreviewImage;
        playerController.OnPlayerInteract -= ShowTileStatusAt;
        plantGrowthManager.OnGrowthStageChanged -= UpdatePreviewImage;
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
        Vector3Int targetCell = tileUtility.WorldToCell(playerPosition);

        if (hasPreviousTile && targetCell == previousTile) return;
        if (!tileUtility.HasTile(targetCell)) return;

        previousTile = targetCell;
        hasPreviousTile = true;

        interactionHint.ShowHint(tileUtility.GetTileWorldPosition(targetCell, 0f, 1.1f), "Press (e) to inspect");

        statusPanel.SetActive(false);
    }
    #endregion

    #region STATUS PANEL
    private void ShowTileStatusAt(Vector2 playerPosition)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(playerPosition);

        if (!tileUtility.HasTile(targetTile)) return;

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
            tileManager.IsWatered(targetTile)
            ? wetTile
            : dryTile;
    }
    private void UpdatePlantPreview(Vector3Int targetTile)
    {
        if (targetTile != currentViewedTile) return;

        bool hasPlant = tileManager.HasPlant(targetTile);

        seedImage.gameObject.SetActive(hasPlant);

        if (!hasPlant) return;

        seedImage.sprite =
            plantGrowthManager.GetPlantSprite(
                tileManager.GetGrowthStage(targetTile)
            );
    }
    private void UpdateActionButtons(Vector3Int targetTile)
    {
        bool hasPlant = tileManager.HasPlant(targetTile);
        bool isWatered = tileManager.IsWatered(targetTile);

        plantButton.interactable = !hasPlant;

        waterButton.interactable =
            hasPlant &&
            !isWatered;
    }
    #endregion
}