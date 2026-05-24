using UnityEngine;
using UnityEngine.UI;
public class MuteAndUnmuteButton : MonoBehaviour
{
    public enum AudioType
    {
        Master,
        Music,
        SFX
    }

    [SerializeField] private AudioType audioType;
    [SerializeField] private Image targetIcon;
    [SerializeField] private Sprite unmuteIcon;
    [SerializeField] private Sprite muteIcon;

    private void Start()
    {
        AudioManager.OnAudioSettingsLoaded += RefreshUI;
        RefreshUI();
    }
    private void OnDestroy()
    {
        AudioManager.OnAudioSettingsLoaded -= RefreshUI;
    }
    public void OnTogglePressed()
    {
        var audioManager = AudioManager.instance;
        switch (audioType)
        {
            case AudioType.Master:
                audioManager.MuteUnmuteMaster(!audioManager.GetMasterToggle());
                break;

            case AudioType.Music:
                audioManager.MuteUnmuteMusic(!audioManager.GetMusicToggle());
                break;

            case AudioType.SFX:
                audioManager.MuteUnmuteSFX(!audioManager.GetSFXToggle());
                break;
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        bool isOn = true;
        var audioManager = AudioManager.instance;
        switch (audioType)
        {
            case AudioType.Master:
                isOn = audioManager.GetMasterToggle();
                break;

            case AudioType.Music:
                isOn = AudioManager.instance.GetMusicToggle();
                break;

            case AudioType.SFX:
                isOn = AudioManager.instance.GetSFXToggle();
                break;
        }
        targetIcon.sprite = isOn? unmuteIcon : muteIcon;
    }
}