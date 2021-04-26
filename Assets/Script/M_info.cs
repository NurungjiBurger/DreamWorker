using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_info : MonoBehaviour
{
    // 몬스터의 스탯 & 상태
    private static int size = 2; 

    private string enemyName;
    private int maxHP;
    private int nowHP;
    private int atkDmg; 
    private int atkSpeed; 
    private int recognition_range; 
    private float jumpPower; 
    private float dashPower; 
    private bool attacked = false; 
    private float dir; 
    private int attacktype; 
    private bool atkdone = false; 
    private bool isground = false; 

    private float height = 0.7f;

    private int mvrandom;  

    private float atktime; 
    private float mvtime; 

    private float atkx, atky, atkz; 

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

    public int Getsize()
    {
        return size;
    }

    public float Getatkposition(string pos)
    {
        if (pos == "atkx") return atkx;
        else return atky;
    }

    public void Setatkposition(string pos, float value)
    {
        if (pos == "atkx") atkx = player.transform.position.x + value;
        else atky = player.transform.position.y + value;
    }

    public int Getrandom(string name)
    {
        return mvrandom;
    }

    public void Setrandom(string name, int s, int e)
    {
        if (name == "mvrandom") mvrandom = Random.Range(s, e);
    }

    public bool Getisground()
    {
        return isground;
    }

    public void Setisground(bool value)
    {
        isground = value;
    }

    public bool Getatkdone()
    {
        return atkdone;
    }

    public void Setatkdone(bool value)
    {
        atkdone = value;
    }

    public int Getatktype()
    {
        return attacktype;
    }

    public float Getdir()
    {
        return dir;
    }

    public void Setdir(float value)
    {
        dir = value;
    }

    public bool Getattacked()
    {
        return attacked;
    }

    public void Setattacked(bool value)
    {
        attacked = value;
    }

    public float Getpower()
    {
        if (attacktype == 0) return jumpPower;
        else return dashPower;
        //else if (attacktype == 1) return dashPower;
    }

    public int Getrecogrange()
    {
        return recognition_range;
    }
    public int GetatkDmg()
    {
        return atkDmg;
    }

    public int GetatkSpd()
    {
        return atkSpeed;
    }

    public float Gettime(string time)
    {
        if (time == "atktime") return atktime;
        else return mvtime;
    }
    public void Settime(string name, float time)
    {
        if (name == "atktime") atktime = time;
        else mvtime = time;
    }    

    private void SetEnemyStatus(string _enemyName, int _maxHP, int _atkDmg, int _atkSpd, int _recognition_range, int _attacktype, float _jump_dashPower)
    {
        enemyName = _enemyName;
        maxHP = _maxHP;
        nowHP = _maxHP;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpd;
        recognition_range = _recognition_range;
        attacktype = _attacktype;
        if (attacktype == 0) jumpPower = _jump_dashPower;
        else if (attacktype == 1) dashPower = _jump_dashPower;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Attack_Judgment"))
        {
            if (player.Getattacked())
            {
                animator.SetTrigger("hit");
                nowHP -= player.Getatkdmg();
                player.Setattacked(false);
                if (nowHP <= 0)
                {
                    Destroy(canvas);
                    Destroy(gameObject);
                    Destroy(hpBar.gameObject);
                }
            }
        }
        
        /*
        if (col.CompareTag("Player"))
        {
            print("캐릭터다");
            col2D.isTrigger = true;
        }
        */

        
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = Instantiate(canvas);
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        animator = GetComponent<Animator>();

        if (name.Equals("mushroom(Clone)"))
        {
            SetEnemyStatus("주황버섯", 100, 10, 3, 3, 0, 0.035f);
        }
        else if (name.Equals("blue_snail(Clone)"))
        {
            SetEnemyStatus("파란달팽이", 50, 5, 5, 2, 1, 0.015f);
        }
        else if (name.Equals("pig(Clone)"))
        {
            SetEnemyStatus("돼지", 100, 15, 5, 4, 1, 0.015f);
        }
        else if (name.Equals("slime(Clone)"))
        {
            SetEnemyStatus("슬라임", 100, 10, 3, 3, 0, 0.015f);
        }
        nowHPbar = hpBar.transform.GetChild(0).GetComponent<Image>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();

        mvtime = 0;
        atktime = 0;
    }
    void FixedUpdate()
    {       

    }
    // Update is called once per frame
    void Update()
    {

        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        hpBar.transform.position = _hpBarPos;
        nowHPbar.fillAmount = (float)nowHP / (float)maxHP;

        atktime += Time.deltaTime;
        mvtime += Time.deltaTime;


    }
}
