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

    public void DeleteGameData()
    {
        Debug.Log("저장 파일을 삭제합니다.");
        File.Delete(filePath);
    }

    public void SaveGameData()
    {
        GameObject.Find("Canvas").transform.Find("Inspector").GetComponent<Inspector>().InspectorStatRerange(-1);

        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("저장");

        GameObject.Find("Canvas").transform.Find("Inspector").GetComponent<Inspector>().InspectorStatRerange(+1);

    }

    public void NewGameData()
    {
        if (File.Exists(filePath))
        {
            DeleteGameData();
        }
        gameData = new GameData();
        Debug.Log("새로운파일생성 완료.");

        GameObject.Find("Canvas").transform.Find("CharacterSelecter").gameObject.SetActive(true);
    }

    public void LoadGameData()
    {
        if (File.Exists(filePath))
        {
            Debug.Log("불러옴");
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);

            GameObject.Find("GameController").GetComponent<GameController>().GameStart = true;
        }
        else
        {
            Debug.Log("불러올 데이터가 없습니다.");
        }

    }

    public void ExitGame()
    {
        Debug.Log("게임종료");
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
