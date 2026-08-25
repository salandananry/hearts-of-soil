using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Threading.Tasks;
#endif

[System.Serializable]
public class TranslationData
{
    public Dictionary<string, Dictionary<string, string>> translations;
}
[System.Serializable]
public class SelectedLanguageData
{
    public string selectedLanguage;
}

public class LanguageDatabase : MonoBehaviour
{
    public static LanguageDatabase Instance { get; private set; }

    [Header("Default Translation")]
    [SerializeField] private TextAsset defaultTranslationFile;

#if UNITY_EDITOR
    [Header("Google Sheets - Editor Only")]
    [SerializeField] private string apiKey;
    [SerializeField] private string spreadSheetID;
#endif

    [SerializeField] private string fileName = "translations.json";

    private Dictionary<string, Dictionary<string, string>> _translations;

    public string SelectedLanguage { get; set; } = "english";
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
    }

    private async void Start()
    {
        LoadSelectedLanguage();
        LoadDefaultTranslationFile();

#if UNITY_EDITOR
        bool hasConnection = await ConnectionStatus.HasConnection();

        if (hasConnection)
        {
            await UpdateTranslationFromGoogleSheets();
        }
        else
        {
            Debug.LogWarning("No internet connection. Using local translation file.");
        }

#endif

        IsInitialized = true;
        OnInitalized?.Invoke();
    }

    private void LoadDefaultTranslationFile()
    {
        if (defaultTranslationFile == null)
        {
            Debug.LogError("Default Translation File is not assigned!");
            return;
        }

        try
        {
            var data = JsonConvert.DeserializeObject<TranslationData>(
                defaultTranslationFile.text
            );

            if (data == null || data.translations == null)
            {
                Debug.LogError("Failed to parse default translation file.");
                return;
            }

            _translations = data.translations;

            Debug.Log("Loaded translation file.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load translation file:\n{e}");
        }
    }

    private void LoadSelectedLanguage()
    {
        string path = Path.Combine(
       Application.persistentDataPath,
       "selectedLanguage.json"
   );

        if (!File.Exists(path))
        {
            Debug.Log($"No saved language found. Using default: {SelectedLanguage}");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);

            var data = JsonConvert.DeserializeObject<SelectedLanguageData>(
                json
            );

            if (data == null)
                return;

            SelectedLanguage = data.selectedLanguage ?? "english";

            Debug.Log($"Loaded selected language: {SelectedLanguage}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load selected language:\n{e}");
        }
    }

#if UNITY_EDITOR

    private async Task UpdateTranslationFromGoogleSheets()
    {
        Debug.Log("Updating translations from Google Sheets...");

        var translations =
            await TranslationSheet.FetchAllSheets(
                apiKey,
                spreadSheetID
            );

        if (translations == null)
        {
            Debug.LogWarning(
                "Failed to download translations. " +
                "Using local translation file."
            );

            return;
        }

        _translations = translations;

        RemoveEmptyTranslations();

        SaveTranslationData();

        Debug.Log("Translations successfully updated from Google Sheets.");
    }

    private void RemoveEmptyTranslations()
    {
        if (_translations == null)
            return;

        foreach (var entry in _translations)
        {
            var languageCodes =
                new List<string>(entry.Value.Keys);

            foreach (var code in languageCodes)
            {
                if (string.IsNullOrWhiteSpace(entry.Value[code]))
                {
                    entry.Value.Remove(code);
                }
            }
        }
    }

    private void SaveTranslationData()
    {
        if (_translations == null)
        {
            Debug.LogWarning(
                "Cannot save translations because translation data is null."
            );

            return;
        }

        string path = Path.Combine(
            Application.dataPath,
            "Resources",
            fileName
        );

        var data = new TranslationData
        {
            translations = _translations
        };

        string json = JsonConvert.SerializeObject(
            data,
            Formatting.Indented
        );

        File.WriteAllText(path, json);

        AssetDatabase.Refresh();

        Debug.Log($"Translation file saved to:\n{path}");
    }

#endif

    public void SaveSelectedLanguage()
    {
        var data = new SelectedLanguageData
        {
            selectedLanguage = SelectedLanguage
        };

        string path = Path.Combine(
            Application.persistentDataPath,
            "selectedLanguage.json"
        );

        string json = JsonConvert.SerializeObject(
            data,
            Formatting.Indented
        );

        File.WriteAllText(path, json);

        Debug.Log($"Selected language saved: {SelectedLanguage}");
    }

    public string GetTranslation(
        string key,
        string fallback
    )
    {
        if (_translations == null)
            return fallback;

        if (!_translations.TryGetValue(
            key,
            out var translations
        ))
        {
            return fallback;
        }

        if (translations.TryGetValue(
            SelectedLanguage,
            out string translation
        ))
        {
            return translation;
        }

        return fallback;
    }
}