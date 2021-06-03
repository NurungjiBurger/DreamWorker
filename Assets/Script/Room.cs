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

    private List<GameObject> portals = new List<GameObject>();

    //
    private int population;
    private float mx, my;

    // �������� ��ȣ 1-1
    private int roomNumber;

    // ����
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
            case 0: // ��
                portal.transform.position = new Vector3(transform.position.x + 10.5f, transform.position.y , transform.position.z);
                break;
            case 1: // ��
                portal.transform.position = new Vector3(transform.position.x - 10.5f, transform.position.y + 1.0f , transform.position.z);
                break;
            case 2: // ��
                portal.transform.position = new Vector3(transform.position.x, transform.position.y - 5.5f, transform.position.z);
                break;
            case 3: // ��
                portal.transform.position = new Vector3(transform.position.x, transform.position.y + 7.6f, transform.position.z);
                break;
            default:
                break;
        } // -60 , 0

        portals.Add(portal);

        return portal;
    }

    private void ManagePortals() // ���� ����
    {
        bool value;

        if (GameObject.FindGameObjectsWithTag("Monster").Length == 0) value = true;
        else value = false;

        for (int i = 0; i < portals.Count; i++) portals[i].SetActive(value);
    }

    private void CreateMonster(GameObject prefab, float x, float y)    // ���� ���� �Լ�
    {
        Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    private void CreateMapTile() // �� ���� �Լ�
    {

    }

    private void CreateStage()    // �������� ���� �Լ�
    {
        CreateMapTile();

        if (roomNumber != 0) // �ν������� ��ȣ�� 0 �� ��� ���Ͱ� ���� ��������.
        {
            //population = Random.Range(8, 10);  // ���� ���������� ������ ������ ���� ��.
            population = 2;
            for (int i = 0; i < population; i++)
            {
                int type;
                type = Random.Range((gameController.StageNumber - 1) * 4, (gameController.StageNumber * 4) - 1);
                mx = Random.Range(transform.position.x - 5, transform.position.x + 5);
                my = Random.Range(transform.position.y - 4, transform.position.y + 4);
                type = 0;
                Debug.Log(mx + "  " + my);
                CreateMonster(prefabMonster[type], mx, my);    // ���� ��ǥ�� ���� ����
            }
        }
    }
    private void CreateBossStage()    // ������ ����
    {
        Debug.Log("������");
        CreateMapTile();
        CreateMonster(prefabBossMonster[0], transform.position.x, transform.position.y);
    }

    void Start()
    {
        // ������ �ε�

        // �ε� ����
        // ���� �ʱ�ȭ

        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        subStageEntrance = false; // �ν��������� ó�� ���Դ°�?
        monsterPresence = false;// ���Ͱ� �����ϴ°�?
        portalPresence = false; // ��Ż�� �����ϴ°�?

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
                    if (!subStageEntrance) // �ν������� ���� ����
                    {
                        visible = true;
                        subStageEntrance = true;

                        if (roomNumber != 0 && roomNumber == gameController.SubStageNumber) // ������Ż�� Ÿ�� �Ѿ�Դٸ�.
                        {
                            CreateBossStage(); // ������ ����
                        }
                        else CreateStage(); // �׷��� ������ �Ϲݸ� ����
                    }
                    else    // ���� ������ 
                    {
                        if (!monsterPresence) // ���Ͱ� �ʿ� �������� �ʴ´ٸ�
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
