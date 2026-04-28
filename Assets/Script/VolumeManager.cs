using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private VolumeToggle masterToggle;
    [SerializeField] private VolumeToggle musicToggle;
    [SerializeField] private VolumeToggle sfxToggle;

    private void Start()
    {
        LoadFromSave();
    }
    private void LoadFromSave()
    {
        var save = SaveManager.Instance.CurrentSave;

        masterToggle.SetState(save.masterToggle);
        musicToggle.SetState(save.musicToggle);
        sfxToggle.SetState(save.sfxToggle);
    }
    public void SaveVolumeData()
    {
        var save = SaveManager.Instance.CurrentSave;

        save.masterToggle = masterToggle.GetState();
        save.musicToggle = musicToggle.GetState();
        save.sfxToggle = sfxToggle.GetState();

        SaveManager.Instance.SaveGame();
    }
}