using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{
    // 게임 오브젝트
    [SerializeField]
    private GameObject[] prefabMonster;
    [SerializeField]
    private GameObject[] prefabBossMonster;
    [SerializeField]
    private GameObject[] prefabPortal;
    // public GameObject[] tile;
    private GameObject player;
    private GameController gameController;

    private GameObject[] monsters;

    private List<GameObject> portals = new List<GameObject>();

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
        GameObject portal;

        if (value) portal = Instantiate(prefabPortal[1]);
        else portal = Instantiate(prefabPortal[0]);

        switch(direction)
        {
            case 0: // 동
                portal.transform.position = new Vector3(transform.position.x + 10.5f, transform.position.y , transform.position.z);
                break;
            case 1: // 서
                portal.transform.position = new Vector3(transform.position.x - 10.5f, transform.position.y + 1.0f , transform.position.z);
                break;
            case 2: // 남
                portal.transform.position = new Vector3(transform.position.x, transform.position.y - 5.5f, transform.position.z);
                break;
            case 3: // 북
                portal.transform.position = new Vector3(transform.position.x, transform.position.y + 7.6f, transform.position.z);
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

    private void CreateMonster(GameObject prefab, float x, float y)    // 몬스터 생성 함수
    {
        Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
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
                type = Random.Range((gameController.StageNumber - 1) * 4, (gameController.StageNumber * 4) - 1);
                mx = Random.Range(transform.position.x - 5, transform.position.x + 5);
                my = Random.Range(transform.position.y - 4, transform.position.y + 4);
                type = 0;
                Debug.Log(mx + "  " + my);
                CreateMonster(prefabMonster[type], mx, my);    // 랜덤 좌표에 몬스터 생성
            }
        }
    }
    private void CreateBossStage()    // 보스맵 생성
    {
        Debug.Log("보스전");
        CreateMapTile();
        CreateMonster(prefabBossMonster[0], transform.position.x, transform.position.y);
    }

    void Start()
    {
        // 파일을 로드

        // 로드 실패
        // 변수 초기화

        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        subStageEntrance = false; // 부스테이지에 처음 들어왔는가?
        monsterPresence = false;// 몬스터가 존재하는가?
        portalPresence = false; // 포탈이 존재하는가?

        isClear = false;
    }

    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        ManagePortals();
        
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
                else
                {

                }
            }
        }
    }
}
