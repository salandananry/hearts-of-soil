using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Sprite defaultCursor;
    [SerializeField] private Sprite plantCursor;
    [SerializeField] private Sprite waterCursor;
    [SerializeField] private Sprite harvestCursor;


    private FarmMechanics farmMechanic;
    private void Start()
    {
        Texture2D texture = defaultCursor.texture;

        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }
    /*
    public void Initialize(FarmMechanics farmMechanic)
    {
        this.farmMechanic = farmMechanic;
        ApplyCursor(farmMechanic.FarmModeController.CurrentFarmMode);
        SubscribeEvents();
        Debug.Log("Farmmode initialize");
    }
    private void OnFarmModeChangeHandler(FarmMode farmMode)
    {
        Debug.Log("Farmmode change");
        ApplyCursor(farmMode);
    }
    private void ApplyCursor(FarmMode farmMode)
    {
        Texture2D texture = GetCursorTexture(farmMode);

        Cursor.SetCursor(
            texture,
            Vector2.zero,
            CursorMode.Auto
        );
    }
    private Texture2D GetCursorTexture(FarmMode farmMode)
    {
        switch (farmMode)
        {
            case FarmMode.Plant:
                return plantCursor.texture;

            case FarmMode.Water:
                return waterCursor.texture;

            case FarmMode.Harvest:
                return harvestCursor.texture;

            case FarmMode.Inspect:
            default:
                return defaultCursor.texture;
        }
    }
    private void SubscribeEvents()
    {
        farmMechanic.FarmModeController.OnFarmModeChange += OnFarmModeChangeHandler;
    }
    private void OnDestroy()
    {
        farmMechanic.FarmModeController.OnFarmModeChange -= OnFarmModeChangeHandler;
    }
    */
}