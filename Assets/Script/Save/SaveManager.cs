using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Volume Toggle
    public bool masterToggle = true;
    public bool musicToggle = true;
    public bool sfxToggle = true;

    // SAVE FORMAT (JSON SAFE)
    public List<TileSaveData> tiles = new List<TileSaveData>();

    // RUNTIME ONLY (NOT SAVED)
    [NonSerialized]
    public Dictionary<Vector3Int, TileSaveData> tileDict =
        new Dictionary<Vector3Int, TileSaveData>();
}

[Serializable]
public class TileSaveData
{
    public Vector3Int position;

    public bool hasPlant = false;
    public bool isWatered = false;

    public int growthStage = 0;
    public float growthTimer = 0;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string filePath;
    private string fileName = "Master_Save";

    public SaveData CurrentSave { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        filePath = Path.Combine(Application.persistentDataPath, fileName);

        LoadGame();
    }

    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CurrentSave = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            CurrentSave = new SaveData();
            SaveGame();
        }

        BuildDictionary();
    }

    private void BuildDictionary()
    {
        CurrentSave.tileDict = new Dictionary<Vector3Int, TileSaveData>();

        foreach (var tile in CurrentSave.tiles)
        {
            CurrentSave.tileDict[tile.position] = tile;
        }
    }

    public void SaveGame()
    {
        // Convert dictionary back to list
        if (CurrentSave.tileDict != null)
        {
            CurrentSave.tiles.Clear();

            foreach (var tile in CurrentSave.tileDict.Values)
            {
                CurrentSave.tiles.Add(tile);
            }
        }

        string json = JsonUtility.ToJson(CurrentSave, true);
        File.WriteAllText(filePath, json);
    }

    public void ResetSave()
    {
        CurrentSave = new SaveData();
        SaveGame();
    }
}