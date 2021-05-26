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
    private GameObject[] dropItemList;
    [SerializeField]
    private GameObject[] dropCoin;
    [SerializeField]
    private int experience;

    private int coinNumber;
    private GameObject canvas;

    private Image nowHpBar;
    private RectTransform hpBar;

    

    public bool Boss { get { return isBoss; } }
    public int Dmg { get { return bodyDmg; } }

    public void DestroyObject()
    {
        if (Random.Range(0, 101) <= dropRate) Instantiate(dropItemList[Random.Range(0, dropItemList.Length)], transform.position, Quaternion.identity);

        for(int i=0;i<coinNumber;i++)
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

    private void Start()
    {
        canvas = GameObject.Find("Canvas");

        nowHP = maxHP;

        hpBar = Instantiate(prefabHpBar, canvas.transform).GetComponent<RectTransform>();
        nowHpBar = hpBar.transform.GetChild(0).GetComponent<Image>();

        if (!isBoss) coinNumber = Random.Range(1, 5);
        else coinNumber = 15;

        if (GetComponent<MonsterStatus>().Boss)
        {
            hpBar.transform.position = new Vector3(transform.position.x + 200.0f, transform.position.y + 202.5f, transform.position.z);
            hpBar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    // Update is called once per frame
    private void Update()
    {
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
