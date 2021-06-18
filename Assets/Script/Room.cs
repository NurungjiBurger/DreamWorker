using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{

    private GameData data;
    public int index = -1;

    private GameObject map;
    private GameObject player;
    private GameController gameController;

    private GameObject[] monsters;
    private List<GameObject> portals = new List<GameObject>();

    private GameObject tester;
    private bool done = false;

    private int population;
    private float mx, my;

    private bool monsterPresence;

    public bool Visible { get { return data.maps[index].visible; } }

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
    }

    public GameObject CreatePortal(int direction, bool value)
    {
        while (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();
        while (!map) map = gameController.Map[index];

        GameObject portal;

        if (value) portal = Instantiate(gameController.PrefabReturn("Portal", 1));
        else portal = Instantiate(gameController.PrefabReturn("Portal", 0));

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
            default:
                break;
        }

        portals.Add(portal);

        return portal;
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
        if (!data.maps[index].monsterCreate)
        {
            data.maps[index].monsterCreate = true;
            if (index != 0) // 부스테이지 번호가 0 일 경우 몬스터가 없는 스테이지.
            {
                if (boss)
                {
                    CreateMonster(gameController.PrefabReturn("BossMonster", 0), new Vector3(transform.position.x, transform.position.y, transform.position.z)).GetComponent<MonsterStatus>().monsterPrfNumber = 0;
                }
                else
                {
                    population = Random.Range(8, 10);  // 현재 스테이지에 생성할 몬스터의 마릿 수.
                    //population = 2;
                    for (int i = 0; i < population; i++)
                    {
                        int type;
                        Vector3 safePosition;
                        type = Random.Range((gameController.StageNumber - 1) * 4, (gameController.StageNumber * 4) - 1);
                        List<float> range = map.GetComponent<Map>().SafeMonsterPosition;
                        int cnt = Random.Range(0, range.Count / 4);
                        safePosition = new Vector3(transform.position.x + Random.Range(range[(4 * cnt) + 0], range[(4 * cnt) + 1]), transform.position.y + Random.Range(range[(4 * cnt) + 2], range[(4 * cnt) + 3]), transform.position.z);
                        type = 0;
                        CreateMonster(gameController.PrefabReturn("Monster", type), safePosition).GetComponent<MonsterStatus>().monsterPrfNumber = type;    // 랜덤 좌표에 몬스터 생성
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
            index = data.maps.Count-1;

            monsterPresence = false;
        }
        else
        {

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
                    if (!data.maps[index].isClear)
                    {
                        if (!data.maps[index].subStageEntrance)
                        {
                            data.maps[index].visible = true;
                            data.maps[index].subStageEntrance = true;

                            if (index != 0 && index == gameController.SubStageNumber)
                            {
                                Debug.Log("방 처음진입");
                                CreateStage(true);
                            }
                            else CreateStage(false);
                        }
                        else    // 게임 진행중 
                        {
                            Debug.Log("방 중복진입");
                            if (!monsterPresence)
                            {
                                data.maps[index].isClear = true;
                            }
                        }                
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();
        else
        {
            if (!map) map = gameController.Map[index];
        }
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        else data.maps[index].SetPosition(transform.position);

        ManageMap();
               
    }
}
