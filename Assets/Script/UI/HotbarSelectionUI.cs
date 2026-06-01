using UnityEngine;

public class HotbarSelectionUI : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private FarmModeController controller;

    [SerializeField] private RectTransform selectionFrame;

    [SerializeField] private RectTransform[] hotbarSlots;
    #endregion

    #region START
    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        UpdateSelection(controller.CurrentFarmMode);
        SubscribeEvents();
    }
    #endregion

    #region EVENT HANDLERS
    private void UpdateSelection(FarmMode farmMode)
    {
        int targetSlot = GetSlot(farmMode);
        selectionFrame.position = hotbarSlots[targetSlot].position;
    }
    private int GetSlot(FarmMode farmMode)
    {
        switch (farmMode)
        {
            case FarmMode.Normal:
                return 0;
            case FarmMode.Inspect:
                return 1;

            case FarmMode.Plant:
                return 2;

            case FarmMode.Water:
                return 3;

            case FarmMode.Harvest:
                return 4;
            default:
                return 0;
        }
    }
    #endregion

    #region SUBSCRIPTIONS
    private void SubscribeEvents()
    {
        controller.OnFarmModeChange += UpdateSelection;
    }
    private void OnDestroy()
    {
        controller.OnFarmModeChange -= UpdateSelection;
    }
    #endregion
}