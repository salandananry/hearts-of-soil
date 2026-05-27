using TMPro;
using UnityEngine;

public class FarmModeUI : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private GameObject displayUI;
    [SerializeField] private TextMeshProUGUI displayText;
    private FarmMechanics farmMechanic;
    #endregion

    #region INITIALIZE
    public void Initalize(FarmMechanics farmMechanic)
    {
        this.farmMechanic = farmMechanic;
        SubscriveEvents();
    }
    #endregion

    #region EVENT HANDLERS
    private void OnFarmModeChangeHandler(FarmMode farmMode)
    {
        switch (farmMode)
        {
            case FarmMode.Inspect:
                displayText.text = "[inspecting mode]";
                break;
            case FarmMode.Plant:
                displayText.text = "[planting mode]";
                break;
            case FarmMode.Water:
                displayText.text = "[watering mode]"; ;
                break;
            case FarmMode.Harvest:
                displayText.text = "[harvesting mode]";
                break;
        }
        displayUI.SetActive(true);
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscriveEvents()
    {
        farmMechanic.FarmModeController.OnFarmModeChange += OnFarmModeChangeHandler;
    }
    private void OnDestroy()
    {
        farmMechanic.FarmModeController.OnFarmModeChange -= OnFarmModeChangeHandler;
    }
    #endregion
}