using System;
using UnityEngine;

public enum  FarmMode
{
    Normal,
    Inspect,
    Plant,
    Water,
    Harvest
}
public class FarmModeController : MonoBehaviour
{
    private PlantController plantController;
    private WaterController waterController;
    private HarvestController harvestController;
    public event Action <FarmMode> OnFarmModeChange;
    public FarmMode CurrentFarmMode { get; private set; }
    public void Initialize(
        PlantController plantController,
        WaterController waterController,
        HarvestController harvestController)
    {
        this.plantController = plantController;
        this.waterController = waterController;
        this.harvestController = harvestController;
    }
    public void NormalMode() => SetState(FarmMode.Normal);
    public void InspectionMode() => SetState(FarmMode.Inspect);
    public void PlantMode() => SetState(FarmMode.Plant);
    public void GoPlant() => plantController.PlantSelectedTile();
    public void GoWater() => waterController.WaterSelectedTile();
    public void GoHarvest() => harvestController.HarvestSelectedTile();
    public void WaterMode() => SetState(FarmMode.Water);
    public void HarvestMode() => SetState(FarmMode.Harvest);

    private void SetState(FarmMode newState)
    {
        if (CurrentFarmMode == newState) return;
        CurrentFarmMode = newState; 
        OnFarmModeChange?.Invoke(newState);
    }
}