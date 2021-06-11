using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabCharacter;
    [SerializeField]
    private GameObject prefabRoom;
    [SerializeField]
    private GameObject[] prefabMapDesign;
    private GameObject player;

    private bool isPause = false;
    private bool goNext;

    private int pastSelectDirection;

    private int subStageNumber;
    private int stageNumber;
    private bool stageEntrance;

    RectTransform hpBar;
    private Image nowHPBar;
    private TextMeshProUGUI textHp;

    private List<GameObject> map = new List<GameObject>();
    private List<GameObject> room = new List<GameObject>();

    public bool IsPause { get { return isPause; } }
    public bool GoNext { get { return goNext; } }
    public int StageNumber { get { return stageNumber; } }
    public int SubStageNumber { get { return subStageNumber; } }
    public List<GameObject> Room { get { return room; } }
    public List<GameObject> Map { get { return map; } }

    private void DestroyAll()
    {
        GameObject tmp;
        int cnt = Room.Count;
        for(int i=0;i<cnt;i++)
        {
            tmp = Room[0];
            Room.Remove(Room[0]);
            tmp.GetComponent<Room>().DestoryAll();
            Destroy(tmp);
        }
    }

    private void CreateRoom(int cnt)
    {
        if (cnt > 0)
        {
            CreateRoom(cnt - 1);
        }
        else
        {
            pastSelectDirection = 0;
            room.Add(Instantiate(prefabRoom, new Vector3(0, 0, 0), Quaternion.identity));
            room[0].GetComponent<Room>().AllocateRoomNumber(0);
            map.Add(Instantiate(prefabMapDesign[Random.Range(0,prefabMapDesign.Length)], new Vector3(0, 0, 0), Quaternion.identity));
            map[0].transform.SetParent(GameObject.Find("Grid").transform);
            return;
        }

        bool complete = false;
        bool create = true;
        GameObject selectRoom;
        int direction = Random.Range(0, 4);
        if (Random.Range(0, 2) == 0) direction = pastSelectDirection;

        while (!complete)
        {
            create = true;
            int number = Random.Range(0, Room.Count);
            selectRoom = Room[number];
            Vector3 position = selectRoom.transform.position;

            switch (direction)
            {
                case 0: // 동
                    position.x += 30;
                    break;
                case 1: // 서
                    position.x -= 30;
                    break;
                case 2: // 남
                    position.y -= 24;
                    break;
                case 3: // 북
                    position.y += 24;
                    break;
                default:
                    break;
            }

            for (int i=0;i<room.Count;i++)
            {
                if (room[i].transform.position == position) create = false;
            }

            if (create)
            {
                room.Add(Instantiate(prefabRoom, position, Quaternion.identity));
                room[room.Count - 1].GetComponent<Room>().AllocateRoomNumber(Room.Count - 1);
                if (cnt == SubStageNumber) map.Add(Instantiate(prefabMapDesign[prefabMapDesign.Length-1], position, Quaternion.identity));
                else map.Add(Instantiate(prefabMapDesign[Random.Range(0, prefabMapDesign.Length)], position, Quaternion.identity));
                map[map.Count - 1].transform.SetParent(GameObject.Find("Grid").transform);

                GameObject first, second;
                bool value;

                if (room.Count - 1 < subStageNumber) value = false;
                else value = true;

                first = selectRoom.GetComponent<Room>().CreatePortal(direction, value);
                if (direction % 2 == 0) direction += 1;
                else direction -= 1;
                second = Room[Room.Count - 1].GetComponent<Room>().CreatePortal(direction, value);

                first.GetComponent<Portal>().PositionSave(second.GetComponent<Portal>());
                second.GetComponent<Portal>().PositionSave(first.GetComponent<Portal>());

                complete = true;
            }
        }
    }

    private bool CheckRoom(string str)
    {
        return true;
    }

    void Start()
    {
        stageNumber = 0;
        subStageNumber = 0;

        stageEntrance = false;  // 스테이지에 처음 들어왔는가?
        goNext = false;

        // 플레이어 캐릭터 생성
        Instantiate(prefabCharacter[0], new Vector3(0, 0, 0), Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player");

        hpBar = GameObject.Find("Canvas").transform.Find("PlayerHPBar").GetComponent<RectTransform>();
        nowHPBar = hpBar.transform.GetChild(0).GetComponent<Image>();


        textHp = nowHPBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        textHp.text = player.GetComponent<PlayerStatus>().NowHP.ToString() + "    /    " + player.GetComponent<PlayerStatus>().MaxHP.ToString();
        // nowHPbar.fillAmount = (float)player.Getnowhp() / (float)player.Getmaxhp();


        Debug.Log("게임");

        if (GameObject.FindGameObjectWithTag("Pause") == null) isPause = false;
        else isPause = true;

        GameObject.Find("Main Camera").transform.SetPositionAndRotation(new Vector3(player.transform.position.x, player.transform.position.y, -10), Quaternion.identity);

        if (stageNumber <= 5) // // 5스테이지가 마지막
        {
            goNext = false;
            if (!stageEntrance) // 처음 스테이지에 입장
            {
                stageEntrance = true;

                subStageNumber = Random.Range(10, 16);
                //subStageNumber = 0;

                CreateRoom(subStageNumber);
            }
            else
            {
                CheckRoom("visible");
            }
        }
        /*
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

            }
            }
            */

    }
}
