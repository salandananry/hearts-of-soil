using UnityEngine;
using System;

public class TileWaterer : MonoBehaviour
{
    private FarmMechanics farmMechanics;

    [Header("Tile Sprites:")]
    [SerializeField] private IsometricRuleTile drySoil;
    [SerializeField] private IsometricRuleTile wetSoil;


    [Header("Player object:")]
    [SerializeField] private GameObject playerObject;

    public event Action <Vector3Int> OnTileWatered;

    public void Initialize(
        FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
        SpawnExistingWateredTiles();
    }
    #region Spawn water tiles from save
    private void SpawnExistingWateredTiles()
    {
        farmMechanics.TileUtility.ForEachTile(targetTile =>
        {
            if (!farmMechanics.TileUtility.HasTile(targetTile)) return;
            if (!farmMechanics.TileManager.IsWatered(targetTile)) return;

            ApplyWaterToTile(targetTile);
        });
    }
    #endregion

    #region Watering mechanic
    public void WaterSoil()
    {
        Vector3 pos = playerObject.transform.position;
        Vector3Int targetCell = farmMechanics.TileUtility.WorldToCell(pos);

        if (!farmMechanics.TileUtility.HasTile(targetCell)) return;
        if (!farmMechanics.TileManager.HasPlant(targetCell)) return;
        if (farmMechanics.TileManager.IsWatered(targetCell)) return;

        ApplyWaterToTile(targetCell);

        OnTileWatered?.Invoke(targetCell);

    }
    #endregion
    private void ApplyWaterToTile(Vector3Int targetTile)
    {
        farmMechanics.TileMapController.SetTileSprite(targetTile, wetSoil);
        farmMechanics.TileManager.SetHasWater(targetTile, true);
    }
}