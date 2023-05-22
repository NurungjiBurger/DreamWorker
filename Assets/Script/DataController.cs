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

    // ���ӵ����� ����
    public void DeleteGameData()
    {
        //Debug.Log("���� ������ �����մϴ�.");
        File.Delete(filePath);
    }

    // ���ӵ����� ����
    public void SaveGameData()
    {
        GameObject.Find("Canvas").transform.Find("Inspector").GetComponent<Inspector>().InspectorStatRerange(-1);

        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        //Debug.Log(jsonData);

        GameObject.Find("Canvas").transform.Find("Inspector").GetComponent<Inspector>().InspectorStatRerange(+1);

        GameObject.Find("GameController").GetComponent<GameController>().setPause(false);

    }

    // ���ο� ���ӵ����� ����
    public void NewGameData()
    {
        if (File.Exists(filePath))
        {
            DeleteGameData();
        }
        gameData = new GameData();
        //Debug.Log("���ο����ϻ��� �Ϸ�.");

        GameObject.Find("Canvas").transform.Find("Tutorial&Story").gameObject.SetActive(true);

        //GameObject.Find("Canvas").transform.Find("CharacterSelecter").gameObject.SetActive(true);
    }

    // ����� ���ӵ����� �ε�
    public void LoadGameData()
    {
        if (File.Exists(filePath))
        {
           // Debug.Log("�ҷ���");
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);

            GameObject.Find("GameController").GetComponent<GameController>().GameStart = true;
        }
        else
        {
           // Debug.Log("�ҷ��� �����Ͱ� �����ϴ�.");
        }

    }

    // �������� ���� ����
    public void ExitGame()
    {
       // Debug.Log("��������");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Ʃ�丮�� ��ŵ
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
