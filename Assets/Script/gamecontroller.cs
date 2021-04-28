using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamecontroller : MonoBehaviour
{
    // ���� ������Ʈ
    public GameObject[] prf_monster;
    public GameObject[] prf_boss_monster;
    public GameObject[] prf_character;
    public GameObject[] prf_portal;
    // public GameObject[] tile;
    private GameObject player;
    private GameObject[] monsters;
    private GameObject[] portals;

    RectTransform hpBar;
    public GameObject prfHpBar;
    public GameObject canvas;
    Image nowHPbar;
    Text txhp;

    //
    int population;
    int counter;
    int gx, gy, maxgy;
    int mx, my;

    // �������� ��ȣ 1-1
    int stagenumber;
    int substagenumber;

    // ����
    bool stageentrance;
    bool substageentrance;
    bool monsterpresence;
    bool portalpresence;

    void countportal()  // ��Ż �� ����
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
    }
    void makeportal()
    {
        print(counter + ", " + substagenumber);
        if (counter != substagenumber + 1) Instantiate(prf_portal[0], new Vector3(0, -2.9f, 0), Quaternion.identity);  // �⺻ ��Ż ����
        else Instantiate(prf_portal[1], new Vector3(0, -2.9f, 0), Quaternion.identity); // ���� ��Ż ����
        portalpresence = true;
    }
    void createmonster(GameObject prf, float x, float y)    // ���� ���� �Լ�
    {
        Instantiate(prf, new Vector3(0, 0, 0), Quaternion.identity);
    }
    void createmaptile() // �� ���� �Լ�
    {

    }
    void makestage()    // �������� ���� �Լ�
    {
        createmaptile();
        maxgy = 0;// �ִ� ���� ��������
        
        if (substagenumber != 0) // �ν������� ��ȣ�� 0 �� ��� ���Ͱ� ���� ��������.
        {
            //population = Random.Range(8, 10);  // ���� ���������� ������ ������ ���� ��.
            population = 1;
            for (int i = 0; i < population; i++)
            {
                int type;
                type = Random.Range((stagenumber - 1) * 3, stagenumber * 3);
                mx = Random.Range(-5, 5);
                my = Random.Range(-4, maxgy);
                createmonster(prf_monster[type], (float)mx, (float)my);    // ���� ��ǥ�� ���� ����
            }
        }
    }
    void gonextstage()
    {
        substageentrance = false;
        stagenumber++;
        substagenumber = 0;
    }
    void countmonster() // ���� �� ����
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
    }
    void makebossstage()    // ������ ����
    {
        print("������");
        createmaptile();
        createmonster(prf_boss_monster[0], 0, 0);
        Instantiate(prf_portal[0], new Vector3(0, -2.9f, 0), Quaternion.identity);  // �⺻ ��Ż ����
        countportal();
    }
    void Start()
    {
        // ���� �ʱ�ȭ
        stagenumber = 1;
        substagenumber = 0;

        stageentrance = false;  // ���������� ó�� ���Դ°�?
        substageentrance = false; // �ν��������� ó�� ���Դ°�?
        monsterpresence = false;// ���Ͱ� �����ϴ°�?
        portalpresence = false; // ��Ż�� �����ϴ°�?

        // �÷��̾� ĳ���� ����
        Instantiate(prf_character[0], new Vector3(1.5f, 0, 0), Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player");

        canvas = Instantiate(canvas);
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        nowHPbar = hpBar.transform.GetChild(0).GetComponent<Image>();


        txhp = nowHPbar.transform.GetChild(0).GetComponent<Text>();


        // Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(5, -4.2f, 0));
         //hpBar.transform.position = _hpBarPos;
    }

    void Update()
    {
       // txhp.text = player.Getnowhp().ToString() + "    /    " + player.Getmaxhp().ToString();
       // nowHPbar.fillAmount = (float)player.Getnowhp() / (float)player.Getmaxhp();

        if (stagenumber < 2)
        {
            if (!stageentrance) // ó�� ���������� ����
            {
                stageentrance = true; // ���������� ���� �Ϸ�.
                counter = Random.Range(8, 10);  // �ν������� ���� ����
                counter = 2;
            }
            else
            {
                if (!substageentrance) // ó�� �ν��������� ����
                {
                    substagenumber++;
                    if (counter == substagenumber) // ���� ��Ż�� Ÿ�� �Ѿ����.
                    {
                        makebossstage();
                    }
                    else makestage();   // �������� ����
                    substageentrance = true; // �ν��������� ���� �Ϸ�.
                }
                else    // ���� ������
                {
                    countmonster();
                    if (monsters.Length == 0) // ���Ͱ� ��� �׾��ٸ�
                    {
                        if (!portalpresence) makeportal();

                        countportal();

                        if (portals.Length == 0) // ��Ż�� ������ٸ�
                        {
                            if (counter == substagenumber)
                            {
                                print("�Ѿ��");
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
}
