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

    // 게임데이터 삭제
    public void DeleteGameData()
    {
        //Debug.Log("저장 파일을 삭제합니다.");
        File.Delete(filePath);
    }

    // 게임데이터 저장
    public void SaveGameData()
    {
        GameObject.Find("Canvas").transform.Find("Inspector").GetComponent<Inspector>().InspectorStatRerange(-1);

        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        //Debug.Log(jsonData);

        GameObject.Find("Canvas").transform.Find("Inspector").GetComponent<Inspector>().InspectorStatRerange(+1);

        GameObject.Find("GameController").GetComponent<GameController>().setPause(false);

    }

    // 새로운 게임데이터 생성
    public void NewGameData()
    {
        if (File.Exists(filePath))
        {
            DeleteGameData();
        }
        gameData = new GameData();
        //Debug.Log("새로운파일생성 완료.");

        GameObject.Find("Canvas").transform.Find("Tutorial&Story").gameObject.SetActive(true);

        //GameObject.Find("Canvas").transform.Find("CharacterSelecter").gameObject.SetActive(true);
    }

    // 저장된 게임데이터 로드
    public void LoadGameData()
    {
        if (File.Exists(filePath))
        {
           // Debug.Log("불러옴");
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);

            GameObject.Find("GameController").GetComponent<GameController>().GameStart = true;
        }
        else
        {
           // Debug.Log("불러올 데이터가 없습니다.");
        }

    }

    // 진행중인 게임 종료
    public void ExitGame()
    {
       // Debug.Log("게임종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // 튜토리얼 스킵
    public void SkipTutorialAndStory()
    {
        GameObject.Find("Canvas").transform.Find("Tutorial&Story").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("CharacterSelecter").gameObject.SetActive(true);
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
