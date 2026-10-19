
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

public static class TranslationEditorUpdater
{
    private const string ConfigFileName =
        "translation-editor-config.json";

    private const string TranslationFileName =
        "translations.json";

    private static bool isUpdating;

    public static async void FetchTranslations()
    {
        if (isUpdating)
        {
            TranslationEditorWindow.AddStatusMessage(
                "A translation update is already in progress."
            );

            return;
        }

        isUpdating = true;

        try
        {
            TranslationEditorConfig config = LoadConfig();

            if (config == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(config.apiKey))
            {
                TranslationEditorWindow.AddStatusMessage(
                    "API Key is missing. Please enter it in the " +
                    "Translation Window and save the configuration."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(config.spreadSheetID))
            {
                TranslationEditorWindow.AddStatusMessage(
                    "Spreadsheet ID is missing. Please enter it in the " +
                    "Translation Window and save the configuration."
                );

                return;
            }

            bool hasConnection =
                await ConnectionStatus.HasConnection();

            if (!hasConnection)
            {
                TranslationEditorWindow.AddStatusMessage(
                    "No internet connection. " +
                    "Translation update cancelled; existing local " +
                    "translations were not changed."
                );

                return;
            }

            await UpdateTranslationFromGoogleSheets(config);
        }
        catch (Exception e)
        {
            TranslationEditorWindow.AddStatusMessage(
                $"Translation update failed: {e.Message}"
            );
        }
        finally
        {
            isUpdating = false;
        }
    }

    private static TranslationEditorConfig LoadConfig()
    {
        string path = Path.Combine(
            Application.dataPath,
            "Unity Translation Kit",
            "Scripts",
            "Editor",
            ConfigFileName
        );

        if (!File.Exists(path))
        {
            TranslationEditorWindow.AddStatusMessage(
                "Configuration file not found. Please save your " +
                "configuration in the Translation Window."
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

            if (config == null)
            {
                TranslationEditorWindow.AddStatusMessage(
                    "Failed to load configuration: " +
                    "Invalid or empty configuration file."
                );
            }

            return config;
        }
        catch (Exception e)
        {
            TranslationEditorWindow.AddStatusMessage(
                $"Failed to load configuration: {e.Message}"
            );

            return null;
        }
    }

    private static async Task UpdateTranslationFromGoogleSheets(
        TranslationEditorConfig config)
    {
        TranslationEditorWindow.AddStatusMessage(
            "Updating translations from Google Sheets..."
        );

        var translations =
            await TranslationSheet.FetchAllSheets(
                config.apiKey,
                config.spreadSheetID
            );

        if (translations == null)
        {
            TranslationEditorWindow.AddStatusMessage(
                "Failed to fetch or process translations. " +
                "Existing local translations were not changed."
            );

            return;
        }

        RemoveEmptyTranslations(translations);
        SaveTranslationData(translations);

        TranslationEditorWindow.AddStatusMessage(
            "Translations successfully updated from Google Sheets."
        );
    }

    private static void RemoveEmptyTranslations(
        Dictionary<string, Dictionary<string, string>> translations)
    {
        if (translations == null)
        {
            return;
        }

        foreach (var entry in translations)
        {
            if (entry.Value == null)
            {
                continue;
            }

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

    private static void SaveTranslationData(
        Dictionary<string, Dictionary<string, string>> translations)
    {
        if (translations == null)
        {
            TranslationEditorWindow.AddStatusMessage(
                "Cannot save translations because the translation " +
                "data is null."
            );

            return;
        }

        string resourcesPath = Path.Combine(
            Application.dataPath,
            "Unity Translation Kit",
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

        string json = JsonConvert.SerializeObject(
            data,
            Formatting.Indented
        );

        File.WriteAllText(path, json);

        AssetDatabase.Refresh();

        TranslationEditorWindow.AddStatusMessage(
            $"Translation file saved to: {path}"
        );
    }
}
#endif