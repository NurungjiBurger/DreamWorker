using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : Status
{
    [SerializeField]
    private GameObject prefabHpBar;
    [SerializeField]
    private GameObject canvas;

    private Image nowHpBar;
    private RectTransform hpBar;

    [SerializeField]
    private bool isBoss;
    [SerializeField]
    private int bodyDmg;

    public bool Boss { get { return isBoss; } }
    public int Dmg { get { return bodyDmg; } }

    // Start is called before the first frame update
    void Start()
    {
        nowHP = maxHP;

        canvas = Instantiate(canvas);
        hpBar = Instantiate(prefabHpBar, canvas.transform).GetComponent<RectTransform>();
        nowHpBar = hpBar.transform.GetChild(0).GetComponent<Image>();

        if (GetComponent<MonsterStatus>().Boss)
        {
            Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(0, 4.3f, 0));
            hpBar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            hpBar.transform.position = _hpBarPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<MonsterStatus>().Boss)
        {
            Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.7f, 0));
            hpBar.transform.position = _hpBarPos;
        }

        nowHpBar.fillAmount = (float)GetComponent<MonsterStatus>().NowHP / (float)GetComponent<MonsterStatus>().MaxHP;

        if (GetComponent<MonsterStatus>().NowHP <= 0)
        {
            Destroy(canvas);
            Destroy(gameObject);
            Destroy(hpBar.gameObject);
        }
    }
}
