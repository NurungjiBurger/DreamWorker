using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : Status
{
    [SerializeField]
    private bool isBoss;
    [SerializeField]
    private int bodyDmg;
    [SerializeField]
    private int dropRate;
    [SerializeField]
    private int dropItemStartindexber;
    [SerializeField]
    private int dropItemFinishindexber;
    [SerializeField]
    private float hpBarYAxis;
    [SerializeField]
    private int experience;
    [SerializeField]
    private GameObject effectBone;
    [SerializeField]
    private GameObject dropBone;
    [SerializeField]
    private GameObject monsterIcon;
    [SerializeField]
    private GameObject prefabHpBar;
    [SerializeField]
    private GameObject[] dropCoin;
    [SerializeField]
    private Sprite miniIcon;

    private int coinindexber;
    public int monsterPrfNumber;
    public int index = -1;

    private GameObject room;
    private GameObject miniMapMonsterIcon;
    private GameObject canvas;

    private GameController gameController;
    private GameData data;
    private Data dataM;
    private Image nowHpBar;
    private RectTransform hpBar;

    public bool Boss { get { return isBoss; } set { isBoss = value; } }
    public int Dmg { get { return bodyDmg * GameObject.Find("Data").GetComponent<DataController>().GameData.round; } }
    public GameObject Bone { get { return effectBone; } }
    public Data Data { get { return dataM; } }

    private void OnEnable()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // 몬스터 삭제
    public void DestroyObject()
    {
        // 처치한 몬스터 수 증가
        data.numberOfKilledMonster++;

        // 미니맵 아이콘 삭제
        Destroy(miniMapMonsterIcon);

        // 아이템 드랍
        if (Random.Range(0, 101) <= dropRate)
        {
            int cnt = 0;

            if (dropRate >= 100)
            {
                cnt = (dropRate - 100) / 20;
            }

            // 설정된 갯수 만큼 아이템 드랍
            for (int index = 0; index <= cnt; index++)
            {
                GameObject tmp;
                int idx = Random.Range(dropItemStartindexber, dropItemFinishindexber);
                tmp = Instantiate(gameController.PrefabReturn("Item", idx), dropBone.transform.position, Quaternion.identity);
                tmp.GetComponent<ItemStatus>().itemPrfNumber = idx;
            }
        }

        // 코인 드랍
        for(int i=0;i<coinindexber;i++)
        {
            Instantiate(dropCoin[0], transform.position, Quaternion.identity);
        }
        if (isBoss) Instantiate(dropCoin[1], transform.position, Quaternion.identity);

        // 플레이어 경험치 획득
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().CalCulateExperience(experience);

        // 데이터 삭제
        data.datas.Remove(dataM);
        for (int idx = index; idx < data.datas.Count; idx++)
        {
            data.datas[idx].index = idx;
        }

        GetComponent<MonsterAttack>().DestroyAll();
        GetComponent<MonsterMovement>().DestroyAll();
        Destroy(gameObject);
        Destroy(hpBar.gameObject);

        //Debug.Log("데이터저장");
        //GameObject.Find("Data").GetComponent<DataController>().SaveGameData();
    }

    private void BalanceStat(int value)
    {
        maxHP *= value;
        power *= value;
        defenseRate *= value;
    }

    // 몬스터 보스화
    public void BeTheBossMonster(bool isevent)
    {
        int magnification = 1;

        if (isevent) magnification++;

        GetComponent<ObjectFlip>().ChangeSize(1.5f);
        dropRate = (int)((float)dropRate * 1.5 * magnification);
        maxHP *= 4 * magnification;
        power = (int)((float)power * 1.5) * magnification;
        defenseRate *= 1.5f * magnification;
        jumpPower /= 2 * magnification;
        moveSpeed /= 2 * magnification;
        attackSpeed /= 2 * magnification;
        //bloodAbsorptionRate 
        //evasionRate

    }

    // 플레이어 위치 방 찾기
    private void RoomFind()
    {
        for (int idx = 0; idx < GameObject.Find("GameController").GetComponent<GameController>().Room.Count; idx++)
        {
            if (GameObject.Find("GameController").GetComponent<GameController>().Room[idx].GetComponent<Room>().isPlayer)
            {
                room = GameObject.Find("GameController").GetComponent<GameController>().Room[idx];
                break;
            }
        }
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    private void Start()
    {
        // 데이터 생성 및 할당
        if (index == -1)
        {
            int[] arr = new int[2];
            float[] arr2 = new float[7];
            arr[0] = maxHP;
            arr2[0] = defenseRate; arr2[1] = jumpPower; arr2[2] = moveSpeed; arr2[3] = attackSpeed; arr2[4] = bloodAbsorptionRate; arr2[5] = evasionRate; arr2[6] = power;
            index = data.datas.Count;

            data.datas.Add(new Data("Monster", monsterPrfNumber, index, arr, arr2, -1, -1, false));
            dataM = data.datas[index];

            BalanceStat(data.stageNumber*5);

            dataM.isBoss = isBoss;
        }
        else
        {
            dataM = data.datas[index];
        }

        // 체력바 생성
        canvas = GameObject.Find("Canvas");

        hpBar = Instantiate(prefabHpBar, canvas.transform).GetComponent<RectTransform>();
        hpBar.SetParent(GameObject.Find("Canvas").transform.Find("GameCanvas"));
        nowHpBar = hpBar.transform.GetChild(0).GetComponent<Image>();

        if (!isBoss) coinindexber = Random.Range(1, 5);
        else coinindexber = 15;

        // 보스몬스터 생성 위치 고정
        if (GetComponent<MonsterStatus>().Boss)
        {
            GetComponent<ObjectFlip>().ChangeSize(1.5f);
            hpBarYAxis *= 2.5f;
            hpBar.transform.position = new Vector3(canvas.transform.position.x + 85, canvas.transform.position.y + 100, transform.position.z);
            hpBar.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        }

        // 미니맵 아이콘 생성
        miniMapMonsterIcon = Instantiate(monsterIcon, transform.position, Quaternion.identity);
        miniMapMonsterIcon.transform.SetParent(GameObject.Find("Canvas").transform.Find("MiniMap").transform.Find("Background"));
        miniMapMonsterIcon.GetComponent<Icon>().obj = gameObject;

        RoomFind();
    }

    private void Update()
    {
        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        else
        {
            dataM.SetPosition(transform.position);
            index = dataM.index;
        }

        if (!gameController.IsPause)
        {
            // 체력바는 몬스터 머리 위에 따라다녀야 함
            Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + hpBarYAxis, 0));
            hpBar.transform.position = _hpBarPos;

            hpBar.transform.SetParent(GameObject.Find("Canvas").transform);

            nowHpBar.fillAmount = (float)dataM.nowHP / (float)dataM.maxHP;

            if (dataM.nowHP <= 0)
            {
                DestroyObject();
            }
        }

        if (room != null)
        {
            // 해당 방을 벗어난다면
            if (transform.position.x > room.transform.position.x + 12.0f || transform.position.x < room.transform.position.x - 12.0f || transform.position.y > room.transform.position.y + 7.5f || transform.position.y < room.transform.position.y - 7.5f)
            {
                // 몬스터 위치 복구
                transform.position = room.transform.position;
            }
        }

        if (!gameController.GetComponent<GameController>().ActvieRoom.GetComponent<Room>().IsInHere(this.gameObject)) gameController.GetComponent<GameController>().RefreshObjectPosition(this.gameObject);
    }
}
