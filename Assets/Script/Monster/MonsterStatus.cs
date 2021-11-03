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
    [SerializeField]
    private GameObject effectBone;
    [SerializeField]
    private GameObject dropBone;
    [SerializeField]
    private GameObject monsterIcon;
    [SerializeField]
    private float hpBarYAxis;

    public int monsterPrfNumber;
    public int index = -1;

    private GameController gameController;
    private GameData data;
    private Data dataM;
    private GameObject miniMapMonsterIcon;

    private int coinindexber;
    private GameObject canvas;

    private Image nowHpBar;
    private RectTransform hpBar;

    public Data Data { get { return dataM; } }
    public bool Boss { get { return isBoss; } set { isBoss = value; } }
    public int Dmg { get { return bodyDmg * GameObject.Find("Data").GetComponent<DataController>().GameData.round; } }
    public GameObject Bone { get { return effectBone; } }

    public void DestroyObject()
    {
        Destroy(miniMapMonsterIcon);

        if (Random.Range(0, 101) <= dropRate)
        {
            int cnt = 0;

            if (dropRate >= 100)
            {
                cnt = (dropRate - 100) / 20;
            }

            for (int index = 0; index <= cnt; index++)
            {
                GameObject tmp;
                int idx = Random.Range(dropItemStartindexber, dropItemFinishindexber);
                tmp = Instantiate(gameController.PrefabReturn("Item", idx), dropBone.transform.position, Quaternion.identity);
                tmp.GetComponent<ItemStatus>().itemPrfNumber = idx;
            }
        }

        for(int i=0;i<coinindexber;i++)
        {
            Instantiate(dropCoin[0], transform.position, Quaternion.identity);
        }
        if (isBoss) Instantiate(dropCoin[1], transform.position, Quaternion.identity);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().CalCulateExperience(experience);

        data.datas.Remove(dataM);
        for (int idx = index; idx < data.datas.Count; idx++)
        {
            data.datas[idx].index = idx;
        }

        GetComponent<MonsterAttack>().DestroyAll();
        GetComponent<MonsterMovement>().DestroyAll();
        Destroy(gameObject);
        Destroy(hpBar.gameObject);
    }

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

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    private void Start()
    {
        GameObject tmp;

        if (index == -1)
        {
            int[] arr = new int[2];
            float[] arr2 = new float[7];
            arr[0] = maxHP;
            arr2[0] = defenseRate; arr2[1] = jumpPower; arr2[2] = moveSpeed; arr2[3] = attackSpeed; arr2[4] = bloodAbsorptionRate; arr2[5] = evasionRate; arr2[6] = power;
            index = data.datas.Count;

            data.datas.Add(new Data("Monster", monsterPrfNumber, index, arr, arr2, -1, -1));
            dataM = data.datas[index];

            dataM.isBoss = isBoss;
        }
        else
        {
            dataM = data.datas[index];
        }

        canvas = GameObject.Find("Canvas");

        hpBar = Instantiate(prefabHpBar, canvas.transform).GetComponent<RectTransform>();
        nowHpBar = hpBar.transform.GetChild(0).GetComponent<Image>();

        if (!isBoss) coinindexber = Random.Range(1, 5);
        else coinindexber = 15;

        if (GetComponent<MonsterStatus>().Boss)
        {
            GetComponent<ObjectFlip>().ChangeSize(1.5f);
            hpBar.transform.position = new Vector3(canvas.transform.position.x + 85, canvas.transform.position.y + 100, transform.position.z);
            hpBar.transform.localScale = new Vector3(2.0f, 1.25f, 1.25f);
        }

        miniMapMonsterIcon = Instantiate(monsterIcon, transform.position, Quaternion.identity);
        miniMapMonsterIcon.transform.SetParent(GameObject.Find("Canvas").transform.Find("MiniMap").transform.Find("Background"));
        miniMapMonsterIcon.GetComponent<Icon>().obj = gameObject;
    }

    private void Update()
    {

        if (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();

        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        else
        {
            dataM.SetPosition(transform.position);
            index = dataM.index;
        }

        if (!gameController.IsPause)
        {
            if (!GetComponent<MonsterStatus>().Boss)
            {
                Debug.Log("Ω««‡¡ﬂ");
                Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + hpBarYAxis, 0));
                hpBar.transform.position = _hpBarPos;
            }

            nowHpBar.fillAmount = (float)dataM.nowHP / (float)dataM.maxHP;

            if (dataM.nowHP <= 0)
            {
                DestroyObject();
            }
        }
    }
}
