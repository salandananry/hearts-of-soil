using UnityEngine;
using System;

public class TileWaterer : MonoBehaviour
{
    [Header("Script reference:")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileMapController tileMapController;
    [SerializeField] private TileUtility tileUtility;

    [Header("Tile Sprites:")]
    [SerializeField] private IsometricRuleTile drySoil;
    [SerializeField] private IsometricRuleTile wetSoil;


    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;

    public event Action <Vector3Int> OnTileWatered;

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
    public void WaterSoil()
    {
        Vector3 pos = playerObject.transform.position;
        Vector3Int targetTile = tileUtility.WorldToCell(pos);

        if (!HasTile(targetTile)) return;
        if (!HasPlant(targetTile)) return;
        if (IsWatered(targetTile)) return;

        ApplyWaterToTile(targetTile);

        OnTileWatered?.Invoke(targetTile);

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
