using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckSoil : MonoBehaviour
{
    [SerializeField] private Tilemap plantableTilemap;
    public void TileCheck(Vector2 playerPosition)
    {
        Vector3Int cellPos = plantableTilemap.WorldToCell(playerPosition);

        if (plantableTilemap.HasTile(cellPos))
        {
            Debug.Log("Plantable tile");
        }
        else
        {
            Debug.Log("Unplantable tile");
        }
    }
}