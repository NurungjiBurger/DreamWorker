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

    private GameObject map;
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
        while (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();
        while (!map) map = gameController.Map[roomNumber];

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

        /*
        Vector3 origin = portal.transform.position;
        float distance = Vector3.Distance(origin, map.GetComponent<Map>().SafePosition[0]);
        portal.transform.position = map.GetComponent<Map>().SafePosition[0];

        for(int i=0;i<map.GetComponent<Map>().SafePosition.Count;i++)
        {
            if (distance > Vector3.Distance(origin, map.GetComponent<Map>().SafePosition[i]))
            {
                distance = Vector3.Distance(origin, map.GetComponent<Map>().SafePosition[i]);
                portal.transform.position = map.GetComponent<Map>().SafePosition[i];
            }
        }
        */
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

    private void CreateMonster(GameObject prefab, Vector3 position)    // ���� ���� �Լ�
    {
        Instantiate(prefab, position, Quaternion.identity);
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
                Vector3 safePosition;
                type = Random.Range((gameController.StageNumber - 1) * 4, (gameController.StageNumber * 4) - 1);
                safePosition = map.GetComponent<Map>().SafePosition[Random.Range(0, map.GetComponent<Map>().SafePosition.Count)];
                type = 0;
                CreateMonster(prefabMonster[type], safePosition);    // ���� ��ǥ�� ���� ����
            }
        }
    }
    private void CreateBossStage()    // ������ ����
    {
        Debug.Log("������");
        CreateMapTile();
        CreateMonster(prefabBossMonster[0], new Vector3(transform.position.x, transform.position.y, transform.position.z));
    }

    void Start()
    {
        // ������ �ε�

        // �ε� ����
        // ���� �ʱ�ȭ

        subStageEntrance = false; // �ν��������� ó�� ���Դ°�?
        monsterPresence = false;// ���Ͱ� �����ϴ°�?
        portalPresence = false; // ��Ż�� �����ϴ°�?

        isClear = false;
    }

    private void FixedUpdate()
    {
        if (player && map)
        {
            if (map.GetComponent<Map>().SafePosition.Count != 0)
            {
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
    }

    private void Update()
    {
        if (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();
        else
        {
            if (!map) map = gameController.Map[roomNumber];
        }
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        ManagePortals();
               
    }
}
