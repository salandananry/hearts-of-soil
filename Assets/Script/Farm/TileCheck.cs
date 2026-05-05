using UnityEngine;

public class TileCheck : MonoBehaviour
{
    [Header("Script reference:")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileUtility tileUtility;
    [SerializeField] private InteractionHint interactionHint;

    [Header("Status panel")]
    [SerializeField] private GameObject statusPanel;
    
    private Vector3Int previousTile;
    private bool hasPreviousTile;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ShowHint(collision);
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
    private void ShowHint(Collider2D collision)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(collision.transform.position);

        if (!tileUtility.HasTile(targetTile)) return;

        if (hasPreviousTile && targetTile == previousTile) return;

        previousTile = targetTile;
        hasPreviousTile = true;

        interactionHint.ShowHint(tileUtility.GetTileWorldPosition(targetTile, 0f, 1.1f), "Press (e) to interact");
        CloseTileStatus();
    }
    public void ShowTileStatus(Vector3 position)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(position);
        
        if (!tileUtility.HasTile(targetTile)) return;

        statusPanel.SetActive(true);
        statusPanel.transform.position = tileUtility.GetTileWorldPosition(targetTile, 2f, 1.1f);
        interactionHint.HideHint();
    }
    public void CloseTileStatus()
    {
        statusPanel.SetActive(false);
    }
    /*
    private void ShowTileStatus(Collider2D collision)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(collision.transform.position);
        
        if (!tileUtility.HasTile(targetTile)) return;

        if (hasLastCell && targetTile == lastCell) return;

        lastCell = targetTile;
        hasLastCell = true;

        if (currentStatusText != null) HideTileStatus();

        currentStatusText = Instantiate(statusTextPrefab, spawnHolder);

        currentStatusText.rectTransform.position = tileUtility.GetSpawnPosition(targetTile, 1f);

        currentStatusText.text = GetSoilOccupancyText(targetTile) + " and " + GetSoilMoistureText(targetTile);
    }
    private void HideTileStatus()
    {
        if (currentStatusText != null)
        {
            Destroy(currentStatusText.gameObject);
            currentStatusText = null;
            hasLastCell = false;
        }
    }
    */
    private void HideHint()
    {
        interactionHint.HideHint();
        hasPreviousTile = false;
    }
    private string GetSoilOccupancyText(Vector3Int targetTile)
    {
        return HasPlant(targetTile)
            ? "This soil is occupied"
            : "This soil is empty";
    }
    private string GetSoilMoistureText(Vector3Int targetTile)
    {
        return IsWatered(targetTile)
            ? "watered"
            : "dry";
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