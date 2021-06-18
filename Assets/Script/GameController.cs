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

    RectTransform hpBar;
    private Image nowHPBar;
    private TextMeshProUGUI textHp;

    public bool IsPause { get { return isPause; } }
    public bool GoNext { get { return data.goNext; } }
    public int StageNumber { get { return data.stageNumber; } }
    public int SubStageNumber { get { return data.subStageNumber; } }
    public List<GameObject> Room { get { return room; } }
    public List<GameObject> Map { get { return map; } }

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

    private void DestroyAll()
    {
        GameObject tmp;
        int cnt = Room.Count;
        for(int i=0;i<cnt;i++)
        {
            tmp = Room[0];
            Room.Remove(Room[0]);
            tmp.GetComponent<Room>().DestoryAll();
            Destroy(tmp);
        }
    }

    private void PortalCreate(GameObject selectRoom, int direction)
    {
        GameObject first, second;
        bool value;

        if (room.Count - 1 < data.subStageNumber) value = false;
        else value = true;

        first = selectRoom.GetComponent<Room>().CreatePortal(direction, value);
        if (direction % 2 == 0) direction += 1;
        else direction -= 1;
        second = Room[Room.Count - 1].GetComponent<Room>().CreatePortal(direction, value);

        first.GetComponent<Portal>().PositionSave(second.GetComponent<Portal>());
        second.GetComponent<Portal>().PositionSave(first.GetComponent<Portal>());
    }

    private void CreateRoom(int cnt)
    {
        int mapPrfNumber;
        if (cnt > 0)
        {
            CreateRoom(cnt - 1);
        }
        else
        {
            pastSelectDirection = 0;
            mapPrfNumber = Random.Range(0, prefabMapDesigns.Length);
            room.Add(Instantiate(prefabRoom, new Vector3(0, 0, 0), Quaternion.identity));
            room[0].transform.SetParent(GameObject.Find("Grid").transform);
            map.Add(Instantiate(prefabMapDesigns[mapPrfNumber], new Vector3(0, 0, 0), Quaternion.identity));
            map[0].transform.SetParent(GameObject.Find("Grid").transform);

            data.maps.Add(new MapData(mapPrfNumber, data.maps.Count, 4, -1));

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
                room.Add(Instantiate(prefabRoom, position, Quaternion.identity));
                room[room.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
                if (cnt == SubStageNumber) mapPrfNumber = prefabMapDesigns.Length - 1;
                else mapPrfNumber = Random.Range(0, prefabMapDesigns.Length);
                map.Add(Instantiate(prefabMapDesigns[mapPrfNumber], position, Quaternion.identity));
                map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);

                room[room.Count-1].GetComponent<Room>().index = data.maps.Count;
                data.maps.Add(new MapData(mapPrfNumber, data.maps.Count, direction, number));

                PortalCreate(selectRoom, direction);

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

        if (data.player == null)
        {
            player = Instantiate(prefabCharacters[0], new Vector3(0, 0, 0), Quaternion.identity);
            player.GetComponent<PlayerStatus>().characterPrfNumber = 0;

            data.stageNumber = 0;
            data.subStageNumber = 0;

            data.stageEntrance = false;  // 스테이지에 처음 들어왔는가?
            data.goNext = false;
        }
        else Restore();
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
        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        else
        {
            Debug.Log(data.items.Count);

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

                    textHp.text = player.GetComponent<PlayerStatus>().Status.nowHP.ToString() + "    /    " + player.GetComponent<PlayerStatus>().Status.maxHP.ToString();
                    // nowHPbar.fillAmount = (float)player.Getnowhp() / (float)player.Getmaxhp();


                    if (GameObject.FindGameObjectWithTag("Pause") == null) isPause = false;
                    else isPause = true;

                    GameObject.Find("Main Camera").transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y, -10), Quaternion.identity);

                    if (data.stageNumber <= 5) // // 5스테이지가 마지막
                    {
                        data.goNext = false;
                        if (!data.stageEntrance) // 처음 스테이지에 입장
                        {
                            Debug.Log("스테이지 처음진입");
                            data.stageEntrance = true;

                            data.subStageNumber = Random.Range(10, 16);
                            //subStageNumber = 0;

                            CreateRoom(data.subStageNumber);
                        }
                        else
                        {
                            Debug.Log("스테이지 중복진입");
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

        // map
        for (int idx = 0; idx < data.maps.Count; idx++ )
        {
            room.Add(obj = Instantiate(prefabRoom, data.maps[idx].Position(), Quaternion.identity));
            room[room.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
            map.Add(Instantiate(prefabMapDesigns[data.maps[idx].mapPrfNumber], data.maps[idx].Position(), Quaternion.identity));
            map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
            obj.GetComponent<Room>().index = data.maps[idx].index;

            if (data.maps[idx].selectRoomIndex != -1) PortalCreate(room[data.maps[idx].selectRoomIndex], data.maps[idx].portalDirection);
        }

        // player
        player = Instantiate(prefabCharacters[data.player.characterPrfNumber], data.player.Position(), Quaternion.identity);

        // item
        for (int idx = 0; idx < data.items.Count; idx++ )
        {
            obj = Instantiate(prefabItems[data.items[idx].itemPrfNumber], data.items[idx].Position(), Quaternion.identity);
            obj.GetComponent<ItemStatus>().index = data.items[idx].index;
        }

        // monster
        for (int idx = 0; idx < data.monsters.Count; idx++)
        {
            if (data.monsters[idx].isBoss) obj = Instantiate(prefabBossMonsters[data.monsters[idx].monsterPrfNumber], data.monsters[idx].Position(), Quaternion.identity);
            else obj = Instantiate(prefabMonsters[data.monsters[idx].monsterPrfNumber], data.monsters[idx].Position(), Quaternion.identity);
            obj.GetComponent<MonsterStatus>().index = data.monsters[idx].index;
        }
    }
}
