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
    private GameObject[] portals;

    //
    private int population;
    private int counter;
    private int gx, gy, maxgy;
    private int mx, my;

    // 스테이지 번호 1-1
    private int roomNumber;

    // 상태
    private bool isClear;
    private bool subStageEntrance;
    private bool monsterPresence;
    private bool portalPresence;

    void ManagePortal()  // 포탈 관리
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
        if (portals.Length == 0) portalPresence = false;
        else portalPresence = true;
    }
    void CreatePortal()
    {

    }
    void ManageMonster() // 몬스터 관리
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monsters.Length == 0) monsterPresence = false;
        else monsterPresence = true;
    }
    void CreateMonster(GameObject prefab, float x, float y)    // 몬스터 생성 함수
    {
        Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
    void CreateMapTile() // 맵 생성 함수
    {

    }

    void CreateStage()    // 스테이지 생성 함수
    {
        CreateMapTile();
        maxgy = 0;// 최대 높이 가져오기

        if (roomNumber != 0) // 부스테이지 번호가 0 일 경우 몬스터가 없는 스테이지.
        {
            //population = Random.Range(8, 10);  // 현재 스테이지에 생성할 몬스터의 마릿 수.
            population = 2;
            for (int i = 0; i < population; i++)
            {
                int type;
                type = Random.Range((gameController.StageNumber - 1) * 4, (gameController.StageNumber * 4) - 1);
                mx = Random.Range(-5, 5);
                my = Random.Range(-4, maxgy);
                type = 0;
                CreateMonster(prefabMonster[type], (float)mx, (float)my);    // 랜덤 좌표에 몬스터 생성
            }
        }
    }
    void CreateBossStage()    // 보스맵 생성
    {
        Debug.Log("보스전");
        CreateMapTile();
        CreateMonster(prefabBossMonster[0], 0, 0);
        Instantiate(prefabPortal[0], new Vector3(0, -2.9f, 0), Quaternion.identity);  // 기본 포탈 생성
        ManagePortal();
    }

    void Start()
    {
        // 파일을 로드

        // 로드 실패
        // 변수 초기화

        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        for (int i=0;i<gameController.Room.Count;i++)
        {
            if (gameController.Room[i] == transform.gameObject) roomNumber = i;
        }

        subStageEntrance = false; // 부스테이지에 처음 들어왔는가?
        monsterPresence = false;// 몬스터가 존재하는가?
        portalPresence = false; // 포탈이 존재하는가?

        isClear = false;
    }

    void Update()
    {
        ManageMonster();
        ManagePortal();

        
        if (isClear)
        {
            if (!subStageEntrance) // 부스테이지 최초 입장
            {
                subStageEntrance = true;

                if (roomNumber != 0 && roomNumber == gameController.StageNumber) // 보스포탈을 타고 넘어왔다면.
                {
                    CreateBossStage(); // 보스맵 생성
                }
                else CreateStage(); // 그렇지 않으면 일반맵 생성
            }
            else    // 게임 진행중 
            {
                //Debug.Log(stageNumber + " - " + subStageNumber + " / " + counter);
                if (!monsterPresence) // 몬스터가 맵에 존재하지 않는다면
                {
                    if (!portalPresence) CreatePortal(); // 몬스터가 다 죽었으니 포탈을 만들어
                    else isClear = true;
                }
            }
        }
        else
        {
            /*
            if (!portalPresence) // 포탈과 상호작용해서 없애버리면
            {
                

                if (counter == subStageNumber) // 보스맵이었으면 다음스테이지로 가야지
                {
                    Debug.Log("다음스테이지로!");
                    stageEntrance = false;
                    subStageEntrance = false;
                    stageNumber++;
                    subStageNumber = 0;
                }
            }
            */
        }
    }
}
