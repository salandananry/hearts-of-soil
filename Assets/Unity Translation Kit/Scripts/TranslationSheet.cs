using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class TranslationSheet
{
    public static async Task<
        Dictionary<string, Dictionary<string, string>>
    > FetchAllSheets(
        string apiKey,
        string spreadsheetId
    )
    {
        string metaUrl =
            $"https://sheets.googleapis.com/v4/spreadsheets/" +
            $"{spreadsheetId}?key={apiKey}";

        string metaJson = await GetRequest(metaUrl);

        if (metaJson == null)
            return null;

        JObject meta;

        try
        {
            meta = JObject.Parse(metaJson);
        }
        catch
        {
            Debug.LogError("Failed to parse Google Sheets metadata.");
            return null;
        }

        JArray sheets = (JArray)meta["sheets"];

        if (sheets == null)
        {
            Debug.LogError("Google Sheets response contains no sheets.");
            return null;
        }

        var flatResult =
            new Dictionary<
                string,
                Dictionary<string, string>
            >();

        foreach (var sheet in sheets)
        {
            string sheetName =
                sheet["properties"]["title"].ToString();

            string sheetUrl =
                $"https://sheets.googleapis.com/v4/" +
                $"spreadsheets/{spreadsheetId}/values/" +
                $"{sheetName}?key={apiKey}";

            string sheetDataJson =
                await GetRequest(sheetUrl);

            if (sheetDataJson == null)
                continue;

            var parsedSheet =
                ParseSheet(sheetDataJson);

            if (parsedSheet == null)
                continue;

            foreach (var kv in parsedSheet)
            {
                flatResult[kv.Key] = kv.Value;
            }
        }

        return flatResult;
    }

    private static Dictionary<
        string,
        Dictionary<string, string>
    > ParseSheet(string json)
    {
        JObject sheetObj;

        try
        {
            sheetObj = JObject.Parse(json);
        }
        catch
        {
            Debug.LogError("Failed to parse Google Sheet data.");
            return null;
        }

        JArray values =
            (JArray)sheetObj["values"];

        if (values == null || values.Count < 2)
            return null;

        var result =
            new Dictionary<
                string,
                Dictionary<string, string>
            >();

        JArray header =
            (JArray)values[0];

        for (
            int rowIndex = 1;
            rowIndex < values.Count;
            rowIndex++
        )
        {
            JArray row =
                (JArray)values[rowIndex];

            if (row == null || row.Count == 0)
                continue;

            string key =
                row[0]?.ToString();

            if (string.IsNullOrWhiteSpace(key))
                continue;

            var translations =
                new Dictionary<string, string>();

            for (
                int col = 1;
                col < header.Count;
                col++
            )
            {
                string language =
                    header[col]?.ToString();

                if (string.IsNullOrWhiteSpace(language))
                    continue;

                string text =
                    col < row.Count
                        ? row[col]?.ToString()
                        : "";

                translations[language] = text;
            }

            result[key] = translations;
        }

        return result;
    }

    private static async Task<string> GetRequest(
        string url
    )
    {
        using (UnityWebRequest request =
               UnityWebRequest.Get(url))
        {
            request.timeout = 5;

            var operation =
                request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (
                request.result ==
                UnityWebRequest.Result.Success
            )
            {
                return request.downloadHandler.text;
            }

            Debug.LogError(
                $"Translation request failed.\n" +
                $"URL: {url}\n" +
                $"Result: {request.result}\n" +
                $"Error: {request.error}\n" +
                $"Response Code: {request.responseCode}"
            );

            return null;
        }
    }
}