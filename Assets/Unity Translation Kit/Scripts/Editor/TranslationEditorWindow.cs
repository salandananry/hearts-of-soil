
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TranslationEditorWindow : EditorWindow
{
    private const string Version = "1.0.0";

    private const string ConfigPath =
        "Unity Translation Kit/Scripts/Editor/translation-editor-config.json";

    [System.Serializable]
    private class ConfigData
    {
        public string apiKey = "";
        public string spreadSheetID = "";
    }

    private string apiKey = "";
    private string spreadSheetID = "";
    private bool showApiKey;

    private readonly List<string> statusHistory = new List<string>();
    private Vector2 statusScrollPosition;

    [MenuItem("Tools/Unity Translation Kit/Translation Window")]
    private static void OpenWindow()
    {
        GetWindow<TranslationEditorWindow>("Translation Kit");
    }

    private void OnEnable()
    {
        LoadConfiguration();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        // Header
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField(
            "Unity Translation Kit",
            EditorStyles.boldLabel
        );

        EditorGUILayout.LabelField(
            "developed by salandananry",
            EditorStyles.miniLabel
        );

        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        EditorGUILayout.LabelField(
            $"v{Version}",
            EditorStyles.miniLabel,
            GUILayout.Width(40)
        );

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(5);

        // Configuration
        EditorGUILayout.LabelField(
            "Configuration",
            EditorStyles.boldLabel
        );

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        apiKey = showApiKey
            ? EditorGUILayout.TextField("API Key", apiKey)
            : EditorGUILayout.PasswordField("API Key", apiKey);

        showApiKey = GUILayout.Toggle(
            showApiKey,
            showApiKey ? "Hide" : "Show",
            EditorStyles.miniButton,
            GUILayout.Width(45)
        );

        EditorGUILayout.EndHorizontal();

        spreadSheetID = EditorGUILayout.TextField(
            "Spreadsheet ID",
            spreadSheetID
        );

        GUILayout.Space(5);

        if (GUILayout.Button("Save Configuration"))
        {
            if (string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(spreadSheetID))
            {
                AddStatusMessage(
                    "Please enter both the API Key and Spreadsheet ID."
                );
            }
            else
            {
                SaveConfiguration();
            }
        }

        GUILayout.Space(10);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(5);

        if (GUILayout.Button(
            "Fetch Translations",
            GUILayout.Height(35)
        ))
        {
            if (string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(spreadSheetID))
            {
                AddStatusMessage(
                    "Please enter both the API Key and Spreadsheet ID."
                );
            }
            else
            {
                AddStatusMessage("Fetching translations...");
                TranslationEditorUpdater.FetchTranslations();
            }
        }

        GUILayout.Space(10);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(
            "Activity Log",
            EditorStyles.boldLabel
        );

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Clear", GUILayout.Width(55)))
        {
            statusHistory.Clear();
        }

        EditorGUILayout.EndHorizontal();

        statusScrollPosition = EditorGUILayout.BeginScrollView(
            statusScrollPosition,
            GUILayout.Height(150)
        );

        if (statusHistory.Count == 0)
        {
            EditorGUILayout.LabelField(
                "No activity yet.",
                EditorStyles.miniLabel
            );
        }
        else
        {
            foreach (string message in statusHistory)
            {
                EditorGUILayout.LabelField(
                    message,
                    EditorStyles.wordWrappedMiniLabel
                );
            }
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
    }

    private void LoadConfiguration()
    {
        string fullPath = Path.Combine(
            Application.dataPath,
            ConfigPath
        );

        if (!File.Exists(fullPath))
        {
            AddStatusMessage(
                "Configuration file not found. " +
                "Please enter your API Key and Spreadsheet ID."
            );
            return;
        }

        try
        {
            string json = File.ReadAllText(fullPath);
            ConfigData config = JsonUtility.FromJson<ConfigData>(json);

            if (config == null)
            {
                AddStatusMessage(
                    "Failed to load configuration: " +
                    "Invalid or empty configuration file."
                );
                return;
            }

            apiKey = config.apiKey ?? "";
            spreadSheetID = config.spreadSheetID ?? "";

            AddStatusMessage("Configuration loaded.");
        }
        catch (Exception exception)
        {
            AddStatusMessage(
                $"Failed to load configuration: {exception.Message}"
            );
        }
    }

    private void SaveConfiguration()
    {
        string fullPath = Path.Combine(
            Application.dataPath,
            ConfigPath
        );

        string directory = Path.GetDirectoryName(fullPath);

        try
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            ConfigData config = new ConfigData
            {
                apiKey = apiKey,
                spreadSheetID = spreadSheetID
            };

            string json = JsonUtility.ToJson(config, true);

            File.WriteAllText(fullPath, json);

            AssetDatabase.Refresh();

            AddStatusMessage("Configuration saved successfully.");
        }
        catch (Exception exception)
        {
            AddStatusMessage(
                $"Failed to save configuration: {exception.Message}"
            );
        }
    }

    public static void AddStatusMessage(string message)
    {
        TranslationEditorWindow window =
            GetWindow<TranslationEditorWindow>("Translation Kit");

        string timestamp = DateTime.Now.ToString("HH:mm:ss");

        window.statusHistory.Add($"[{timestamp}] {message}");
        window.statusScrollPosition.y = float.MaxValue;

        window.Repaint();
    }
}
#endif