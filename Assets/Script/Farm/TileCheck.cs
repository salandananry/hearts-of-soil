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
        Vector3Int cell = soilTilemap.WorldToCell(collision.transform.position);
        
        if (!soilTilemap.HasTile(cell)) return;

        if (cell == lastCell) return;

        lastCell = cell;

        if (currentStatusText != null)
            Destroy(currentStatusText.gameObject);

        currentStatusText = Instantiate(statusTextPrefab, spawnHolder);

        Vector3 worldPos = soilTilemap.GetCellCenterWorld(cell);
        worldPos.y += 1f;

        currentStatusText.rectTransform.position = worldPos;

        currentStatusText.text = GetSoilOccupancyText(cell) + " and " + GetSoilMoistureText(cell);
    }
    private void HideTileStatus()
    {
        if(currentStatusText.gameObject.activeInHierarchy)
        {
            Destroy(currentStatusText.gameObject);
        }
    }
    private string GetSoilOccupancyText(Vector3Int cell)
    {
        return tileManager.HasPlant(cell)
            ? "This soil is occupied"
            : "This soil is empty";
    }
    private string GetSoilMoistureText(Vector3Int cell)
    {
        return tileManager.IsWatered(cell)
            ? "watered"
            : "dry";
    }
}