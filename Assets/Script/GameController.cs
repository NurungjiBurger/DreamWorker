using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject newButton;
    [SerializeField]
    private GameObject loadButton;
    [SerializeField]
    private GameObject exitButton;
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

    private GameData data;

    private GameObject player;
    private GameObject inventory;
    private GameObject inspector;

    private bool revert = false;
    private bool isPause = false;

    private int pastSelectDirection;

    private List<GameObject> map = new List<GameObject>();
    private List<GameObject> room = new List<GameObject>();
    private List<GameObject> eventRoom = new List<GameObject>();

    RectTransform hpBar;
    private Image nowHPBar;
    private TextMeshProUGUI textHp;

    public bool IsPause { get { return isPause; } }
    public List<GameObject> EventRoom { get { return eventRoom;} }
    public List<GameObject> Room { get { return room; } }
    public List<GameObject> Map { get { return map; } }

    private void printalldata()
    {
        for (int idx = 0; idx < data.datas.Count; idx++) Debug.Log(data.datas[idx].structName + "  " + idx);
    }

    public void NewButton()
    {
        Debug.Log("new");
    }

    public void LoadButton()
    {
        Debug.Log("Load");
    }

    public void ExitButton()
    {
        Debug.Log("Exit");
    }

    public Slot CreateItemSlot(GameObject item)
    {
        GameObject tmp;
        tmp = Instantiate(prefabSlot);

        tmp.GetComponent<Slot>().InsertImage(item);

        item.gameObject.SetActive(false);

        item.GetComponent<ItemStatus>().Data.isAcquired = true;

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

    public void DestroyNowStage()
    {
        GameObject tmp;
        int cnt = room.Count;
        for(int i=0;i<cnt;i++)
        {
            tmp = room[0];
            room.Remove(Room[0]);
            tmp.GetComponent<Room>().DestoryAll();
            Destroy(tmp);

            tmp = map[0];
            map.Remove(map[0]);
            Destroy(tmp);
        }

        data.stageNumber++;
        data.stageEntrance = false;
    }

    public void PortalCreate(GameObject selectRoom, GameObject nowRoom, int direction)
    {
        GameObject first, second;
        int value;

        if (room.Count - 1 < data.subStageNumber) value = 0;
        else value = 1;

        if (data.stageClear)
        {
            if (direction == -1) value = 2;
            else if (direction == -2) value = 3;

            direction = -3;
        }

        first = selectRoom.GetComponent<Room>().CreatePortal(direction, value);
        if (direction % 2 == 0) direction += 1;
        else direction -= 1;

        if (value == 2) direction = -1;
        else if (value == 3) direction = -2;
        second = nowRoom.GetComponent<Room>().CreatePortal(direction, value);

        first.GetComponent<Portal>().PositionSave(second.GetComponent<Portal>());
        second.GetComponent<Portal>().PositionSave(first.GetComponent<Portal>());
    }

    private void CreateEventRoom()
    {
        // -300, 300
        // 300, 300
        eventRoom.Add(Instantiate(prefabEventMapDesigns[0], new Vector3(300, 300, 0), Quaternion.identity));
        eventRoom[0].transform.SetParent(GameObject.Find("Grid").transform);
        eventRoom[0].transform.Find("GreenMap_Wall").GetComponent<Room>().map = eventRoom[0];
        eventRoom.Add(Instantiate(prefabEventMapDesigns[1], new Vector3(-300, -300, 0), Quaternion.identity));
        eventRoom[1].transform.SetParent(GameObject.Find("Grid").transform);
        eventRoom[1].transform.Find("GreenMap_Wall").GetComponent<Room>().map = eventRoom[1];
    }

    private void CreateRoom(int cnt)
    {
        int mapPrfNumber;

        if (cnt > 0)
        {
            CreateRoom(cnt - 1);
        }
        else
        {// 0~5 1
            // 6~11 2
            pastSelectDirection = 0;
            mapPrfNumber = Random.Range((6 * (data.stageNumber - 1)), (6 * data.stageNumber) - 1);
            map.Add(Instantiate(prefabMapDesigns[mapPrfNumber], new Vector3(0, 0, 0), Quaternion.identity));
            map[0].transform.SetParent(GameObject.Find("Grid").transform);
            room.Add(Instantiate(prefabRoom, new Vector3(0, 0, 0), Quaternion.identity));
            room[0].transform.SetParent(GameObject.Find("Grid").transform);

            room[0].GetComponent<Room>().map = map[0];
            room[0].GetComponent<Room>().mapPrfNumber = mapPrfNumber;
            room[0].GetComponent<Room>().dir = 4;
            room[0].GetComponent<Room>().sel = -1;

            return;
        }

        bool complete = false;
        bool create = true;
        GameObject selectRoom;
        int direction = Random.Range(0, 4);
        if (Random.Range(0, 2) == 0) direction = pastSelectDirection;

        while (!complete)
        {
            create = true;
            int number = Random.Range(0, Room.Count);
            selectRoom = Room[number];
            Vector3 position = selectRoom.transform.position;

            switch (direction)
            {
                case 0: // 동
                    position.x += 30;
                    break;
                case 1: // 서
                    position.x -= 30;
                    break;
                case 2: // 남
                    position.y -= 24;
                    break;
                case 3: // 북
                    position.y += 24;
                    break;
                default:
                    break;
            }

            for (int i=0;i<room.Count;i++)
            {
                if (room[i].transform.position == position) create = false;
            }

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

                PortalCreate(selectRoom, Room[Room.Count - 1], direction);

                complete = true;
            }
        }
    }

    private bool CheckRoom(string str)
    {
        return true;
    }

    private void RevertScene()
    {
        revert = false;

        if (data.datas.Count == 0)
        {
            player = Instantiate(prefabCharacters[0], new Vector3(0, 0, 0), Quaternion.identity);

            player.GetComponent<PlayerStatus>().characterPrfNumber = 0;

            data.stageNumber = 1;
            data.subStageNumber = 0;

            data.stageEntrance = false;  // 스테이지에 처음 들어왔는가?
            data.stageClear = false;
            data.eventRoomVisit = false;
        }
        else
        {
            Restore();
            for (int idx = 0; idx <= data.subStageNumber; idx++) room[idx].GetComponent<Room>().AllocateSubStageNumber(idx);
        }
    }

    private void Awake()
    {
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
        if (Input.GetKey(KeyCode.A)) printalldata();

        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        else
        {            

            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                if (Input.GetKey(KeyCode.Backspace))
                {
                    SceneManager.LoadScene("Dungeon");
                    revert = true;
                }
            }
            else if (SceneManager.GetActiveScene().name == "Dungeon")
            {
                if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
                if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;


                if (revert) RevertScene();
                else
                {

                    hpBar = GameObject.Find("Canvas").transform.Find("PlayerHPBar").GetComponent<RectTransform>();
                    nowHPBar = hpBar.transform.GetChild(0).GetComponent<Image>();


                    textHp = nowHPBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                    textHp.text = player.GetComponent<PlayerStatus>().Data.nowHP.ToString() + "    /    " + player.GetComponent<PlayerStatus>().Data.maxHP.ToString();
                    nowHPBar.fillAmount = (float)player.GetComponent<PlayerStatus>().Data.nowHP / (float)player.GetComponent<PlayerStatus>().Data.maxHP;


                    if (GameObject.FindGameObjectWithTag("Pause") == null) isPause = false;
                    else isPause = true;

                    GameObject.Find("Main Camera").transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y, -10), Quaternion.identity);

                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        Debug.Log("데이터의 수 " + data.datas.Count);
                        for (int idx = 0; idx < data.datas.Count; idx++) Debug.Log(data.datas[idx].structName);
                    }

                    if (data.stageNumber <= 5) // // 5스테이지가 마지막
                    {
                        Debug.Log(data.stageNumber);
                        data.stageClear = false;
                        if (!data.stageEntrance) // 처음 스테이지에 입장
                        {
                            data.stageEntrance = true;

                            data.subStageNumber = Random.Range(10, 16);

                            data.subStageNumber = 3;

                            CreateRoom(data.subStageNumber);

                            CreateEventRoom();

                            for (int idx = 0; idx <= data.subStageNumber; idx++) room[idx].GetComponent<Room>().AllocateSubStageNumber(idx);
                        }
                        else
                        {
                            if (data.stageClear)
                            {

                            }

                            CheckRoom("visible");
                        }
                    }
                }

            }
        }
    }


    /// ////////////////////////////////////////////////////////////////////////////////


    public void Restore()
    {
        GameObject obj;

        for(int idx =0; idx < data.datas.Count; idx++)
        {
            //printalldata();

            if (data.datas[idx].structName == "Map")
            {
                map.Add(Instantiate(prefabMapDesigns[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity));
                map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
                room.Add(obj = Instantiate(prefabRoom, data.datas[idx].Position(), Quaternion.identity));
                room[room.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);

                room[room.Count - 1].GetComponent<Room>().map = map[map.Count - 1];

                obj.GetComponent<Room>().index = idx;

                if (data.datas[idx].selectRoomIndex != -1) PortalCreate(room[data.datas[idx].selectRoomIndex], room[room.Count - 1], data.datas[idx].portalDirection);
            }
            else if (data.datas[idx].structName == "Player")
            {
                player = Instantiate(prefabCharacters[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);

                player.GetComponent<PlayerStatus>().index = idx;
            }
            else if (data.datas[idx].structName == "Item")
            {
                obj = Instantiate(prefabItems[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);

                obj.GetComponent<ItemStatus>().index = idx;
            }
            else if (data.datas[idx].structName == "Monster")
            {
                if (data.datas[idx].isBoss) obj = Instantiate(prefabBossMonsters[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);
                else obj = Instantiate(prefabMonsters[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);

                obj.GetComponent<MonsterStatus>().index = idx;
            }
        }

        CreateEventRoom();
       // if (Room[Room.Count - 1].GetComponent<Room>().Data.isClear) Room[Room.Count - 1].GetComponent<Room>().BossClearAfter();
    }
}
