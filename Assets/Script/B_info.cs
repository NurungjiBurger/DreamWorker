using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_info : MonoBehaviour
{
    // 몬스터의 스탯 & 상태
    private static int size = 3;
    private string enemyName;
    private int maxHP;
    private int nowHP;
    private int atkDmg;
    private int atkSpeed;
    private int recognition_range;
    private int attacktype;

    private bool attacked = false;
    private float dir;

    private bool atkdone = false;

    private int atkrandom;

    private float atktime;


    // 오브젝트
    [SerializeField]
    private GameObject prfHpBar;
    [SerializeField]
    private GameObject canvas;

    public Animator animator;

    private RectTransform hpBar;
    private P_info player;
    private Image nowHPbar;

    private BoxCollider2D col2D;
    private Rigidbody2D rigid2D;


    private void SetEnemyStatus(string _enemyName, int _maxHP, int _atkDmg, int _atkSpd, int _recognition_range, int _attacktype)
    {
        enemyName = _enemyName;
        maxHP = _maxHP;
        nowHP = _maxHP;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpd;
        recognition_range = _recognition_range;
        attacktype = _attacktype;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = Instantiate(canvas);
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        animator = GetComponent<Animator>();

        if (name.Equals("balrog(Clone"))
        {
            SetEnemyStatus("발록", 5000, 0, 10, 4, 0);
        }

        nowHPbar = hpBar.transform.GetChild(0).GetComponent<Image>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();

        atktime = 0;

        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(0, 3, 0));
        hpBar.transform.position = _hpBarPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
