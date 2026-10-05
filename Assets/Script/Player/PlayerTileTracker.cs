using UnityEngine;

public class PlayerTileTracker : MonoBehaviour
{
    public event System.Action<Vector3Int, Vector3Int> OnPlayerTileChanged;
    [SerializeField] private FarmMechanics farmMechanics;

    private Vector3Int currentTile;

    private void Start()
    {
        currentTile = farmMechanics.TileUtility.WorldToCell(transform.position);
    }

    private void Update()
    {
        Vector3Int newTile =
            farmMechanics.TileUtility.WorldToCell(transform.position);
        
        if (!farmMechanics.TileUtility.HasTile(newTile)) return;
        if (newTile == currentTile) return;

        Vector3Int previousTile = currentTile;
        currentTile = newTile;

        Debug.Log($"Player moved from {previousTile} to {currentTile}");
        // invoke event if the player is inside the 
    }
}
