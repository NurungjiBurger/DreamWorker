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
    public int index = -1;

    private GameController gameController;
    private GameData data;
    private MonsterData dataM;
    private StatData status;

    private int coinindexber;
    private GameObject canvas;

    private Image nowHpBar;
    private RectTransform hpBar;

    public StatData Status { get { return status; } }
    public bool Boss { get { return isBoss; } }
    public int Dmg { get { return bodyDmg; } }

    public void DestroyObject()
    {
        if (Random.Range(0, 101) <= dropRate)
        {
            GameObject tmp;
            int idx = Random.Range(dropItemStartindexber, dropItemFinishindexber);
            tmp = Instantiate(gameController.PrefabReturn("Item", idx), transform.position, Quaternion.identity);
            tmp.GetComponent<ItemStatus>().itemPrfNumber = idx;
        }

        for(int i=0;i<coinindexber;i++)
        {
            Instantiate(dropCoin[0], transform.position, Quaternion.identity);
        }
        if (isBoss) Instantiate(dropCoin[1], transform.position, Quaternion.identity);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().CalCulateExperience(experience);

        data.monsters.Remove(dataM);
        for(int idx = index;idx<data.monsters.Count;idx++)
        {
            data.monsters[idx].index = idx;
        }
        GetComponent<MonsterAttack>().DestroyAll();
        GetComponent<MonsterMovement>().DestroyAll();
        Destroy(gameObject);
        Destroy(hpBar.gameObject);
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    private void Start()
    {
        if (index == -1)
        {
            int[] arr = new int[2];
            float[] arr2 = new float[6];
            arr[0] = maxHP; arr[1] = power;
            arr2[0] = defenseRate; arr2[1] = jumpPower; arr2[2] = moveSpeed; arr2[3] = attackSpeed; arr2[4] = bloodAbsorptionRate; arr2[5] = evasionRate;
            index = data.monsters.Count;

            data.monsters.Add(new MonsterData(monsterPrfNumber, index, arr, arr2));
            dataM = data.monsters[index];

            dataM.isBoss = isBoss;

            status = dataM.status;
        }
        else
        {
            dataM = data.monsters[index];
            status = dataM.status;
        }

        canvas = GameObject.Find("Canvas");

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
                Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.7f, 0));
                hpBar.transform.position = _hpBarPos;
            }

            nowHpBar.fillAmount = (float)status.nowHP / (float)status.maxHP;

            if (status.nowHP <= 0)
            {
                if (GetComponent<MonsterStatus>().isBoss) GetComponent<Animator>().SetTrigger("die");
                else DestroyObject();
            }
        }
    }
}
