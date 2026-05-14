using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public static event Action OnAudioSettingsLoaded;

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    private const string MASTER = "MasterVolume";
    private const string MUSIC = "MusicVolume";
    private const string SFX = "SFXVolume";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #region LOAD AUDIO SETTINGS
    private void Start()
    {
        LoadAudioSettings();
    }
    public void LoadAudioSettings()
    {
        var save = SaveManager.instance.CurrentSave;

        SetMasterVolume(save.masterVolume);
        SetMusicVolume(save.musicVolume);
        SetSFXVolume(save.sfxVolume);

        OnAudioSettingsLoaded?.Invoke();
    }

    #endregion

    #region SET VOLUME VIA SLIDER

    public void SetMasterVolume(float value)
    {
        var save = SaveManager.instance.CurrentSave;
        save.masterVolume = value;
        ApplyMasterAudio();
    }

    public void SetMusicVolume(float value)
    {
        var save = SaveManager.instance.CurrentSave;
        save.musicVolume = value;
        ApplyMusicAudio();
    }

    public void SetSFXVolume(float value)
    {
        var save = SaveManager.instance.CurrentSave;
        save.sfxVolume = value;
        ApplySFXAudio();
    }

    #endregion

    #region SET TOGGLE

    public void MuteUnmuteMaster(bool isUnmute)
    {
        var save = SaveManager.instance.CurrentSave;
        save.isMasterVolumeUnmute = isUnmute;
        ApplyMasterAudio();
    }

    public void MuteUnmuteMusic(bool isUnmute)
    {
        var save = SaveManager.instance.CurrentSave;
        save.isMusicVolumeUnmute = isUnmute;
        ApplyMusicAudio();
    }

    public void MuteUnmuteSFX(bool isUnmute)
    {
        var save = SaveManager.instance.CurrentSave;
        save.isSFXVolumeUnmute = isUnmute;
        ApplySFXAudio();
    }

    #endregion

    private void ApplyMasterAudio()
    {
        var save = SaveManager.instance.CurrentSave;

        mixer.SetFloat(
            MASTER,
            save.isMasterVolumeUnmute
                ? AudioUtility.LinearToDB(save.masterVolume)
                : -80f
        );
    }
    private void ApplyMusicAudio()
    {
        var save = SaveManager.instance.CurrentSave;

        mixer.SetFloat(
            MUSIC,
            save.isMusicVolumeUnmute
                ? AudioUtility.LinearToDB(save.musicVolume)
                : -80f
        );
    }
    private void ApplySFXAudio()
    {
        var save = SaveManager.instance.CurrentSave;

        mixer.SetFloat(
            SFX,
            save.isSFXVolumeUnmute
                ? AudioUtility.LinearToDB(save.sfxVolume)
                : -80f
        );
    }
    #region GET VOLUME

    public float GetMasterVolume() => SaveManager.instance.CurrentSave.masterVolume;
    public float GetMusicVolume() => SaveManager.instance.CurrentSave.musicVolume;
    public float GetSFXVolume() => SaveManager.instance.CurrentSave.sfxVolume;
    public bool GetMasterToggle() => SaveManager.instance.CurrentSave.isMasterVolumeUnmute;
    public bool GetMusicToggle() => SaveManager.instance.CurrentSave.isMusicVolumeUnmute;
    public bool GetSFXToggle() => SaveManager.instance.CurrentSave.isSFXVolumeUnmute;

    #endregion
}