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

    private GameObject canvas;

    [SerializeField]
    private GameObject[] dropItemList;

    private Image nowHpBar;
    private RectTransform hpBar;

    

    public bool Boss { get { return isBoss; } }
    public int Dmg { get { return bodyDmg; } }

    public void DestroyObject()
    {
        Instantiate(dropItemList[Random.Range(0, dropItemList.Length)], transform.position, Quaternion.identity);
        // 아이템 떨구기

        GetComponent<MonsterAttack>().DestroyAll();
        GetComponent<MonsterMovement>().DestroyAll();
        Destroy(gameObject);
        Destroy(hpBar.gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        canvas = GameObject.Find("Canvas");

        nowHP = maxHP;

        hpBar = Instantiate(prefabHpBar, canvas.transform).GetComponent<RectTransform>();
        nowHpBar = hpBar.transform.GetChild(0).GetComponent<Image>();

        if (GetComponent<MonsterStatus>().Boss)
        {
            // Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(0, 4.3f, 0));
            hpBar.transform.position = new Vector3(transform.position.x + 175.5f, transform.position.y + 177, transform.position.z);
            hpBar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
           // hpBar.transform.position = _hpBarPos;
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
