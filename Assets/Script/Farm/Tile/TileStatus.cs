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
    [SerializeField] private Button goPlantButton;
    [SerializeField] private Button goWaterButton;

    [Header("Soil variants:")]
    [SerializeField] private Sprite dryTile;
    [SerializeField] private Sprite wetTile;
    [Header("Plant variants")]
    [SerializeField] private Sprite[] plantVariant;
    
    private Vector3Int previousTile;
    private bool hasPreviousTile;
    private Vector3Int currentViewedTile;

    private void OnEnable()
    {
        tileWaterer.OnTileWatered += UpdateSoilPreview;
        seedPlanter.OnTilePlanted += UpdatePlantPreview;
        playerController.OnPlayerInteract += ShowTileStatusAt;
        plantGrowthManager.OnGrowthStageChanged += UpdatePlantPreview;

    }
    private void OnDisable()
    {
        tileWaterer.OnTileWatered -= UpdateSoilPreview;
        seedPlanter.OnTilePlanted -= UpdatePlantPreview;
        playerController.OnPlayerInteract -= ShowTileStatusAt;
        plantGrowthManager.OnGrowthStageChanged -= UpdatePlantPreview;
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
        UpdateSoilPreview(targetTile);
        UpdatePlantPreview(targetTile);
    }
    private void UpdateSoilPreview(Vector3Int targetTile)
    {
        if(tileManager.IsWatered(targetTile))
        {
            tileImage.sprite = wetTile;
            goWaterButton.interactable = false;
        }
        else
        {
            tileImage.sprite = dryTile;
            goWaterButton.interactable = true;
        }
    }
    private void UpdatePlantPreview(Vector3Int targetTile)
    {
        if (targetTile != currentViewedTile) return;

        if (tileManager.HasPlant(targetTile))
        {
            seedImage.gameObject.SetActive(true);
            seedImage.sprite = plantGrowthManager.GetPlantSprite(tileManager.GetGrowthStage(targetTile));
            goPlantButton.interactable = false;
        }
        else
        {
            seedImage.gameObject.SetActive(false);
            goPlantButton.interactable = true;
        }
    }
    #endregion
}