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

    private GameObject player;
    private GameController gameController;

    private GameObject[] monsters;
    private List<GameObject> portals = new List<GameObject>();

    private GameObject tester;
    private bool done = false;

    private int population;
    private float mx, my;

    private bool monsterPresence;

    public bool Visible { get { return dataM.visible; } }

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

        data.datas.Remove(dataM);
        for (int idx = index; idx < data.datas.Count; idx++)
        {
            data.datas[idx].index = idx;
        }
    }

    public GameObject CreatePortal(int direction, bool value)
    {
        while (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();

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
                        type = Random.Range((gameController.StageNumber - 1) * 4, (gameController.StageNumber * 4) - 1);
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
            index = data.datas.Count;

            data.datas.Add(new Data("Map", mapPrfNumber, index, null, null, dir, sel));

            dataM = data.datas[index];

            monsterPresence = false;
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
                    if (!dataM.isClear)
                    {
                        if (!dataM.subStageEntrance)
                        {
                            dataM.visible = true;
                            dataM.subStageEntrance = true;

                            if (index != 0 && index == gameController.SubStageNumber)
                            {
                                CreateStage(true);
                            }
                            else CreateStage(false);
                        }
                        else    // 게임 진행중 
                        {
                            if (!monsterPresence)
                            {
                                dataM.isClear = true;
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

        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        if (dataM == null) dataM = data.datas[index];
        else dataM.SetPosition(transform.position);

        ManageMap();
               
    }
}
