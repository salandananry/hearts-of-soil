using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    public enum AudioType
    {
        Master,
        Music,
        SFX
    }

    [SerializeField] private AudioType audioType;
    [SerializeField] private Image toggleImage;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private void Start()
    {
        RefreshUI();
    }

    public void OnTogglePressed()
    {
        switch (audioType)
        {
            case AudioType.Master:
                AudioManager.instance.ToggleMaster();
                break;

            case AudioType.Music:
                AudioManager.instance.ToggleMusic();
                break;

            case AudioType.SFX:
                AudioManager.instance.ToggleSFX();
                break;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        bool isOn = audioType switch
        {
            AudioType.Master => AudioManager.instance.GetMasterToggle(),
            AudioType.Music => AudioManager.instance.GetMusicToggle(),
            AudioType.SFX => AudioManager.instance.GetSFXToggle(),
            _ => true
        };

        toggleImage.sprite = isOn ? onSprite : offSprite;
    }
}