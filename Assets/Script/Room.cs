using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{

    private GameData data;
    // 게임 오브젝트

    public int index;

    private GameObject map;
    private GameObject player;
    private GameController gameController;

    private GameObject[] monsters;

    private List<GameObject> portals = new List<GameObject>();

    private GameObject tester;
    private bool done = false;

    //
    private int population;
    private float mx, my;

    // 스테이지 번호 1-1
    private int roomNumber;

    // 상태
    private bool visible = false;
    private bool isClear;
    private bool subStageEntrance;
    private bool monsterPresence;
    private bool portalPresence;

    public bool Visible { get { return visible; } }

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

    public void AllocateRoomNumber(int num)
    {
        roomNumber = num;
    }

    public GameObject CreatePortal(int direction, bool value)
    {
        while (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();
        while (!map) map = gameController.Map[roomNumber];

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
        } // -60 , 0




        portals.Add(portal);

        return portal;
    }

    private void ManagePortals() // 몬스터 관리
    {
        bool value;

        if (GameObject.FindGameObjectsWithTag("Monster").Length == 0) value = true;
        else value = false;

        for (int i = 0; i < portals.Count; i++) portals[i].SetActive(value);
    }

    private GameObject CreateMonster(GameObject prefab, Vector3 position)    // 몬스터 생성 함수
    {
        return Instantiate(prefab, position, Quaternion.identity);
    }

    private void CreateMapTile() // 맵 생성 함수
    {

    }

    private void CreateStage()    // 스테이지 생성 함수
    {
        CreateMapTile();

        if (roomNumber != 0) // 부스테이지 번호가 0 일 경우 몬스터가 없는 스테이지.
        {
            //population = Random.Range(8, 10);  // 현재 스테이지에 생성할 몬스터의 마릿 수.
            population = 2;
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
    private void CreateBossStage()    // 보스맵 생성
    {
        Debug.Log("보스전");
        CreateMapTile();
        CreateMonster(gameController.PrefabReturn("BossMonster", 0), new Vector3(transform.position.x, transform.position.y, transform.position.z)).GetComponent<MonsterStatus>().monsterPrfNumber = 0;
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    void Start()
    {
        // 파일을 로드

        // 로드 실패
        // 변수 초기화

        subStageEntrance = false; // 부스테이지에 처음 들어왔는가?
        monsterPresence = false;// 몬스터가 존재하는가?
        portalPresence = false; // 포탈이 존재하는가?

        isClear = false;
    }

    private void FixedUpdate()
    {
        // 아래 데이터들 저장 필요
        if (player && map)
        {
            if (player.transform.position.x <= transform.position.x + 11.0f && player.transform.position.x >= transform.position.x - 11.0f)
            {
                if (player.transform.position.y <= transform.position.y + 7.5f && player.transform.position.y >= transform.position.y - 7.5f)
                {
                    if (!isClear)
                    {
                        if (!subStageEntrance) // 부스테이지 최초 입장
                        {
                            visible = true;
                            subStageEntrance = true;

                            if (roomNumber != 0 && roomNumber == gameController.SubStageNumber) // 보스포탈을 타고 넘어왔다면.
                            {
                                CreateBossStage(); // 보스맵 생성
                            }
                            else CreateStage(); // 그렇지 않으면 일반맵 생성
                        }
                        else    // 게임 진행중 
                        {
                            if (!monsterPresence) // 몬스터가 맵에 존재하지 않는다면
                            {
                                isClear = true;
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
            if (!map) map = gameController.Map[roomNumber];
        }
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        else data.maps[index].SetPosition(transform.position);

        ManagePortals();
               
    }
}
