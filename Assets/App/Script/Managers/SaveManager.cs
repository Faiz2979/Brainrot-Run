using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SettingsData
{
    public bool ToggleMusicActive;
    // add new data if needed
}

[Serializable]
public class SaveData
{
    public int Coins;
    public float HighScore;
    // add new data if needed
    public SettingsData SettingsData;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private const string FILENAME_SAVEDATA = "saveData.json"; // create file "saveData.json"
    private string SaveNamePath => Path.Combine(Application.persistentDataPath, FILENAME_SAVEDATA); // store that file to the folder path on the device

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);

        if (FILENAME_SAVEDATA == null || FILENAME_SAVEDATA.Length == 0) { Debug.LogWarning("File Name not exist"); return; }
    }

    public void SaveGame()
    {
        if (SaveNamePath.Length == 0) { Debug.LogWarning("File path not exist"); return; }
        if (GameManager.Instance == null) return;

        // save data to json file based on data from GameManager
        SaveData data = new SaveData
        {
            Coins = GameManager.Instance.Coins,
            HighScore = GameManager.Instance.HighScore,
            // add new data if needed
            SettingsData = new SettingsData
            {
                ToggleMusicActive = GameManager.Instance.ToggleMusicActive,
                // add new data if needed
            }
        };

        string dataJson = JsonUtility.ToJson(data);
        try
        {
            File.WriteAllText(SaveNamePath, dataJson);
            Debug.Log($"game saved to: {SaveNamePath}");
            // save data and load data at the same time
            LoadGame();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }

    }

    public void LoadGame()
    {
        if (SaveNamePath.Length == 0) { Debug.LogWarning("File path not exist"); return; }
        if (GameManager.Instance == null) return;

        // load data game(GameManager) from file json 
        try
        {
            string dataJson = File.ReadAllText(SaveNamePath);
            SaveData data = JsonUtility.FromJson<SaveData>(dataJson); // get data file json for read data

            GameManager.Instance.SetCoins(data.Coins); // get data from file json to set data in GameManager
            GameManager.Instance.SetHighScore(data.HighScore); // get data from file json to set data in GameManager
            GameManager.Instance.SetToggleMusicActive(data.SettingsData.ToggleMusicActive); // get data from file json to set data in GameManager

            Debug.Log($"Game loaded from: {SaveNamePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
        }
    }

    public void ResetData()
    {
        // for clear data or delete data
        if (File.Exists(SaveNamePath))
        {
            File.Delete(SaveNamePath);
            Debug.Log($"Save file deleted: {SaveNamePath}");
        }
        else
        {
            Debug.Log("No save file to deleted");
        }
    }
}
