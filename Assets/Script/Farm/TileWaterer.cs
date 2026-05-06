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

    #region Spawn water tiles from save
    private void Start()
    {
        SpawnExistingWateredTiles();
    }
    private void SpawnExistingWateredTiles()
    {
        tileUtility.ForEachTile(targetTile =>
        {
            if (!tileUtility.HasTile(targetTile)) return;
            if (!tileManager.IsWatered(targetTile)) return;

            ApplyWaterToTile(targetTile);
        });
    }
    #endregion

    #region Watering mechanic
    public void WaterSoil()
    {
        Vector3 pos = playerObject.transform.position;
        Vector3Int targetTile = tileUtility.WorldToCell(pos);

        if (!tileUtility.HasTile(targetTile)) return;
        if (!tileManager.HasPlant(targetTile)) return;
        if (tileManager.IsWatered(targetTile)) return;

        ApplyWaterToTile(targetTile);

        OnTileWatered?.Invoke(targetTile);

    }
    #endregion
    private void ApplyWaterToTile(Vector3Int targetTile)
    {
        tileMapController.SetTileSprite(targetTile, wetSoil);
        tileManager.SetHasWater(targetTile, true);
    }
}