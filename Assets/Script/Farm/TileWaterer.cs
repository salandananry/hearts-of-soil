using UnityEngine;

public class TileWaterer : MonoBehaviour
{
    [Header("Script reference:")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileMapController tileMapController;
    [SerializeField] private TileUtility tileUtility;

    [Header("Tile Sprites:")]
    [SerializeField] private IsometricRuleTile drySoil; //future use
    [SerializeField] private IsometricRuleTile wetSoil;

    private void Start()
    {
        SpawnExistingWateredTiles();
    }
    private void SpawnExistingWateredTiles()
    {
        tileUtility.ForEachTile(targetTile =>
        {
            if (!HasTile(targetTile)) return;
            if (!IsWatered(targetTile)) return;

            ApplyWaterToTile(targetTile);
        });
    }
    public void WaterSoil(Vector2 position)
    {
        Vector3Int targetTile = tileUtility.WorldToCell(position);

        if (!HasTile(targetTile)) return;
        if (!HasPlant(targetTile)) return;
        if (IsWatered(targetTile)) return;

        ApplyWaterToTile(targetTile);

    }
    private void ApplyWaterToTile(Vector3Int targetTile)
    {
        tileMapController.SetTileSprite(targetTile, wetSoil);
        tileManager.SetHasWater(targetTile, true);
    }
    private bool HasPlant(Vector3Int targetTile)
    {
        return tileManager.HasPlant(targetTile);
    }
    private bool IsWatered(Vector3Int targetTile)
    {
        return tileManager.IsWatered(targetTile);  
    }
    private bool HasTile(Vector3Int targetTile)
    {
        return tileUtility.HasTile(targetTile);
    }
}
