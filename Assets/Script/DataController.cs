using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataController : MonoBehaviour
{
    private string dataFile = ".json";
    private string filePath;

    private GameData gameData;

    public GameData GameData { get { return gameData;} }


    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("����");

    }

    public void NewGameData()
    {
        if (File.Exists(filePath))
        {
            Debug.Log("���� ������ �����մϴ�.");
            File.Delete(filePath);
        }
        gameData = new GameData();
        Debug.Log("���ο����ϻ��� �Ϸ�.");

        GameObject.Find("Canvas").transform.Find("CharacterSelecter").gameObject.SetActive(true);
    }

    public void LoadGameData()
    {
        if (File.Exists(filePath))
        {
            Debug.Log("�ҷ���");
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);

            GameObject.Find("GameController").GetComponent<GameController>().GameStart = true;
        }
        else
        {
            Debug.Log("�ҷ��� �����Ͱ� �����ϴ�.");
        }

    }

    public void ExitGame()
    {
        Debug.Log("��������");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void OnApplicationQuit()
    {
        //SaveGameData();
    }

    private void Awake()
    {
        filePath = Application.persistentDataPath + dataFile;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
