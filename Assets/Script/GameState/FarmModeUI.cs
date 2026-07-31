using TMPro;
using UnityEngine;

public class FarmModeUI : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private FarmModeController controller;
    [SerializeField] private UIPanelTransition displayUI;
    [SerializeField] private TextMeshProUGUI displayText;
    #endregion

    #region START
    private void Start()
    {
        SubscriveEvents();
    }
    #endregion

    #region EVENT HANDLERS
    private void OnFarmModeChangeHandler(FarmMode farmMode)
    {
        switch (farmMode)
        {
            /*
            case FarmMode.Normal: 
                displayUI.SetActive(false);
                return;
            */
            case FarmMode.Inspect:
                displayText.text = "inspecting mode";
                break;
            case FarmMode.Plant:
                displayText.text = "planting mode";
                break;
            case FarmMode.Water:
                displayText.text = "watering mode"; ;
                break;
            case FarmMode.Harvest:
                displayText.text = "harvesting mode";
                break;
        }
        displayUI.Open();
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscriveEvents()
    {
        controller.OnFarmModeChange += OnFarmModeChangeHandler;
    }
    private void OnDestroy()
    {
        controller.OnFarmModeChange -= OnFarmModeChangeHandler;
    }
    #endregion
}