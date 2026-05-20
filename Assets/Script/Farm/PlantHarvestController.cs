using UnityEngine;
using UnityEngine.UI;

public class PlantHarvestController : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private GameObject playerPosition;
    [SerializeField] private Button harvestButton;

    private FarmMechanics farmMechanics;
    #endregion

    #region INITIALIZE
    public void Initialize(FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
        harvestButton.onClick.AddListener(() => HarvestPlant());
    }
    #endregion

    #region PLAYER ACTION
    private void HarvestPlant()
    {
        var pos = playerPosition.transform.position;
        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(pos);
        farmMechanics.FarmManager.HarvestPlant(targetTile);
    }
    #endregion
}