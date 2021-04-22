using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamecontroller : MonoBehaviour
{
    // 게임 오브젝트
    public GameObject[] prf_monster;
    public GameObject[] prf_boss_monster;
    public GameObject[] prf_character;
    public GameObject[] prf_portal;
    // public GameObject[] tile;
    private character player;
    private GameObject[] monsters;
    private GameObject[] portals;

    //
    int population;
    int counter;
    int gx, gy, maxgy;
    int mx, my;

    // 스테이지 번호 1-1
    int stagenumber;
    int substagenumber;

    // 상태
    bool stageentrance;
    bool substageentrance;
    bool monsterpresence;
    bool portalpresence;

    void countportal()  // 포탈 수 세기
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
    }
    void makeportal()
    {
        print(counter + ", " + substagenumber);
        if (counter != substagenumber+1) Instantiate(prf_portal[0], new Vector3(0, -3.8f, 0), Quaternion.identity);  // 기본 포탈 생성
        else Instantiate(prf_portal[1], new Vector3(0, -3.8f, 0), Quaternion.identity); // 보스 포탈 생성
        portalpresence = true;
    }
    void createmonster(float x, float y, int type)    // 몬스터 생성 함수
    {
        Instantiate(prf_monster[type], new Vector3(x, y, 0), Quaternion.identity);
    }
    void createmaptile() // 맵 생성 함수
    {

    }
    void makestage()    // 스테이지 생성 함수
    {
        createmaptile();
        maxgy = 0;// 최대 높이 가져오기
        
        if (substagenumber != 0) // 부스테이지 번호가 0 일 경우 몬스터가 없는 스테이지.
        {
            //population = Random.Range(8, 10);  // 현재 스테이지에 생성할 몬스터의 마릿 수.
            population = 5;
            for (int i = 0; i < population; i++)
            {
                int type;
                type = Random.Range((stagenumber - 1) * 3, stagenumber * 3);
                mx = Random.Range(-5, 5);
                my = Random.Range(-4, maxgy);
                createmonster((float)mx, (float)my, type);    // 랜덤 좌표에 몬스터 생성
            }
        }
    }
    void gonextstage()
    {
        substageentrance = false;
        stagenumber++;
        substagenumber = 0;
    }
    void countmonster() // 몬스터 수 세기
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
    }
    void makebossstage()    // 보스맵 생성
    {
        print("보스전");
        createmaptile();
        createmonster(0, 0, stagenumber);
        Instantiate(prf_portal[0], new Vector3(0, -4.0f, 0), Quaternion.identity);  // 기본 포탈 생성
        countportal();
    }
    void Start()
    {
        // 변수 초기화
        stagenumber = 1;
        substagenumber = 0;

        stageentrance = false;  // 스테이지에 처음 들어왔는가?
        substageentrance = false; // 부스테이지에 처음 들어왔는가?
        monsterpresence = false;// 몬스터가 존재하는가?
        portalpresence = false; // 포탈이 존재하는가?

        // 플레이어 캐릭터 생성
        Instantiate(prf_character[0], new Vector3(1.5f, 0, 0), Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<character>();
    }

    void Update()
    {
        if (!stageentrance) // 처음 스테이지에 입장
        {
            stageentrance = true; // 스테이지에 입장 완료.
            counter = Random.Range(8, 10);  // 부스테이지 갯수 설정
            counter = 2;
        }
        else
        {
            if (!substageentrance) // 처음 부스테이지에 입장
            {
                substagenumber++;
                if (counter == substagenumber) // 보스 포탈을 타고 넘어왔음.
                {
                    makebossstage();
                }
                else makestage();   // 스테이지 생성
                substageentrance = true; // 부스테이지에 입장 완료.
            }
            else    // 게임 진행중
            {
                countmonster();
                if (monsters.Length == 0) // 몬스터가 모두 죽었다면
                {
                    if(!portalpresence) makeportal();

                    countportal();

                    if (portals.Length == 0) // 포탈이 사라졌다면
                    {
                        if(counter == substagenumber)
                        {
                            print("넘어가자");
                            gonextstage();
                        }
                        else substageentrance = false;
                        portalpresence = false;
                    }
                }
            }
        }

     

        
        
    }
}
