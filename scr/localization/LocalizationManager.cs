using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : Singleton<LocalizationManager>
{   
    private Dictionary<string, string> list;
    private bool isReady = false;
    private string missingTextString = "text not found";
    
    public void Initialization()
    {
        //로드한다.
        string fileName = PlayerPrefs.GetString("language");
        list = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log(Application.streamingAssetsPath);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                list.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + list.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    public string GetValue(string key)
    {
        string result = missingTextString;
        if (list.ContainsKey(key))
        {
            result = list[key];
        }

        return result;

    }

    public bool GetIsReady()
    {
        return isReady;
    }

}