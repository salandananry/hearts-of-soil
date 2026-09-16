
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

[Serializable]
public class TranslationEditorConfig
{
    public string apiKey;
    public string spreadSheetID;
}

[InitializeOnLoad]
public static class TranslationEditorUpdater
{
    private const string ConfigFileName =
        "translation-editor-config.json";

    private const string TranslationFileName =
        "translations.json";

    private static bool isUpdating;

    [InitializeOnEnterPlayMode]
    private static void OnEnterPlayMode(
        EnterPlayModeOptions options
    )
    {
        if (isUpdating)
            return;

        StartUpdate();
    }

    private static async void StartUpdate()
    {
        isUpdating = true;
        EditorApplication.isPlaying = false;

        try
        {
            TranslationEditorConfig config =
                LoadConfig();

            if (config == null)
            {
                Debug.LogWarning(
                    "Translation Editor Config could not be loaded. " +
                    "Using existing local translation file."
                );

                FinishUpdate();
                return;
            }

            if (string.IsNullOrWhiteSpace(config.apiKey))
            {
                Debug.LogWarning(
                    "Google Sheets API key is missing. " +
                    "Using existing local translation file."
                );

                FinishUpdate();
                return;
            }

            if (string.IsNullOrWhiteSpace(config.spreadSheetID))
            {
                Debug.LogWarning(
                    "Google Sheets Spreadsheet ID is missing. " +
                    "Using existing local translation file."
                );

                FinishUpdate();
                return;
            }

            bool hasConnection =
                await ConnectionStatus.HasConnection();

            if (hasConnection)
            {
                await UpdateTranslationFromGoogleSheets(
                    config
                );
            }
            else
            {
                Debug.LogWarning(
                    "No internet connection. " +
                    "Using existing local translation file."
                );
            }
        }
        catch (Exception e)
        {
            Debug.LogError(
                $"Failed to update translations:\n{e}"
            );
        }

        FinishUpdate();
    }

    private static TranslationEditorConfig LoadConfig()
    {
        string path = Path.Combine(
            Application.dataPath,
            "Scripts",
            "Localization",
            "Editor",
            ConfigFileName
        );

        if (!File.Exists(path))
        {
            Debug.LogWarning(
                $"Translation Editor Config not found:\n{path}"
            );

            return null;
        }

        try
        {
            string json = File.ReadAllText(path);

            TranslationEditorConfig config =
                JsonConvert.DeserializeObject<TranslationEditorConfig>(
                    json
                );

            return config;
        }
        catch (Exception e)
        {
            Debug.LogError(
                $"Failed to read Translation Editor Config:\n{e}"
            );

            return null;
        }
    }

    private static async Task UpdateTranslationFromGoogleSheets(
        TranslationEditorConfig config
    )
    {
        Debug.Log(
            "Updating translations from Google Sheets..."
        );

        var translations =
            await TranslationSheet.FetchAllSheets(
                config.apiKey,
                config.spreadSheetID
            );

        if (translations == null)
        {
            Debug.LogWarning(
                "Failed to download translations. " +
                "Using existing local translation file."
            );

            return;
        }

        RemoveEmptyTranslations(translations);

        SaveTranslationData(translations);

        Debug.Log(
            "Translations successfully updated from Google Sheets."
        );
    }

    private static void RemoveEmptyTranslations(
        Dictionary<string, Dictionary<string, string>> translations
    )
    {
        if (translations == null)
            return;

        foreach (var entry in translations)
        {
            var languageCodes =
                new List<string>(entry.Value.Keys);

            foreach (var code in languageCodes)
            {
                if (string.IsNullOrWhiteSpace(
                    entry.Value[code]
                ))
                {
                    entry.Value.Remove(code);
                }
            }
        }
    }

    private static void SaveTranslationData(
        Dictionary<string, Dictionary<string, string>> translations
    )
    {
        if (translations == null)
        {
            Debug.LogWarning(
                "Cannot save translations because " +
                "translation data is null."
            );

            return;
        }

        string resourcesPath = Path.Combine(
            Application.dataPath,
            "Resources"
        );

        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
        }

        string path = Path.Combine(
            resourcesPath,
            TranslationFileName
        );

        var data = new TranslationData
        {
            translations = translations
        };

        string json =
            JsonConvert.SerializeObject(
                data,
                Formatting.Indented
            );

        File.WriteAllText(path, json);

        AssetDatabase.Refresh();

        Debug.Log(
            $"Translation file saved to:\n{path}"
        );
    }
    private static void FinishUpdate()
    {
        isUpdating = false;
        EditorApplication.EnterPlaymode();
    }
}
#endif