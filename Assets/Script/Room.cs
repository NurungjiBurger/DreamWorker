using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{
    // ���� ������Ʈ
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

    // �������� ��ȣ 1-1
    private int roomNumber;

    // ����
    private bool isClear;
    private bool subStageEntrance;
    private bool monsterPresence;
    private bool portalPresence;

    void ManagePortal()  // ��Ż ����
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
        if (portals.Length == 0) portalPresence = false;
        else portalPresence = true;
    }
    void CreatePortal()
    {

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

        if (roomNumber != 0) // �ν������� ��ȣ�� 0 �� ��� ���Ͱ� ���� ��������.
        {
            //population = Random.Range(8, 10);  // ���� ���������� ������ ������ ���� ��.
            population = 2;
            for (int i = 0; i < population; i++)
            {
                int type;
                type = Random.Range((gameController.StageNumber - 1) * 4, (gameController.StageNumber * 4) - 1);
                mx = Random.Range(-5, 5);
                my = Random.Range(-4, maxgy);
                type = 0;
                CreateMonster(prefabMonster[type], (float)mx, (float)my);    // ���� ��ǥ�� ���� ����
            }
        }
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

        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        for (int i=0;i<gameController.Room.Count;i++)
        {
            if (gameController.Room[i] == transform.gameObject) roomNumber = i;
        }

        subStageEntrance = false; // �ν��������� ó�� ���Դ°�?
        monsterPresence = false;// ���Ͱ� �����ϴ°�?
        portalPresence = false; // ��Ż�� �����ϴ°�?

        isClear = false;
    }

    void Update()
    {
        ManageMonster();
        ManagePortal();

        
        if (isClear)
        {
            if (!subStageEntrance) // �ν������� ���� ����
            {
                subStageEntrance = true;

                if (roomNumber != 0 && roomNumber == gameController.StageNumber) // ������Ż�� Ÿ�� �Ѿ�Դٸ�.
                {
                    CreateBossStage(); // ������ ����
                }
                else CreateStage(); // �׷��� ������ �Ϲݸ� ����
            }
            else    // ���� ������ 
            {
                //Debug.Log(stageNumber + " - " + subStageNumber + " / " + counter);
                if (!monsterPresence) // ���Ͱ� �ʿ� �������� �ʴ´ٸ�
                {
                    if (!portalPresence) CreatePortal(); // ���Ͱ� �� �׾����� ��Ż�� �����
                    else isClear = true;
                }
            }
        }
        else
        {
            /*
            if (!portalPresence) // ��Ż�� ��ȣ�ۿ��ؼ� ���ֹ�����
            {
                

                if (counter == subStageNumber) // �������̾����� �������������� ������
                {
                    Debug.Log("��������������!");
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
