using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{

    private bool isGoNext;
    private bool monsterPresence;
    public bool isEvent = false;
    public bool isPlayer = false;

    private int subStageNumber;
    private int population;
    private float mx, my;
    public int mapPrfNumber;
    public int index = -1;
    public int dir = -1;
    public int sel = -1;

    private GameObject player;
    private GameObject[] monsters;
    public GameObject map;

    private GameController gameController;
    private List<GameObject> portals = new List<GameObject>();
    private List<GameObject> objects = new List<GameObject>();
    private GameData data;
    private Data dataM = null;

    public bool Visible { get { return dataM.visible; } }
    public int SubStageNumber { get { return subStageNumber; } }
    public Data Data { get { return dataM; } }

    public void AllocateSubStageNumber(int num)
    {
        subStageNumber = num;
    }

    public void DestoryAll()
    {
        GameObject tmp;
        int cnt = portals.Count;
        for (int i = 0; i < cnt; i++)
        {
            tmp = portals[0];
            portals.Remove(portals[0]);
            if (tmp.GetComponent<Portal>().MiniMapPortalIcon) Destroy(tmp.GetComponent<Portal>().MiniMapPortalIcon);
            Destroy(tmp);
        }

        if (dataM != null)
        {
            data.datas.Remove(dataM);
            for (int idx = index; idx < data.datas.Count; idx++)
            {
                data.datas[idx].index = idx;
            }
        }
    }

    public GameObject CreatePortal(int direction, int value)
    {
        while (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();

        GameObject portal;

        portal = Instantiate(gameController.PrefabReturn("Portal", value));

        List<float> range = map.GetComponent<Map>().SafePortalPosition;

        switch(direction)
        {
            case 0: // 동
                portal.transform.position = new Vector3(transform.position.x + Random.Range(range[0], range[1]), transform.position.y + Random.Range(range[2], range[3]), transform.position.z);
                break;
            case 1: // 서
                portal.transform.position = new Vector3(transform.position.x + Random.Range(range[4], range[5]), transform.position.y + Random.Range(range[6], range[7]), transform.position.z);
                break;
            case 2: // 남
                portal.transform.position = new Vector3(transform.position.x + Random.Range(range[8], range[9]), transform.position.y + Random.Range(range[10], range[11]), transform.position.z);
                break;
            case 3: // 북
                portal.transform.position = new Vector3(transform.position.x + Random.Range(range[12], range[13]), transform.position.y + Random.Range(range[14], range[15]), transform.position.z);
                break;
            case -1: // 가운데 오른쪽
                portal.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, transform.position.z);
                break;
            case -2: // 가운데 왼쪽
                portal.transform.position = new Vector3(transform.position.x - 3.5f, transform.position.y, transform.position.z);
                break;
            case -3: // 가운데
                portal.transform.position = transform.position;
                break;
            default:
                break;
        }

        portals.Add(portal);

        return portal;
    }

    public void BossClearAfter()
    {
        gameController.PortalCreate(gameController.Room[1].gameObject , gameController.Room[gameController.Room.Count - 1], -3);
        gameController.PortalCreate(gameController.Room[0].gameObject , gameController.Room[gameController.Room.Count - 1], -3);
    }

    private void ManageMap()
    {
        if (GameObject.FindGameObjectsWithTag("Monster").Length == 0) monsterPresence = false;
        else monsterPresence = true;

        if (true)
        {
            for (int i = 0; i < portals.Count; i++) portals[i].SetActive(!monsterPresence);

            for (int i = 0; i < portals.Count; i++)
            {
                if (portals[i].GetComponent<Portal>().MiniMapPortalIcon)
                {
                    if (portals[i].activeSelf && isPlayer) portals[i].GetComponent<Portal>().MiniMapPortalIcon.SetActive(true);
                    else portals[i].GetComponent<Portal>().MiniMapPortalIcon.SetActive(false);
                }
            }
        }
    }

    private GameObject CreateMonster(GameObject prefab, Vector3 position) { return Instantiate(prefab, position, Quaternion.identity); }

    private void CreateStage(bool boss, bool isevent) 
    {
        if (!dataM.monsterCreate)
        {
            dataM.monsterCreate = true;

            GameObject tmp;
            int type;
            type = Random.Range(0, gameController.prfMonsters.Length);
            population = Random.Range(8, 10);

            if (!isevent)
            {
                if (data.datas[index - 1].structName == "Map")
                {

                    if (boss)
                    {

                        tmp = CreateMonster(gameController.PrefabReturn("Monster", type), new Vector3(transform.position.x, transform.position.y, transform.position.z));
                        tmp.GetComponent<MonsterStatus>().monsterPrfNumber = type;
                        tmp.GetComponent<MonsterStatus>().Boss = true;
                        tmp.GetComponent<MonsterStatus>().BeTheBossMonster(isevent);

                    }
                    else
                    {

                        for (int i = 0; i < population; i++)
                        {
                            type = Random.Range(0, gameController.prfMonsters.Length);
                            Vector3 safePosition;
                            List<float> range = map.GetComponent<Map>().SafeMonsterPosition;
                            int cnt = Random.Range(0, range.Count / 4);
                            safePosition = new Vector3(transform.position.x + Random.Range(range[(4 * cnt) + 0], range[(4 * cnt) + 1]), transform.position.y + Random.Range(range[(4 * cnt) + 2], range[(4 * cnt) + 3]), transform.position.z);
                            tmp = CreateMonster(gameController.PrefabReturn("Monster", type), safePosition);
                            tmp.GetComponent<MonsterStatus>().monsterPrfNumber = type;
                        }
                    }
                }
            }
            else
            {
                if (subStageNumber == 1)
                {
                    tmp = CreateMonster(gameController.PrefabReturn("Monster", type), new Vector3(transform.position.x, transform.position.y, transform.position.z));
                    tmp.GetComponent<MonsterStatus>().monsterPrfNumber = type;
                    tmp.GetComponent<MonsterStatus>().Boss = true;
                    tmp.GetComponent<MonsterStatus>().BeTheBossMonster(isevent);
                }
            }
        }
    }

    public void ConnectData()
    {
        if (index > -1) dataM = data.datas[index];
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    void Start()
    {
        if (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (index == -1)
        { 
            index = data.datas.Count;

            data.datas.Add(new Data("Map", mapPrfNumber, index, null, null, dir, sel, isEvent));

            dataM = data.datas[index];

            monsterPresence = false;
        }
        else
        {
            if (index > -1 ) dataM = data.datas[index];
        }

        if (!dataM.isEvent && dataM.selectRoomIndex != -1)
        {
            gameController.PortalCreate(gameController.Room[dataM.selectRoomIndex], gameController.Room[subStageNumber], dataM.portalDirection);
        }
    }

    private void FixedUpdate()
    {
        if (player && map)
        {
            if (player.transform.position.x <= transform.position.x + 12.0f && player.transform.position.x >= transform.position.x - 12.0f)
            {
                if (player.transform.position.y <= transform.position.y + 7.5f && player.transform.position.y >= transform.position.y - 7.5f)
                {
                    isPlayer = true;
                    if (!dataM.isEvent) // 일반 필드 
                    {
                        if (!dataM.isClear)
                        {
                            if (!dataM.subStageEntrance)
                            {
                                if (GameObject.FindGameObjectWithTag("Monster")) player.transform.position = new Vector3(-1000, -1000, -1000);
                                else
                                {
                                    dataM.visible = true;
                                    dataM.subStageEntrance = true;

                                    if (subStageNumber != 2 && subStageNumber == data.subStageNumber + 2)
                                    {
                                        if (Vector3.Distance(player.transform.position, transform.position) <= 6) CreateStage(true, false);
                                        else dataM.subStageEntrance = false;

                                    }
                                    else if (subStageNumber == 2)
                                    {
                                        dataM.isClear = true;
                                    }
                                    else CreateStage(false, false);
                                }
                            }
                            else    // 게임 진행중 
                            {
                                if (!monsterPresence)
                                {
                                    if (subStageNumber == gameController.Room.Count - 1)
                                    {
                                        if (!data.stageClear)
                                        {
                                            data.stageClear = true;
                                            BossClearAfter();
                                        }
                                    }

                                    if (!dataM.isClear) dataM.isClear = true;
                                }
                            }
                        }
                        else
                        {
                            // 보스방
                            if (subStageNumber == gameController.Room.Count - 1)
                            {

                                if (data.eventRoomVisit)
                                {
                                    gameController.DestroyNowStage();
                                    data.eventRoomVisit = false;
                                    player.transform.position = new Vector3(0, 0, 0);
                                }
                            }
                           // Debug.Log("여기는 " + index + "번째 방이고 클리어 하셨습니다.");
                        }
                    }
                    else // 이벤트 필드
                    {
                        data.eventRoomVisit = true;

                        if (gameObject == gameController.Room[1])
                        {
                            CreateStage(true, true);
                            // 보스 출현
                        }

                    }
                }
                else
                {
                    isPlayer = false;
                    // 해당 방에 플레이어가 존재하지 않는 경우
                }
            }
            else
            {
                isPlayer = false;
                // 해당 방에 플레이어가 존재하지 않는 경우
            }
        }
    }

    private void Update()
    {
        if (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();

        if (!player) player = GameObject.FindGameObjectWithTag("Player");


        if (dataM == null) dataM = data.datas[index];
        else dataM.SetPosition(transform.position);

        ManageMap();

        //if (isPlayer)  Debug.Log("나의 방 번호는 " + subStageNumber + "인덱스는 " + (index-2) + "방의 속성은 " + isEvent + "방의 위치는 " + transform.position  );
    }
}
