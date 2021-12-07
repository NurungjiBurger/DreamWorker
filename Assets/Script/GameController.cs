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

    private bool gameStart = false;
    private bool revert = false;
    private bool isPause = false;

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

    public bool GameStart { set { gameStart = value; } }
    public bool IsPause { get { return isPause; } }

    public List<GameObject> Room { get { return room; } }
    public List<GameObject> Map { get { return map; } }

    public GameObject[] prfMonsters { get { return prefabMonsters; } }
    public GameObject[] Items { get { return prefabItems; } }
    public GameObject[] Monsters { get { return prefabMonsters; } }
    public GameObject[] Portals { get { return prefabPortals; } }

    private void printalldata()
    {
        for (int idx = 0; idx < data.datas.Count; idx++) Debug.Log(data.datas[idx].structName + "  " + idx);
    }

    // 캐릭터 선택후 게임 시작
    public void PlayerSelect(int value)
    {
        selectedPlayerIndex = value;

        GameObject.Find("Canvas").transform.Find("CharacterSelecter").gameObject.SetActive(false);
        gameStart = true;

        Debug.Log("게임을시작하지");
    }

    // 아이템 획득후 해당 아이템 슬롯화
    public Slot CreateItemSlot(GameObject item, bool isacquired)
    {
        GameObject tmp;
        tmp = Instantiate(prefabSlot);

        tmp.GetComponent<Slot>().InsertImage(item);

        // 인벤토리 행
        if (isacquired)
        {

            item.gameObject.SetActive(false);

            item.GetComponent<ItemStatus>().Data.isAcquired = true;
        }
        // 상점 행
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

    // 다음 스테이지로 넘어가기 위해 현재 스테이지 삭제
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

        data.stageEntrance = false;
        activeRoom = null;

        GameObject.Find("Canvas").transform.Find("Smithy").GetComponent<Smithy>().Clear();
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
            // 천국 과 지옥포탈
            if (selectRoom.transform.position == room[0].transform.position) value = 2;
            else value = 3;
        }

        // 기준방 먼저 포탈 생성
        first = selectRoom.GetComponent<Room>().CreatePortal(direction, value);
        if (direction % 2 == 0) direction += 1;
        else direction -= 1;

        // 기준방을 기준으로 방향 설정후 기준방 포탈과 연동되는 두번째 포탈 생성
        if (value == 2) direction = -1;
        else if (value == 3) direction = -2;
        second = nowRoom.GetComponent<Room>().CreatePortal(direction, value);

        // 두개 포탈의 위치를 저장하여 연결
        first.GetComponent<Portal>().PositionSave(second.GetComponent<Portal>());
        second.GetComponent<Portal>().PositionSave(first.GetComponent<Portal>());
    }

    // 스태이지내 부스테이지 개념의 방 생성
    private void CreateRoom(int cnt)
    {
        int mapPrfNumber;

        if (cnt > 0)
        {
            CreateRoom(cnt - 1);
        }
        else
        {
            // eventroom 생성
            // 천국
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

            // 지옥
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

            // 던전 첫번째 방
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
            // 현재 방을 기준으로 동, 서, 남, 북에 방을 생성 가능
            create = true;
            int number = Random.Range(2, Room.Count);
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

            // 생성되어있는 방 중에 현재 생성하려는 위치에 이미 존재하면 그곳에는 방을 생성할 수 없음
            for (int i=2;i<room.Count;i++)
            {
                if (room[i].transform.position == position) create = false;
            }

            // 해당 위치에 겹치는 방이 없다면 새로운 방을 생성
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

    // 플레이어 위치 변경
    public void RefreshPlayerPosition()
    {
        if (activeRoom != null)
        {
            player.transform.position = activeRoom.transform.position;
            Debug.Log("실행했어!");
        }
    }

    // 게임 종료
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // 메인메뉴로 돌아가기
    public void ReturntoMain()
    {
        Destroy(GameObject.Find("GameController"));
        Destroy(GameObject.Find("Data"));
        GameObject.Find("GameController").GetComponent<GameController>().RevertScene("MainMenu");
    }

    // 플레이어가 속한 방 체크
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

    // 던전씬으로 전환
    public void RevertScene(string scenename)
    {
        revert = false;

        if (scenename == "Dungeon")
        {
            npc.Add(GameObject.Find("BlackSmith").gameObject);

            // 저장된 데이터가 아예 없는 경우 새로 시작한 것이므로 오브젝트 및 데이터 초기화
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
                // 저장된 데이터가 있으면 해당 데이터를 기반으로 이전 게임 환경으로 복구
                Restore();
            }
        }
        else if (scenename == "MainMenu")
        {
            SceneManager.UnloadSceneAsync("Dungeon");
            SceneManager.LoadScene("MainMenu");

            GameStart = false;
        }

    }

    // 해상도 설정
    private void SetResolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
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

        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
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

                if (revert) RevertScene("Dungeon");
                else
                {

                    if (GameObject.FindGameObjectWithTag("Pause") == null) isPause = false;
                    else isPause = true;

                    GameObject.Find("Main Camera").transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y + 1.55f, -10), Quaternion.identity);

                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        Debug.Log("데이터의 수 " + data.datas.Count);
                        for (int idx = 0; idx < data.datas.Count; idx++) Debug.Log(data.datas[idx].structName);
                    }

                    if (true) // 무한 스테이지
                    {
                        data.stageClear = false;
                        if (!data.stageEntrance) 
                        {
                            data.round++;

                            data.stageEntrance = true;

                            data.subStageNumber = Random.Range(10, 16);

                            data.subStageNumber = 3;

                            data.stageNumber = Random.Range(1, (prefabMapDesigns.Length - 2) / 6);

                            CreateRoom(data.subStageNumber);

                        }
                        else
                        {
                            // 플레이어가 속한 방이 존재하지 않을때 버그로 방을 벗어난 것이므로 복구시켜줌
                            if (!PlayerCheck()) RefreshPlayerPosition();
                        }
                    }
                }
            }
        }
    }


    /// ////////////////////////////////////////////////////////////////////////////////

    // 저장된 데이터를 기반으로 복구
    public void Restore()
    {
        GameObject obj;

        for(int idx =0; idx < data.datas.Count; idx++)
        {
            // 맵 복구
            if (data.datas[idx].structName == "Map")
            {
                map.Add(Instantiate(prefabMapDesigns[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity));
                map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);
                // 방 연결
                room.Add(obj = Instantiate(prefabRoom, data.datas[idx].Position(), Quaternion.identity));
                room[room.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);

                room[room.Count - 1].GetComponent<Room>().map = map[map.Count - 1];

                obj.GetComponent<Room>().index = idx;
                obj.GetComponent<Room>().AllocateSubStageNumber(room.Count-1);
                obj.GetComponent<Room>().ConnectData();
            }
            // 플레이어 복구
            else if (data.datas[idx].structName == "Player")
            {
                player = Instantiate(prefabCharacters[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);

                player.GetComponent<PlayerStatus>().index = idx;
            }
            // 아이템 복구
            else if (data.datas[idx].structName == "Item")
            {
                Debug.Log("아이템 복구");
                obj = Instantiate(prefabItems[data.datas[idx].prfNumber], data.datas[idx].Position(), Quaternion.identity);

                obj.GetComponent<ItemStatus>().itemPrfNumber = data.datas[idx].prfNumber;
                obj.GetComponent<ItemStatus>().index = idx;
            }
            // 몬스터 복구
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
