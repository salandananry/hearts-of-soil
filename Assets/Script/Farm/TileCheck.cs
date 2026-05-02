using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCheck : MonoBehaviour
{
    [SerializeField] private Tilemap soilTilemap;
    [SerializeField] private TileManager tileManager;

    [Header("Pop Up Status")]
    [SerializeField] private TextMeshProUGUI _statusText;
    
    private Vector3Int lastCell;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Vector3Int cell = soilTilemap.WorldToCell(collision.transform.position);

            if (cell == lastCell) return;

            lastCell = cell;

            if (!tileManager.HasPlant(cell))
            {
                Debug.Log("Tile is available: " + cell);
            }
            else
            {
                Debug.Log("Tile is not available: " + cell);
            }
        }
    }
}