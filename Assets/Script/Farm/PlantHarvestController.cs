using UnityEngine;
using UnityEngine.UI;

public class PlantHarvestController : MonoBehaviour
{
    [SerializeField] private GameObject playerPosition;
    [SerializeField] private Button harvestButton;

    private FarmMechanics farmMechanics;

    public void Initialize(FarmMechanics farmMechanics)
    {
        this.farmMechanics = farmMechanics;
        SubscribeEvents();
        harvestButton.onClick.AddListener(() => HarvestPlant());
    }
    private void HarvestPlant()
    {
        var pos = playerPosition.transform.position;
        Vector3Int targetTile = farmMechanics.TileUtility.WorldToCell(pos);
        farmMechanics.FarmManager.HarvestPlant(targetTile);
        Debug.Log("harvesting plant" + targetTile);
    }
    private void OnHarvestPlantHandler(Vector3Int targetTile)
    {
        GameObject targetPlant =
            farmMechanics.plantController.GetPlant(targetTile);

        Destroy(targetPlant);
        farmMechanics.plantController.RemovePlant(targetTile);
    }
    #region SUBSCRIBE AND UNSUBSCRIBE EVENT
    private void SubscribeEvents()
    {
        farmMechanics.FarmManager.OnHarvestPlant += OnHarvestPlantHandler;
    }
    private void OnDestroy()
    {
        farmMechanics.FarmManager.OnHarvestPlant -= OnHarvestPlantHandler;
    }
    #endregion
}