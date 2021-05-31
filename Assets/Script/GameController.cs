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

    private List<GameObject> room = new List<GameObject>();

    public bool IsPause { get { return isPause; } }
    public bool GoNext { get { return goNext; } }
    public int StageNumber { get { return stageNumber; } }
    public int SubStageNumber { get { return subStageNumber; } }
    public List<GameObject> Room { get { return room; } }

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
            Room.Add(Instantiate(prefabRoom, new Vector3(0, 0, 0), Quaternion.identity));
            Room[0].GetComponent<Room>().AllocateRoomNumber(0);
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

            for (int i=0;i<Room.Count;i++)
            {
                if (Room[i].transform.position == position) create = false;
            }

            if (create)
            {
                Room.Add(Instantiate(prefabRoom, position, Quaternion.identity));
                Room[Room.Count - 1].GetComponent<Room>().AllocateRoomNumber(Room.Count - 1);

                GameObject first, second;
                bool value;

                if (Room.Count - 1 < subStageNumber) value = false;
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

    void Update()
    {
        
        textHp.text = player.GetComponent<PlayerStatus>().NowHP.ToString() + "    /    " + player.GetComponent<PlayerStatus>().MaxHP.ToString();
        // nowHPbar.fillAmount = (float)player.Getnowhp() / (float)player.Getmaxhp();


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
                //subStageNumber = 3;

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
