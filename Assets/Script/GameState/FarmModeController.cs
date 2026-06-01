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
    public event Action <FarmMode> OnFarmModeChange;
    public FarmMode CurrentFarmMode { get; private set; }

    public void NormalMode() => SetState(FarmMode.Normal);
    public void InspectionMode() => SetState(FarmMode.Inspect);
    public void PlantMode() => SetState(FarmMode.Plant);
    public void WaterMode() => SetState(FarmMode.Water);
    public void HarvestMode() => SetState(FarmMode.Harvest);

    private void SetState(FarmMode newState)
    {
        if (CurrentFarmMode == newState) return;
        CurrentFarmMode = newState; 
        OnFarmModeChange?.Invoke(newState);
    }
}