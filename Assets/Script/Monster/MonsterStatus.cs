using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : Status
{
    [SerializeField]
    private GameObject prefabHpBar;
    [SerializeField]
    private Sprite miniIcon;
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
    private GameObject[] dropCoin;
    [SerializeField]
    private int experience;

    public int monsterPrfNumber;
    public bool isFirst = false;
    public int index;

    private GameData data;

    private int coinindexber;
    private GameObject canvas;

    private Image nowHpBar;
    private RectTransform hpBar;

    private GameObject[] dropItemList;

    public bool Boss { get { return isBoss; } }
    public int Dmg { get { return bodyDmg; } }

    public void DestroyObject()
    {
        if (Random.Range(0, 101) <= dropRate)
        {
            GameObject tmp;
            int num = Random.Range(dropItemStartindexber, dropItemFinishindexber);
            tmp = Instantiate(dropItemList[index], transform.position, Quaternion.identity);
            tmp.GetComponent<ItemStatus>().itemPrfNumber = num;
            tmp.GetComponent<ItemStatus>().isFirst = true;
        }

        for(int i=0;i<coinindexber;i++)
        {
            Instantiate(dropCoin[0], transform.position, Quaternion.identity);
        }
        if (isBoss) Instantiate(dropCoin[1], transform.position, Quaternion.identity);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().CalCulateExperience(experience);
        // 경험치 주기

        GetComponent<MonsterAttack>().DestroyAll();
        GetComponent<MonsterMovement>().DestroyAll();
        Destroy(gameObject);
        Destroy(hpBar.gameObject);
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        if (isFirst)
        {
            index = data.monsters.Count;
            data.monsters.Add(new MonsterData(monsterPrfNumber, index));
            data.monsters[index].isBoss = isBoss;
        }
        else
        {
            
            // 몬스터 좌표 조정
        }
    }

    private void Start()
    {
        canvas = GameObject.Find("Canvas");

        nowHP = maxHP;

        hpBar = Instantiate(prefabHpBar, canvas.transform).GetComponent<RectTransform>();
        nowHpBar = hpBar.transform.GetChild(0).GetComponent<Image>();

        if (!isBoss) coinindexber = Random.Range(1, 5);
        else coinindexber = 15;

        if (GetComponent<MonsterStatus>().Boss)
        {
            hpBar.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y + 85, transform.position.z);
            hpBar.transform.localScale = new Vector3(2.0f, 1.5f, 1.5f);
        }
    }

    private void Update()
    {
        if (dropItemList == null) dropItemList = GameObject.Find("GameController").GetComponent<GameController>().DropItem;

        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        else data.monsters[index].SetPosition(transform.position);

        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (!GetComponent<MonsterStatus>().Boss)
            {
                Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.7f, 0));
                hpBar.transform.position = _hpBarPos;
            }

            nowHpBar.fillAmount = (float)GetComponent<MonsterStatus>().NowHP / (float)GetComponent<MonsterStatus>().MaxHP;

            if (GetComponent<MonsterStatus>().NowHP <= 0)
            {
                if (GetComponent<MonsterStatus>().isBoss) GetComponent<Animator>().SetTrigger("die");
                else DestroyObject();
            }
        }
    }
}
