using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // 게임 오브젝트
    [SerializeField]
    private GameObject[] prefabMonster;
    [SerializeField]
    private GameObject[] prefabBossMonster;
    [SerializeField]
    private GameObject[] prefabCharacter;
    [SerializeField]
    private GameObject[] prefabPortal;
    [SerializeField]
    private GameObject[] prefabUtility;
    // public GameObject[] tile;
    private GameObject player;

    private GameObject[] monsters;
    private GameObject[] portals;

    RectTransform hpBar;
    [SerializeField]
    private GameObject prefabHpBar;
    [SerializeField]
    private GameObject canvas;
    private Image nowHPBar;
    private Text textHp;

    //
    private int population;
    private int counter;
    private int gx, gy, maxgy;
    private int mx, my;

    // 스테이지 번호 1-1
    private int stageNumber;
    private int subStageNumber;

    // 상태
    private bool isPause;
    private bool goNext;
    private bool isClear;
    private bool stageEntrance;
    private bool subStageEntrance;
    private bool monsterPresence;
    private bool portalPresence;

    public bool IsPause { get { return isPause; } }
    public bool GoNext { get { return goNext; } }

    void ManagePortal()  // 포탈 관리
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
        if (portals.Length == 0) portalPresence = false;
        else portalPresence = true;
    }
    void CreatePortal()
    {
        if (counter - 1 == subStageNumber || counter == 0) Instantiate(prefabPortal[1], new Vector3(0, -2.9f, 0), Quaternion.identity);  // 보스 포탈 생성
        else Instantiate(prefabPortal[0], new Vector3(0, -2.9f, 0), Quaternion.identity); // 기본 포탈 생성
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
        
        if (subStageNumber != 0) // 부스테이지 번호가 0 일 경우 몬스터가 없는 스테이지.
        {
            //population = Random.Range(8, 10);  // 현재 스테이지에 생성할 몬스터의 마릿 수.
            population = 2;
            for (int i = 0; i < population; i++)
            {
                int type;
                type = Random.Range((stageNumber - 1) * 4, (stageNumber * 4) - 1);
                mx = Random.Range(-5, 5);
                my = Random.Range(-4, maxgy);
                type = 0;
                CreateMonster(prefabMonster[type], (float)mx, (float)my);    // 랜덤 좌표에 몬스터 생성
            }
        }
    }
    void GoNextStage()
    {
        stageEntrance = false;
        subStageEntrance = false;
        stageNumber++;
        subStageNumber = 0;
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
        stageNumber = 0;
        subStageNumber = 0;

        goNext = false;
        stageEntrance = false;  // 스테이지에 처음 들어왔는가?
        subStageEntrance = false; // 부스테이지에 처음 들어왔는가?
        monsterPresence = false;// 몬스터가 존재하는가?
        portalPresence = false; // 포탈이 존재하는가?

        // 플레이어 캐릭터 생성
        Instantiate(prefabCharacter[0], new Vector3(1.5f, 0, 0), Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player");

        hpBar = Instantiate(prefabHpBar, canvas.transform).GetComponent<RectTransform>();
        nowHPBar = hpBar.transform.GetChild(0).GetComponent<Image>();


        textHp = nowHPBar.transform.GetChild(0).GetComponent<Text>();

        isClear = false;

        Instantiate(prefabUtility[0], canvas.transform).GetComponent<RectTransform>();
        Instantiate(prefabUtility[1], canvas.transform).GetComponent<RectTransform>();
        Instantiate(prefabUtility[2], canvas.transform).GetComponent<RectTransform>();


        // Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(5, -4.2f, 0));
        // hpBar.transform.position = _hpBarPos;
    }

    void Update()
    {
        // txhp.text = player.Getnowhp().ToString() + "    /    " + player.Getmaxhp().ToString();
        // nowHPbar.fillAmount = (float)player.Getnowhp() / (float)player.Getmaxhp();

        // 1-0 2-0 같은 스테이지인가

        ManageMonster();
        ManagePortal();


        if (GameObject.FindGameObjectWithTag("Pause") == null) isPause = false;
        else isPause = true;

        GameObject.Find("Main Camera").transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y, -10), Quaternion.identity);

        if (stageNumber <= 5) // // 5스테이지가 마지막
        {
            goNext = false;
            if (!stageEntrance) // 처음 스테이지에 입장
            {
                stageEntrance = true;
               // CreateMonster(prefabBossMonster[1], 0, 0);
                if (stageNumber != 0)
                {
                    counter = Random.Range(8, 10);
                    counter = 3;
                }
                else counter = 0;
            }
            else
            {
                if (!subStageEntrance) // 부스테이지 최초 입장
                {
                    subStageEntrance = true;

                    if (subStageNumber != 0 && counter == subStageNumber) // 보스포탈을 타고 넘어왔다면.
                    {
                        CreateBossStage(); // 보스맵 생성
                    }
                    else CreateStage(); // 그렇지 않으면 일반맵 생성
                }
                else    // 게임 진행중 
                {
                    //Debug.Log(stageNumber + " - " + subStageNumber + " / " + counter);
                    if (!isClear)
                    {
                        if (!monsterPresence) // 몬스터가 맵에 존재하지 않는다면
                        {
                            if (!portalPresence) CreatePortal(); // 몬스터가 다 죽었으니 포탈을 만들어
                            else isClear = true;
                        }
                    }
                    else
                    {
                        if (!portalPresence) // 포탈과 상호작용해서 없애버리면
                        {
                            isClear = false;
                            goNext = true;

                            if (counter == subStageNumber) // 보스맵이었으면 다음스테이지로 가야지
                            {
                                Debug.Log("다음스테이지로!");
                                stageEntrance = false;
                                subStageEntrance = false;
                                stageNumber++;
                                subStageNumber = 0;
                            }
                            else
                            {
                                subStageNumber++;
                                subStageEntrance = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
