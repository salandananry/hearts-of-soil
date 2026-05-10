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

    [Header("Status panel")]
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private Image tileImage;
    [SerializeField] private Image seedImage;
    [SerializeField] private Button goPlantButton;
    [SerializeField] private Button goWaterButton;

    [Header("Sprite variants")]
    [Header("Tiles:")]
    [SerializeField] private Sprite dryTile;
    [SerializeField] private Sprite wetTile;
    [Header("Seeds and Plants:")]
    [SerializeField] private Sprite seedPlantStage1;
    [SerializeField] private Sprite seedPlantStage2;
    [SerializeField] private Sprite seedPlantStage3;
    [SerializeField] private Sprite seedPlantStage4;
    [SerializeField] private Sprite seedPlantStage5;
    
    private Vector3Int previousTile;
    private bool hasPreviousTile;

    private void OnEnable()
    {
        tileWaterer.OnTileWatered += UpdateSoilPreview;
        seedPlanter.OnTilePlanted += UpdatePlantPreview;
        playerController.OnPlayerInteract += ShowTileStatusAt;
    }
    private void OnDisable()
    {
        tileWaterer.OnTileWatered -= UpdateSoilPreview;
        seedPlanter.OnTilePlanted -= UpdatePlantPreview;
        playerController.OnPlayerInteract -= ShowTileStatusAt;
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
            CloseTileStatus();
        }
    }
    private void ShowHint(Vector2 playerPosition)
    {
        Vector3Int targetCell = tileUtility.WorldToCell(playerPosition);

        if (!tileUtility.HasTile(targetCell)) return;
        if (hasPreviousTile && targetCell == previousTile) return;

        previousTile = targetCell;
        hasPreviousTile = true;

        interactionHint.ShowHint(tileUtility.GetTileWorldPosition(targetCell, 0f, 1.1f), "Press (e) to inspect");
        
        CloseTileStatus();
    }
    #endregion

    #region Status Panel
    private void ShowTileStatusAt(Vector2 playerPosition)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(playerPosition);

        if (!tileUtility.HasTile(targetTile)) return;

        interactionHint.HideHint();

        statusPanel.SetActive(true);
        UpdatePreviewImage(targetTile);
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
        if(tileManager.HasPlant(targetTile))
        {
            seedImage.gameObject.SetActive(true);
            seedImage.sprite = seedPlantStage1;
            goPlantButton.interactable = false;
        }
        else
        {
            seedImage.gameObject.SetActive(false);
            goPlantButton.interactable = true;
        }
    }
    private void CloseTileStatus()
    {
        statusPanel.SetActive(false);
    }
    #endregion
}