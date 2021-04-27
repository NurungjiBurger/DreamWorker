using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_info : MonoBehaviour
{
    private static int size = 2;    

    private bool iswall = false; 
    private bool isground = false; 
    private bool inputright = false; 
    private bool inputleft = false; 
    private bool inputjump = false; 
    private bool inputdash = false; 
    private float movespeed; 
    private float jumppower; 

    private float dashtime; 
    private bool usedash = false;

    private float hittime;
    private float hitdelay;
    private bool hit = false;

    private bool portal = false; 
    private bool inputup = false;
    private bool gonext = false; 
    private float atktime; 
    private bool useattack = false;
    private float atkSpeed = 0.5f; // 캐릭터의 공격 속도 ( 공격 사이의 간격시간 )

    // 캐릭터 스탯 & 상태
    private int maxHP; 
    private int nowHP; 
    private int cnt;
    private int jumpcnt;

    private int dir;

    private int atkdmg; 
    private bool attacked = false;  // 캐릭터가 공격중인가?
    private int attack_range; 



    // 오브젝트
    public Animator animator;

    // 캐릭터 위치 및 사출기 위치

    private Rigidbody2D rigid2D;
    private BoxCollider2D col2D;

    private float plax, play, plaz;

    public float Getp_position(string name)
    {
        if (name == "plax") return plax;
        else if (name == "play") return play;
        else if (name == "plaz") return plaz;
        else return 0;
    }

    public void Setp_position()
    {
        plax = this.transform.position.x;
        play = this.transform.position.y;
        plaz = this.transform.position.z;
    }

    public float Getatkspd()
    {
        return atkSpeed;
    }

    public int Getatkrange()
    {
        return attack_range;
    }

    public int Getatkdmg()
    {
        return atkdmg;
    }

    public int Getdir()
    {
        return dir;
    }

    public void Setdir(int value)
    {
        dir = value;
    }

    public int Getnowhp()
    {
        return nowHP;
    }

    public void Calnowhp(int value)
    {
        nowHP += value;
    }

    public int Getmaxhp()
    {
        return maxHP;
    }

    public float Getmvspd()
    {
        return movespeed;
    }

    public float Getjumppower()
    {
        return jumppower;
    }

    public bool Getattacked()
    {
        return attacked;
    }

    public void Setattacked(bool value)
    {
        attacked = value;
    }

    public bool Getgonext()
    {
        return gonext;
    }

    public void Setgonext(bool value)
    {
        gonext = value;
    }

    public bool Getportal()
    {
        return portal;
    }

    public void Setportal(bool value)
    {
        portal = value;
    }

    public bool Getuseattack()
    {
        return useattack;
    }

    public void Setuseattack(bool value)
    {
        useattack = value;
    }

    public bool Gethit()
    {
        return hit;
    }

    public void Sethit(bool value)
    {
        hit = value;
    }

    public bool Getusedash()
    {
        return usedash;
    }

    public void Setusedash(bool value)
    {
        usedash = value;
    }

    public bool Getinputdash()
    {
        return inputdash;
    }

    public void Setinputdash(bool value)
    {
        inputdash = value;
    }

    public bool Getinputjump()
    {
        return inputjump;
    }

    public void Setinputjump(bool value)
    {
        inputjump = value;
    }

    public bool Getinputup()
    {
        return inputup;
    }

    public void Setinputup(bool value)
    {
        inputup = value;
    }

    public bool Getinputleft()
    {
        return inputleft;
    }

    public void Setinputleft(bool value)
    {
        inputleft = value;
    }

    public bool Getinputright()
    {
        return inputright;
    }

    public void Setinputright(bool value)
    {
        inputright = value;
    }

    public bool Getisground()
    {
        return isground;
    }

    public void Setisground(bool value)
    {
        isground = value;
    }

    public bool Getiswall()
    {
        return iswall;
    }

    public void Setiswall(bool value)
    {
        iswall = value;
    }
   
    public float Gettime(string name)
    {
        if (name == "atktime") return atktime;
        else if (name == "hittime") return hittime;
        else return dashtime;
    }

    public void Settime(string name, float time)
    {
        if (name == "atktime") atktime = time;
        else if (name == "hittime") hittime = time;
        else dashtime = time;

    }

    public int Getsize()
    {
        return size;
    }

    public bool CalCoolDown(float time, float cooldown)
    {
        if (time >= cooldown)
        {
            return false;
        }

        return true;
    }


    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monster"))
        {
            if (!Gethit()) // 피격이 가능한 상태라면
            {
                // 데미지 들어옴
                Settime("hittime", 0);
                Sethit(true);
                // 피격 후 무적을 위한 타임 체크 ( 피격 후 곧바로 피격되지는 않는다. )
                print(col.GetComponent<M_info>().GetbodyDmg());
            }
        }

        if (col.CompareTag("Monster_attack_judgement"))
        {
            if (!Gethit()) // 피격이 가능한 상태라면
            {
                // 데미지 들어옴
                Settime("hittime", 0);
                Sethit(true);
                // 피격 후 무적을 위한 타임 체크 ( 피격 후 곧바로 피격되지는 않는다. )
                //print(col.GetComponent<Ef_balrog>().GetDmg());
            }
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Portal")) // 포탈과 충돌하면
        {
            Setportal(true);
        }
        if (col.CompareTag("Ground")) // 땅과 닿아있다면
        {
            Setiswall(false);
            Setisground(true);
            col2D.isTrigger = false;
        }
        if (col.CompareTag("Wall")) // 벽과 충돌하면
        {
            Setiswall(true);
            col2D.isTrigger = false;
        }
        if (col.CompareTag("Monster"))
        {
            if (!Gethit())
            {
                Settime("hittime", 0);
                Sethit(true);
            }
        }
        if (col.CompareTag("Monster_attack_judgement"))
        {
            if (!Gethit()) // 피격이 가능한 상태라면
            {
                // 데미지 들어옴
                Settime("hittime", 0);
                Sethit(true);
                // 피격 후 무적을 위한 타임 체크 ( 피격 후 곧바로 피격되지는 않는다. )
                print(col.GetComponent<Ef_balrog>().GetDmg());
            }
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Monster"))
        {
            col.GetComponent<BoxCollider2D>().isTrigger = false;
            col.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        dir = 1;
        cnt = 0;

        jumpcnt = 1; // 기본적으로 대부분의 캐릭터의 경우 이단점프 불가. 아이템이나 캐릭터 추가시 변경될 부분.

        hitdelay = 2.0f; // 플레이어가 피격당한 후 일정시간 동안에는 피격불가 상태가 됨.

        atktime = 0;
        dashtime = 0;

        movespeed = 0.1f;
        jumppower = 0.4f;
        dashtime = 0;

        attack_range = 5;
        maxHP = 50;
        nowHP = 50;
        atkdmg = 30;

        this.transform.localScale = new Vector3(size * dir, size, size);
        this.transform.position = new Vector3(2, 0, 0);

        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //nowHPbar.fillAmount = (float)nowHP / (float)maxHP;

        
        //print(Mathf.Round(accumulTime));

        // 캐릭터의 현재 좌표
        plax = this.transform.position.x;
        play = this.transform.position.y;
        plaz = this.transform.position.z;

        dashtime += Time.deltaTime;
        atktime += Time.deltaTime;
        hittime += Time.deltaTime;

        if (Gethit()) hit = CalCoolDown(hittime, hitdelay);

    }
}