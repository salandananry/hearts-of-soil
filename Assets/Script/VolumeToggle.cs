using UnityEngine;
using UnityEngine.UI;

public class VolumeToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    public System.Action<bool> onValueChanged;

    private void Start()
    {
        toggle.onValueChanged.AddListener(HandleToggle);
    }

    private void HandleToggle(bool isOn)
    {
        toggle.image.sprite = isOn ? onSprite : offSprite;
        onValueChanged?.Invoke(isOn);
    }

    public void SetState(bool isOn)
    {
        toggle.SetIsOnWithoutNotify(isOn);
        toggle.image.sprite = isOn ? onSprite : offSprite;
    }
    public bool GetState()
    {
        return toggle.isOn;
    }
}