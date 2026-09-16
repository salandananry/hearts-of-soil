using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    private void Start()
    {
        LoadSelectedLanguage();
        LoadDefaultTranslationFile();

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