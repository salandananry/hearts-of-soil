using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public enum AudioType
    {
        Master,
        Music,
        SFX
    }

    [SerializeField] private AudioType audioType;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
        AudioManager.OnAudioSettingsLoaded += InitializeSlider;
        InitializeSlider();
    }
    private void OnDestroy()
    {
        AudioManager.OnAudioSettingsLoaded -= InitializeSlider;
    }
    private void InitializeSlider()
    {
        switch (audioType)
        {
            case AudioType.Master:
                slider.value = AudioManager.instance.GetMasterVolume();
                break;

            case AudioType.Music:
                slider.value = AudioManager.instance.GetMusicVolume();
                break;

            case AudioType.SFX:
                slider.value = AudioManager.instance.GetSFXVolume();
                break;
        }
    }

    private void OnValueChanged(float value)
    {
        switch (audioType)
        {
            case AudioType.Master:
                AudioManager.instance.SetMasterVolume(value);
                break;

            case AudioType.Music:
                AudioManager.instance.SetMusicVolume(value);
                break;

            case AudioType.SFX:
                AudioManager.instance.SetSFXVolume(value);
                break;
        }
    }
}