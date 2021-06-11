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
        Debug.Log("����");

    }

    public void LoadGameData()
    {

        if (File.Exists(filePath))
        {
            Debug.Log("�ҷ���");
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            Debug.Log("���ο����ϻ���");

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
