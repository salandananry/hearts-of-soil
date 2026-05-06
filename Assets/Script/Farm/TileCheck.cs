using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileCheck : MonoBehaviour
{
    [Header("Script reference:")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileUtility tileUtility;
    [SerializeField] private InteractionHint interactionHint;

    [Header("Status panel")]
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Image tileImage;
    [SerializeField] private Image seedImage;

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
            HideHint();
            CloseTileStatus();
        }
    }
    private void ShowHint(Vector2 playerPosition)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(playerPosition);

        if (!tileUtility.HasTile(targetTile)) return;

        if (hasPreviousTile && targetTile == previousTile) return;

        previousTile = targetTile;
        hasPreviousTile = true;

        interactionHint.ShowHint(tileUtility.GetTileWorldPosition(targetTile, 0f, 1.1f), "Press (e) to inspect");
        CloseTileStatus();
    }
    public void ShowTileStatusAt(Vector2 playerPosition)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(playerPosition);
        
        if (!tileUtility.HasTile(targetTile)) return;

        statusPanel.SetActive(true);
        //statusPanel.transform.position = tileUtility.GetTileWorldPosition(targetTile, 2f, 1.1f);
        statusText.text = GetSoilOccupancyText(targetTile) + " + " + GetSoilMoistureText(targetTile);
        UpdatePreviewImage(targetTile);
        interactionHint.HideHint();
    }
    private void UpdatePreviewImage(Vector3Int targetTile)
    {
        if(IsWatered(targetTile))
        {
            tileImage.sprite = wetTile;
        }
        else
        {
            tileImage.sprite = dryTile;
        }
        seedImage.gameObject.SetActive(true);
        if(HasPlant(targetTile))
        {
            seedImage.sprite = seedPlantStage1;
        }
        else
        {
            seedImage.gameObject.SetActive(false);
        }
    }
    public void CloseTileStatus()
    {
        statusPanel.SetActive(false);
    }
    private void HideHint()
    {
        interactionHint.HideHint();
        hasPreviousTile = false;
    }
    private string GetSoilOccupancyText(Vector3Int targetTile)
    {
        return HasPlant(targetTile)
            ? "Planted"
            : "Empty";
    }
    private string GetSoilMoistureText(Vector3Int targetTile)
    {
        return IsWatered(targetTile)
            ? "Watered"
            : "Dry";
    }
    public bool HasPlant(Vector3Int targetTile)
    {
        return tileManager.HasPlant(targetTile);
    }
    public bool IsWatered(Vector3Int targetTile)
    {
        return tileManager.IsWatered(targetTile);
    }
}