using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{

    private GameData data;
    private Data dataM = null;

    public int mapPrfNumber;
    public int index = -1;
    public int dir = -1;
    public int sel = -1;
    public GameObject map;

    public bool isEvent = false;
    public bool isPlayer = false;

    private int subStageNumber;

    private GameObject player;
    private GameController gameController;

    private GameObject[] monsters;
    private List<GameObject> portals = new List<GameObject>();

    private List<GameObject> objects = new List<GameObject>();

    private int population;
    private float mx, my;

    private bool isGoNext;
    private bool monsterPresence;

    public bool Visible { get { return dataM.visible; } }
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
        gameController.PortalCreate(gameController.EventRoom[0].transform.Find("GreenMap_Wall").gameObject , gameController.Room[gameController.Room.Count - 1], -1);
        gameController.PortalCreate(gameController.EventRoom[1].transform.Find("GreenMap_Wall").gameObject , gameController.Room[gameController.Room.Count - 1], -2);
    }

    private void ManageMap()
    {
        bool value;

        if (GameObject.FindGameObjectsWithTag("Monster").Length == 0)
        {
            monsterPresence = false;
            value = true;
        }
        else
        {
            monsterPresence = true;
            value = false;
        }

        for (int i = 0; i < portals.Count; i++) portals[i].SetActive(value);
    }

    private GameObject CreateMonster(GameObject prefab, Vector3 position) { return Instantiate(prefab, position, Quaternion.identity); }

    private void CreateStage(bool boss) 
    {
        if (!dataM.monsterCreate)
        {
            dataM.monsterCreate = true;
            if (data.datas[index-1].structName == "Map")
            {
                if (boss)
                {
                    CreateMonster(gameController.PrefabReturn("BossMonster", 0), new Vector3(transform.position.x, transform.position.y, transform.position.z)).GetComponent<MonsterStatus>().monsterPrfNumber = 0;
                }
                else
                {
                    population = Random.Range(8, 10);
                    //population = 1;

                    for (int i = 0; i < population; i++)
                    {
                        int type;
                        Vector3 safePosition;
                        type = Random.Range((data.stageNumber - 1) * 4, (data.stageNumber * 4) - 1);
                        List<float> range = map.GetComponent<Map>().SafeMonsterPosition;
                        int cnt = Random.Range(0, range.Count / 4);
                        safePosition = new Vector3(transform.position.x + Random.Range(range[(4 * cnt) + 0], range[(4 * cnt) + 1]), transform.position.y + Random.Range(range[(4 * cnt) + 2], range[(4 * cnt) + 3]), transform.position.z);
                        CreateMonster(gameController.PrefabReturn("Monster", type), safePosition).GetComponent<MonsterStatus>().monsterPrfNumber = type;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    void Start()
    {
        if (index == -1)
        { 
            if (!isEvent)
            {

                index = data.datas.Count;

                data.datas.Add(new Data("Map", mapPrfNumber, index, null, null, dir, sel));

                dataM = data.datas[index];

                monsterPresence = false;
            }
        }
        else
        {
            dataM = data.datas[index];
        }
    }

    private void FixedUpdate()
    {
        if (player && map)
        {
            if (player.transform.position.x <= transform.position.x + 11.0f && player.transform.position.x >= transform.position.x - 11.0f)
            {
                if (player.transform.position.y <= transform.position.y + 7.5f && player.transform.position.y >= transform.position.y - 7.5f)
                {
                    isPlayer = true;
                    if (!isEvent) // 일반 필드 
                    {
                        Debug.Log(transform.position + " 이벤트맵 x ");
                        if (!dataM.isClear)
                        {
                            if (!dataM.subStageEntrance)
                            {
                                dataM.visible = true;
                                dataM.subStageEntrance = true;

                                if (subStageNumber != 0 && subStageNumber == data.subStageNumber)
                                {
                                    if (Vector3.Distance(player.transform.position, transform.position) <= 6) CreateStage(true);
                                    else dataM.subStageEntrance = false;

                                }
                                else CreateStage(false);
                            }
                            else    // 게임 진행중 
                            {
                                if (!monsterPresence)
                                {
                                    if (subStageNumber == data.subStageNumber)
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

                            if (subStageNumber == data.subStageNumber)
                            {
                                if (data.eventRoomVisit)
                                {
                                    gameController.DestroyNowStage();
                                    data.eventRoomVisit = false;
                                    player.transform.position = new Vector3(0, 0, 0);
                                }
                            }
                            //Debug.Log("여기는 " + index + "번째 방이고 클리어 하셨습니다.");
                        }
                    }
                    else // 이벤트 필드
                    {
                        data.eventRoomVisit = true;
                    }
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

        if (!isEvent)
        {
            if (dataM == null) dataM = data.datas[index];
            else dataM.SetPosition(transform.position);
        }

        ManageMap();

       // Debug.Log("나의 방 번호는 " + subStageNumber + "인덱스는 " + index);
    }
}
