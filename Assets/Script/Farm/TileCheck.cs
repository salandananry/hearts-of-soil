using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCheck : MonoBehaviour
{
    [SerializeField] private Tilemap soilTilemap;
    [SerializeField] private TileManager tileManager;

    [Header("Tile Status UI: ")]
    [SerializeField] private TextMeshProUGUI statusTextPrefab;
    [SerializeField] private Transform spawnHolder;
    private TextMeshProUGUI currentStatusText;
    
    private Vector3Int lastCell;
    private bool hasLastCell;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ShowTileStatus(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HideTileStatus();
        }
    }
    private void ShowTileStatus(Collider2D collision)
    {
        Vector3Int targetTile = GetTileAtPosition(collision.transform.position);
        
        if (!HasSoilTile(targetTile)) return;

        if (hasLastCell && targetTile == lastCell) return;

        lastCell = targetTile;
        hasLastCell = true;

        if (currentStatusText != null) HideTileStatus();

        currentStatusText = Instantiate(statusTextPrefab, spawnHolder);

        currentStatusText.rectTransform.position = GetSpawnPosition(targetTile);

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
    public bool HasSoilTile(Vector3Int targetTile)
    {
        return soilTilemap.HasTile(targetTile);
    }
    public bool HasPlant(Vector3Int targetTile)
    {
        return tileManager.HasPlant(targetTile);
    }
    public bool IsWatered(Vector3Int targetTile)
    {
        return tileManager.IsWatered(targetTile);
    }
    public Vector3 GetSpawnPosition(Vector3Int targetTile)
    {
        Vector3 worldPos = soilTilemap.GetCellCenterWorld(targetTile);
        worldPos.y += 1f;

        return worldPos;
    }
    public Vector3Int GetTileAtPosition(Vector3 playerPosition)
    {
        return soilTilemap.WorldToCell(playerPosition);
    }
}