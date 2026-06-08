using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using System;

[System.Serializable]
public class LanguageFileData
{
    public string selectedLanguage;
    public Dictionary<string, Dictionary<string, string>> translations;
}

public class LanguageDatabase : MonoBehaviour
{
    public static LanguageDatabase Instance { get; private set; }

    [SerializeField] private string apiKey;
    [SerializeField] private string spreadSheetID;

    [SerializeField] private string fileName = "translations.json";

    private Dictionary<string, Dictionary<string, string>> _translations;

    public string SelectedLanguage { get; set; } = "en";
    public bool IsInitialized { get; private set; }

    public static event Action OnInitalized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }
    private async void Start()
    {
        LoadSelectedLanguage();

        bool HasConnection = await ConnectionStatus.HasConnection();
        if (HasConnection)
        {
            await UpdateTranslationFile();
        }
        else
        {
            LoadLocalTranslationFile();
        }
        IsInitialized = true;
        OnInitalized?.Invoke();
    }
    private async Task UpdateTranslationFile()
    {
        _translations = await TranslationSheet.FetchAllSheets(apiKey, spreadSheetID);

        if (_translations == null)
        {
            Debug.LogWarning("Failed to download spreadsheet data. Using cached file.");
            LoadLocalTranslationFile();
            return;
        }
        Debug.Log("Translation successfully fetched.");
        RemoveEmptyTranslations();
        SaveTranslationData();
    }
    private void LoadSelectedLanguage()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        var data = JsonConvert.DeserializeObject<LanguageFileData>(json);

        if (data == null) return;

        SelectedLanguage = data.selectedLanguage ?? "en";
        Debug.Log("Loaded selected language from local file.");

    }
    private void LoadLocalTranslationFile()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);

        var data = JsonConvert.DeserializeObject<LanguageFileData>(json);

        if (data == null || data.translations == null) return;

        _translations = data.translations;

        Debug.Log("Loaded translations from local file.");
    }
    private void RemoveEmptyTranslations()
    {
        foreach (var entry in _translations)
        {
            var LanguageCodes = new List<string>(entry.Value.Keys);

            foreach (var code in LanguageCodes)
            {
                if (string.IsNullOrWhiteSpace(entry.Value[code]))
                    entry.Value.Remove(code);
            }
        }
    }
    public void SaveTranslationData()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        var data = new LanguageFileData
        {
            selectedLanguage = SelectedLanguage,
            translations = _translations
        };

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(path, json);
        //Debug.Log("Selected Language and Translations saved successfully.");
    }
    public string GetTranslation(string key, string fallback)
    {
        if (_translations == null)
            return fallback;

        if (!_translations.ContainsKey(key))
            return fallback;

        if (_translations[key].ContainsKey(SelectedLanguage))
            return _translations[key][SelectedLanguage];

        return fallback;
    }
}