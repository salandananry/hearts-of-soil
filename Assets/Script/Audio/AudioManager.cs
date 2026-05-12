using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadAudioSettings();
    }

    #region SET VOLUME

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat(MASTER, AudioUtility.LinearToDB(value));
        SaveManager.instance.CurrentSave.masterVolume = value;
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(MUSIC, AudioUtility.LinearToDB(value));
        SaveManager.instance.CurrentSave.musicVolume = value;
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(SFX, AudioUtility.LinearToDB(value));
        SaveManager.instance.CurrentSave.sfxVolume = value;
    }

    #endregion

    #region SET TOGGLE

    public void ToggleMaster()
    {
        var save = SaveManager.instance.CurrentSave;
        save.masterToggle = !save.masterToggle;

        mixer.SetFloat("MasterVolume",
            save.masterToggle ? 0f : -80f);
    }

    public void ToggleMusic()
    {
        var save = SaveManager.instance.CurrentSave;
        save.musicToggle = !save.musicToggle;

        mixer.SetFloat("MusicVolume",
            save.musicToggle ? 0f : -80f);
    }

    public void ToggleSFX()
    {
        var save = SaveManager.instance.CurrentSave;
        save.sfxToggle = !save.sfxToggle;

        mixer.SetFloat("SFXVolume",
            save.sfxToggle ? 0f : -80f);
    }

    #endregion

    #region GET VOLUME

    public float GetMasterVolume() => SaveManager.instance.CurrentSave.masterVolume;
    public float GetMusicVolume() => SaveManager.instance.CurrentSave.musicVolume;
    public float GetSFXVolume() => SaveManager.instance.CurrentSave.sfxVolume;
    public bool GetMasterToggle() => SaveManager.instance.CurrentSave.masterToggle;
    public bool GetMusicToggle() => SaveManager.instance.CurrentSave.musicToggle;
    public bool GetSFXToggle() => SaveManager.instance.CurrentSave.sfxToggle;

    #endregion

    #region LOAD

    public void LoadAudioSettings()
    {
        var save = SaveManager.instance.CurrentSave;

        SetMasterVolume(save.masterVolume);
        SetMusicVolume(save.musicVolume);
        SetSFXVolume(save.sfxVolume);
    }

    #endregion
}