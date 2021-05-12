using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // ���� ������Ʈ
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

    // �������� ��ȣ 1-1
    private int stageNumber;
    private int subStageNumber;

    // ����
    private bool isPause;
    private bool goNext;
    private bool isClear;
    private bool stageEntrance;
    private bool subStageEntrance;
    private bool monsterPresence;
    private bool portalPresence;

    public bool IsPause { get { return isPause; } }
    public bool GoNext { get { return goNext; } }

    void ManagePortal()  // ��Ż ����
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
        if (portals.Length == 0) portalPresence = false;
        else portalPresence = true;
    }
    void CreatePortal()
    {
        if (counter - 1 == subStageNumber || counter == 0) Instantiate(prefabPortal[1], new Vector3(0, -2.9f, 0), Quaternion.identity);  // ���� ��Ż ����
        else Instantiate(prefabPortal[0], new Vector3(0, -2.9f, 0), Quaternion.identity); // �⺻ ��Ż ����
    }
    void ManageMonster() // ���� ����
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monsters.Length == 0) monsterPresence = false;
        else monsterPresence = true;
    }
    void CreateMonster(GameObject prefab, float x, float y)    // ���� ���� �Լ�
    {
        Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
    void CreateMapTile() // �� ���� �Լ�
    {

    }
    void CreateStage()    // �������� ���� �Լ�
    {
        CreateMapTile();
        maxgy = 0;// �ִ� ���� ��������
        
        if (subStageNumber != 0) // �ν������� ��ȣ�� 0 �� ��� ���Ͱ� ���� ��������.
        {
            //population = Random.Range(8, 10);  // ���� ���������� ������ ������ ���� ��.
            population = 2;
            for (int i = 0; i < population; i++)
            {
                int type;
                type = Random.Range((stageNumber - 1) * 4, (stageNumber * 4) - 1);
                mx = Random.Range(-5, 5);
                my = Random.Range(-4, maxgy);
                type = 0;
                CreateMonster(prefabMonster[type], (float)mx, (float)my);    // ���� ��ǥ�� ���� ����
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
    void CreateBossStage()    // ������ ����
    {
        Debug.Log("������");
        CreateMapTile();
        CreateMonster(prefabBossMonster[0], 0, 0);
        Instantiate(prefabPortal[0], new Vector3(0, -2.9f, 0), Quaternion.identity);  // �⺻ ��Ż ����
        ManagePortal();
    }
    void Start()
    {
        // ������ �ε�

        // �ε� ����
        // ���� �ʱ�ȭ
        stageNumber = 0;
        subStageNumber = 0;

        goNext = false;
        stageEntrance = false;  // ���������� ó�� ���Դ°�?
        subStageEntrance = false; // �ν��������� ó�� ���Դ°�?
        monsterPresence = false;// ���Ͱ� �����ϴ°�?
        portalPresence = false; // ��Ż�� �����ϴ°�?

        // �÷��̾� ĳ���� ����
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

        // 1-0 2-0 ���� ���������ΰ�

        ManageMonster();
        ManagePortal();


        if (GameObject.FindGameObjectWithTag("Pause") == null) isPause = false;
        else isPause = true;

        GameObject.Find("Main Camera").transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y, -10), Quaternion.identity);

        if (stageNumber <= 5) // // 5���������� ������
        {
            goNext = false;
            if (!stageEntrance) // ó�� ���������� ����
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
                if (!subStageEntrance) // �ν������� ���� ����
                {
                    subStageEntrance = true;

                    if (subStageNumber != 0 && counter == subStageNumber) // ������Ż�� Ÿ�� �Ѿ�Դٸ�.
                    {
                        CreateBossStage(); // ������ ����
                    }
                    else CreateStage(); // �׷��� ������ �Ϲݸ� ����
                }
                else    // ���� ������ 
                {
                    //Debug.Log(stageNumber + " - " + subStageNumber + " / " + counter);
                    if (!isClear)
                    {
                        if (!monsterPresence) // ���Ͱ� �ʿ� �������� �ʴ´ٸ�
                        {
                            if (!portalPresence) CreatePortal(); // ���Ͱ� �� �׾����� ��Ż�� �����
                            else isClear = true;
                        }
                    }
                    else
                    {
                        if (!portalPresence) // ��Ż�� ��ȣ�ۿ��ؼ� ���ֹ�����
                        {
                            isClear = false;
                            goNext = true;

                            if (counter == subStageNumber) // �������̾����� �������������� ������
                            {
                                Debug.Log("��������������!");
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
