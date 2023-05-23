using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject prefabRoom;
    [SerializeField]
    private GameObject prefabSlot;
    [SerializeField]
    private GameObject[] prefabCharacters;
    [SerializeField]
    private GameObject[] prefabMapDesigns;
    [SerializeField]
    private GameObject[] prefabEventMapDesigns;
    [SerializeField]
    private GameObject[] prefabItems;
    [SerializeField]
    private GameObject[] prefabMonsters;
    [SerializeField]
    private GameObject[] prefabBossMonsters;
    [SerializeField]
    private GameObject[] prefabPortals;
    [SerializeField]
    private GameObject[] prefabNpcs;
    [SerializeField]
    private GameObject stageNumber;

    private bool gameStart = false;
    private bool revert = false;
    private bool isPause = false;
    private bool isReuslt = false;
    private bool isSave = false;

    private int selectedPlayerIndex;
    private int pastSelectDirection;

    private GameObject activeRoom;
    private GameObject player;
    private GameObject inventory;
    private GameObject inspector;

    private List<GameObject> map = new List<GameObject>();
    private List<GameObject> room = new List<GameObject>();
    private List<GameObject> npc = new List<GameObject>();
    private GameData data;
    private Data dataM = null;

    public bool GameStart { set { gameStart = value; } }
    public bool IsPause { get { return isPause; } }

    public void setPause(bool val) { isPause = val; }
    public Data Data { get { return dataM; } }

    public List<GameObject> Room { get { return room; } }
    public List<GameObject> Map { get { return map; } }

    public GameObject ActvieRoom { get { return activeRoom; } }
    public GameObject[] prfMonsters { get { return prefabMonsters; } }
    public GameObject[] Items { get { return prefabItems; } }
    public GameObject[] Monsters { get { return prefabMonsters; } }
    public GameObject[] Portals { get { return prefabPortals; } }


    public void BGMPlay(int id)
    {
        if (GetComponent<Audio>().IsNowPlaying()) GetComponent<Audio>().NowPlayingStop();
        GetComponent<Audio>().AudioPlay(id);
        GetComponent<Audio>().IsLoop(true);
    }
    private void printalldata()
    {
        for (int idx = 0; idx < data.datas.Count; idx++) Debug.Log(data.datas[idx].structName + "  " + idx);
    }

    // ĳ���� ������ ���� ����
    public void PlayerSelect(int value)
    {
        selectedPlayerIndex = value;

        GameObject.Find("Canvas").transform.Find("CharacterSelecter").gameObject.SetActive(false);
        gameStart = true;

       // Debug.Log("��������������");
    }

    // ������ ȹ���� �ش� ������ ����ȭ
    public Slot CreateItemSlot(GameObject item, bool isacquired)
    {
        GameObject tmp;
        tmp = Instantiate(prefabSlot);

        tmp.GetComponent<Slot>().InsertImage(item);

        // �κ��丮 ��
        if (isacquired)
        {

            item.gameObject.SetActive(false);

            item.GetComponent<ItemStatus>().Data.isAcquired = true;
        }
        // ���� ��
        else
        {
            tmp.transform.SetParent(GameObject.Find("Canvas").transform.Find("Smithy").transform.Find("Background"));
        }

        return tmp.GetComponent<Slot>();
    }

    public GameObject PrefabReturn(string str, int idx)
    {

        GameObject tmp = null;
        if (str == "BossMonster") tmp = prefabBossMonsters[idx];
        else if (str == "Monster") tmp = prefabMonsters[idx];
        else if (str == "Portal") tmp = prefabPortals[idx];
        else if (str == "Item") tmp = prefabItems[idx];

        return tmp;
    }

    // ���� ���������� �Ѿ�� ���� ���� �������� ����
    public void DestroyNowStage(bool type)
    {
        BGMPlay(Random.Range(0, 1));

        stageNumber.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Stage " + data.stageNumber;
        stageNumber.SetActive(true);
        stageNumber.GetComponent<Timer>().TimerSetZero();
        stageNumber.GetComponent<Timer>().ActiveTrue();
        

        if (type)
        {
            GameObject tmp;
            int cnt = room.Count;
            for (int i = 0; i < cnt; i++)
            {
                tmp = room[0];
                room.Remove(Room[0]);
                tmp.GetComponent<Room>().DestoryAll();
                Destroy(tmp);

                tmp = map[0];
                map.Remove(map[0]);
                Destroy(tmp);
            }

            data.stageEntrance = false;
            activeRoom = null;

            GameObject.Find("Canvas").transform.Find("Smithy").GetComponent<Smithy>().Clear();
        }
    }

    public void PortalCreate(GameObject selectRoom, GameObject nowRoom, int direction)
    {
        GameObject first, second;
        int value=0;

        if (!selectRoom.GetComponent<Room>().Data.isEvent)
        {
            if (nowRoom.GetComponent<Room>().SubStageNumber < room.Count - 1) value = 0;
            else
            {
                value = 1;
            }
        }
        else
        {
            // õ�� �� ������Ż
            if (selectRoom.transform.position == room[0].transform.position) value = 2;
            else value = 3;
        }

        // ���ع� ���� ��Ż ����
        first = selectRoom.GetComponent<Room>().CreatePortal(direction, value);
        if (direction % 2 == 0) direction += 1;
        else direction -= 1;

        // ���ع��� �������� ���� ������ ���ع� ��Ż�� �����Ǵ� �ι�° ��Ż ����
        if (value == 2) direction = -1;
        else if (value == 3) direction = -2;
        second = nowRoom.GetComponent<Room>().CreatePortal(direction, value);

        // �ΰ� ��Ż�� ��ġ�� �����Ͽ� ����
        first.GetComponent<Portal>().PositionSave(second.GetComponent<Portal>());
        second.GetComponent<Portal>().PositionSave(first.GetComponent<Portal>());
    }

    // ���������� �ν������� ������ �� ����
    private void CreateRoom(int cnt)
    {

        int mapPrfNumber;

        if (cnt > 0)
        {
            CreateRoom(cnt - 1);
        }
        else
        {
            // eventroom ����
            // õ��
            map.Add(Instantiate(prefabMapDesigns[prefabMapDesigns.Length-2], new Vector3(300, 300, 0), Quaternion.identity));
            map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
            room.Add(Instantiate(prefabRoom, new Vector3(300, 300, 0), Quaternion.identity));
            room[room.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);

            room[room.Count - 1].GetComponent<Room>().isEvent = true;
            room[room.Count - 1].GetComponent<Room>().map = map[map.Count - 1];
            room[room.Count - 1].GetComponent<Room>().mapPrfNumber = prefabMapDesigns.Length-2;
            room[room.Count - 1].GetComponent<Room>().dir = -1;
            room[room.Count - 1].GetComponent<Room>().sel = -1;

            room[room.Count - 1].GetComponent<Room>().AllocateSubStageNumber(0);

            npc[0].transform.position = room[room.Count-1].transform.position;
            npc[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            // ����
            map.Add(Instantiate(prefabMapDesigns[prefabMapDesigns.Length - 1], new Vector3(-300, -300, 0), Quaternion.identity));
            map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
            room.Add(Instantiate(prefabRoom, new Vector3(-300, -300, 0), Quaternion.identity));
            room[room.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);

            room[room.Count - 1].GetComponent<Room>().isEvent = true;
            room[room.Count - 1].GetComponent<Room>().map = map[map.Count - 1];
            room[room.Count - 1].GetComponent<Room>().mapPrfNumber = prefabMapDesigns.Length - 1;
            room[room.Count - 1].GetComponent<Room>().dir = -1;
            room[room.Count - 1].GetComponent<Room>().sel = -1;

            room[room.Count - 1].GetComponent<Room>().AllocateSubStageNumber(1);

            // ���� ù��° ��
            pastSelectDirection = 0;

            mapPrfNumber = Random.Range((6 * (data.stageNumber - 1)), (6 * data.stageNumber) - 1);
            map.Add(Instantiate(prefabMapDesigns[mapPrfNumber], new Vector3(0, 0, 0), Quaternion.identity));
            map[map.Count-1].transform.SetParent(GameObject.Find("Grid").transform);
            room.Add(Instantiate(prefabRoom, new Vector3(0, 0, 0), Quaternion.identity));
            room[room.Count-1].transform.SetParent(GameObject.Find("Grid").transform);

            room[room.Count-1].GetComponent<Room>().map = map[map.Count-1];
            room[room.Count-1].GetComponent<Room>().mapPrfNumber = mapPrfNumber;
            room[room.Count-1].GetComponent<Room>().dir = 4;
            room[room.Count-1].GetComponent<Room>().sel = -1;

            room[room.Count - 1].GetComponent<Room>().AllocateSubStageNumber(2);

            return;
        }
        bool complete = false;
        bool create = true;
        GameObject selectRoom;
        int direction = Random.Range(0, 4);
        if (Random.Range(0, 2) == 0) direction = pastSelectDirection;

        while (!complete)
        {
            // ���� ���� �������� ��, ��, ��, �Ͽ� ���� ���� ����
            create = true;
            int number = Random.Range(2, Room.Count);
            selectRoom = Room[number];
            Vector3 position = selectRoom.transform.position;

            switch (direction)
            {
                case 0: // ��
                    position.x += 30;
                    break;
                case 1: // ��
                    position.x -= 30;
                    break;
                case 2: // ��
                    position.y -= 24;
                    break;
                case 3: // ��
                    position.y += 24;
                    break;
                default:
                    break;
            }

            // �����Ǿ��ִ� �� �߿� ���� �����Ϸ��� ��ġ�� �̹� �����ϸ� �װ����� ���� ������ �� ����
            for (int i=2;i<room.Count;i++)
            {
                if (room[i].transform.position == position) create = false;
            }

            // �ش� ��ġ�� ��ġ�� ���� ���ٸ� ���ο� ���� ����
            if (create)
            {
                if (cnt == data.subStageNumber) mapPrfNumber = (6 * data.stageNumber) - 1;
                else mapPrfNumber = Random.Range((6 * (data.stageNumber - 1)), (6 * data.stageNumber) - 1);
                map.Add(Instantiate(prefabMapDesigns[mapPrfNumber], position, Quaternion.identity));
                map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
                room.Add(Instantiate(prefabRoom, position, Quaternion.identity));
                room[room.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);

                room[room.Count - 1].GetComponent<Room>().map = map[map.Count - 1];
                room[room.Count - 1].GetComponent<Room>().mapPrfNumber = mapPrfNumber;
                room[room.Count - 1].GetComponent<Room>().dir = direction;
                room[room.Count - 1].GetComponent<Room>().sel = number;

                room[room.Count - 1].GetComponent<Room>().AllocateSubStageNumber(room.Count - 1);

                complete = true;
            }
        }
    }

    // �÷��̾� ��ġ ����
    public void RefreshObjectPosition()
    {
        if (activeRoom != null)
        {
            player.transform.position = activeRoom.transform.position;
            //Debug.Log("�����߾�!");
        }
    }

    // ���� ����
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // ���θ޴��� ���ư���
    public void ReturntoMain()
    {
        GameObject.Find("GameController").GetComponent<GameController>().RevertScene("MainMenu");
    }

    // �÷��̾ ���� �� üũ
    private bool PlayerCheck()
    {
        for (int idx = 0; idx < room.Count; idx++)
        {
            if (room[idx].GetComponent<Room>().isPlayer)
            {
                activeRoom = room[idx];
                return true;
            }
        }

        return false;
    }

    private bool CheckRoom(string str)
    {
        return true;
    }

    // ���ȭ�� ���
    public void AllocateResultScene()
    {
        //Debug.Log("��������");

        // ���� �÷��� ����
        GameObject.Find("Canvas").transform.Find("Background").transform.Find("BoardBackground").transform.Find("ScoreBoard").transform.Find("PlayTime").transform.Find("PlayTimeData").GetComponent<TextMeshProUGUI>().text = (int)data.playTime/3600 + " : " + (int)data.playTime/60%60 + " : " + (int)data.playTime%60;
        GameObject.Find("Canvas").transform.Find("Background").transform.Find("BoardBackground").transform.Find("ScoreBoard").transform.Find("KilledMonster").transform.Find("KilledMonsterData").GetComponent<TextMeshProUGUI>().text = " " + data.numberOfKilledMonster;
        GameObject.Find("Canvas").transform.Find("Background").transform.Find("BoardBackground").transform.Find("ScoreBoard").transform.Find("ObtainedItems").transform.Find("ObtainedItemsData").GetComponent<TextMeshProUGUI>().text = " " + data.numberOfObtainedItems;
        GameObject.Find("Canvas").transform.Find("Background").transform.Find("BoardBackground").transform.Find("ScoreBoard").transform.Find("ObtainedGold").transform.Find("ObtainedGoldData").GetComponent<TextMeshProUGUI>().text = " " + data.obtainedGold;
        GameObject.Find("Canvas").transform.Find("Background").transform.Find("BoardBackground").transform.Find("ScoreBoard").transform.Find("ClearRoom").transform.Find("ClearRoomData").GetComponent<TextMeshProUGUI>().text = " " + data.numberOfClearRoom;

        if (data.winOrLose == "Victory!") GameObject.Find("Canvas").transform.Find("Background").transform.Find("BoardBackground").transform.Find("GameBoard").transform.Find("DeadRoomNumber").transform.Find("RoomNumber").GetComponent<TextMeshProUGUI>().text = "All Clear !";
        else GameObject.Find("Canvas").transform.Find("Background").transform.Find("BoardBackground").transform.Find("GameBoard").transform.Find("DeadRoomNumber").transform.Find("RoomNumber").GetComponent<TextMeshProUGUI>().text = " " + data.stageNumber + " - " + data.subStageNumber;

        if (!isReuslt)
        {
            GameObject.Find("Canvas").transform.Find("Background").transform.Find("GameResult").GetComponent<TextMeshProUGUI>().text = data.winOrLose;
            GameObject.Find("Canvas").transform.Find("Background").transform.Find("MainButton").GetComponent<Button>().onClick.AddListener(ReturntoMain);
            GameObject.Find("Canvas").transform.Find("Background").transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(ExitGame);
            isReuslt = true;
        }
    }

    // ���������� ��ȯ
    public void RevertScene(string scenename)
    {
        revert = false;

        if (scenename == "Dungeon")
        {
            npc.Add(GameObject.Find("BlackSmith").gameObject);

            // ����� �����Ͱ� �ƿ� ���� ��� ���� ������ ���̹Ƿ� ������Ʈ �� ������ �ʱ�ȭ
            if (data.datas.Count == 0)
            {
                player = Instantiate(prefabCharacters[selectedPlayerIndex], new Vector3(0, 0, 0), Quaternion.identity);

                player.GetComponent<PlayerStatus>().characterPrfNumber = selectedPlayerIndex;

                data.round = 0;
                data.stageNumber = 1;
                data.subStageNumber = 0;

                data.stageEntrance = false;
                data.stageClear = false;
                data.eventRoomVisit = false;
            }
            else
            {
                //Debug.Log(JsonUtility.ToJson(data));
                // ����� �����Ͱ� ������ �ش� �����͸� ������� ���� ���� ȯ������ ����
                Restore();
            }

            BGMPlay(Random.Range(0, 1));
        }
        else if (scenename == "MainMenu")
        {
            if (SceneManager.GetActiveScene().name == "Dungeon") SceneManager.UnloadSceneAsync("Dungeon");
            else if (SceneManager.GetActiveScene().name == "Result") SceneManager.UnloadSceneAsync("Result");
            Destroy(GameObject.Find("Data"));
            Destroy(GameObject.Find("GameController"));
            SceneManager.LoadScene("MainMenu");

            GameStart = false;
            isReuslt = false;

            BGMPlay(2);
        }
        else if (scenename == "Result")
        {
            if (SceneManager.GetActiveScene().name == "Dungeon")  SceneManager.UnloadSceneAsync("Dungeon");
            SceneManager.LoadScene("Result");

            GameStart = false;

            BGMPlay(2);
        }

    }

    // �ػ� ����
    private void SetResolution()
    {
        int setWidth = 1920; // ����� ���� �ʺ�
        int setHeight = 1080; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }

    private void Awake()
    {
        //SetResolution();

        DontDestroyOnLoad(gameObject);


    }
    void Start()
    {

    }

    private void FixedUpdate()
    {
        
    }

    void Update()
    {

        if (Input.GetKey("escape"))
        {
            ExitGame();
        }

        if (data == null)
        {
            data = GameObject.Find("Data").GetComponent<DataController>().GameData;

            if (GetComponent<Audio>().IsNowPlaying())
            {
                //Debug.Log("������");
                BGMPlay(2);
            }
        }
        else
        {

            if (SceneManager.GetActiveScene().name == "MainMenu")
            {

                if (gameStart)
                {
                    SceneManager.LoadScene("Dungeon");
                    revert = true;
                    gameStart = false;
                }
            }
            else if (SceneManager.GetActiveScene().name == "Dungeon")
            {
                if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
                if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
                if (!stageNumber)
                {
                    stageNumber = GameObject.Find("Canvas").transform.Find("StageNumber").gameObject;
                    stageNumber.GetComponent<Timer>().SetCooldown(1);
                }

                if (revert) RevertScene("Dungeon");
                else
                {

                    if (GameObject.FindGameObjectWithTag("Pause") == null)
                    {
                        isPause = false;
                        if (data == null) Awake();

                        data.playTime += Time.deltaTime;
                        // Debug.Log(data.playTime + " " + Time.deltaTime);

                    }
                    else
                    {
                        isPause = true;
                    }

                    GameObject.Find("Main Camera").transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y + 1.55f, -10), Quaternion.identity);

                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        Debug.Log("�������� �� " + data.datas.Count);
                        for (int idx = 0; idx < data.datas.Count; idx++) Debug.Log(data.datas[idx].structName);
                    }

                    //if (data.stageNumber <= 6) // 5 ��������
                    if (data.stageNumber <= 3) // 3 ��������
                    {
                        data.stageClear = false;
                        if (!data.stageEntrance)
                        {
                            data.round++;

                            data.stageEntrance = true;

                            data.subStageNumber = Random.Range(10, 16);

                            //data.subStageNumber = 3;

                            //data.stageNumber = Random.Range(1, (prefabMapDesigns.Length - 2) / 6);

                            DestroyNowStage(false);

                            CreateRoom(data.subStageNumber);

                        }
                        else
                        {
                            // �÷��̾ ���� ���� �������� ������ ���׷� ���� ��� ���̹Ƿ� ����������
                            if (!PlayerCheck()) RefreshObjectPosition();
                        }
                    }
                    else
                    {
                        data.winOrLose = "Victory !";
                        RevertScene("Result");
                    }
                }
            }
            else if (SceneManager.GetActiveScene().name == "Result") AllocateResultScene();
        }
    }


    /// ////////////////////////////////////////////////////////////////////////////////

    // ����� �����͸� ������� ����
    public void Restore()
    {
        GameObject obj;

        for(int idx =0; idx < data.datas.Count; idx++)
        {
            // �� ����
            if (data.datas[idx].structName == "Map")
            {
                map.Add(Instantiate(prefabMapDesigns[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity));
                map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
                // �� ����
                room.Add(obj = Instantiate(prefabRoom, data.datas[idx].Position(), Quaternion.identity));
                room[room.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);

                room[room.Count - 1].GetComponent<Room>().map = map[map.Count - 1];

                obj.GetComponent<Room>().index = idx;
                obj.GetComponent<Room>().AllocateSubStageNumber(room.Count-1);
                obj.GetComponent<Room>().ConnectData();
            }
            // �÷��̾� ����
            else if (data.datas[idx].structName == "Player")
            {
                player = Instantiate(prefabCharacters[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);

                player.GetComponent<PlayerStatus>().index = idx;
            }
            // ������ ����
            else if (data.datas[idx].structName == "Item")
            {
                obj = Instantiate(prefabItems[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);

                obj.GetComponent<ItemStatus>().itemPrfNumber = data.datas[idx].prfNumber;
                obj.GetComponent<ItemStatus>().index = idx;
            }
            // ���� ����
            else if (data.datas[idx].structName == "Monster")
            {
                if (data.datas[idx].isBoss) obj = Instantiate(prefabMonsters[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);
                else obj = Instantiate(prefabMonsters[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);

                obj.GetComponent<MonsterStatus>().index = idx;
            }
        }

        npc.Add(GameObject.Find("BlackSmith").gameObject);
        npc[0].transform.position = room[0].transform.position;
    }
}
