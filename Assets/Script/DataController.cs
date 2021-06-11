using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
    public string dataFile = ".json";
    private string filePath;

    public GameData gameData;

    public GameData GameData
    {
        get
        { if (gameData == null)
            {
                LoadGameData();
                SaveGameData();
            } return gameData;
        }
    }


    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("历厘");

    }

    public void LoadGameData()
    {

        if (File.Exists(filePath))
        {
            Debug.Log("阂矾咳");
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            Debug.Log("货肺款颇老积己");

            gameData = new GameData();
        }

    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    void Start()
    {
        filePath = Application.persistentDataPath + dataFile;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

    }
}
