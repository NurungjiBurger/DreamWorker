using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_info : MonoBehaviour
{

    // 캐릭터 스탯 & 상태
    private int maxHP;
    private int nowHP;
    private static int size = 2;

    
    private int cnt;
    private int jumpcnt;
    public int dir;
    public bool portal = false;
    public bool inputUp = false;
    public int atkDmg;
    public bool attacked = false;  // 캐릭터가 공격중인가?
    public bool gonext = false;
    public int attack_range;

    // 오브젝트
    [SerializeField]
    private GameObject objcharacter;
    public Animator animator;

    // 캐릭터 위치 및 사출기 위치
    public Transform obj;
    private Rigidbody2D rigid2D;
    private BoxCollider2D col2D;
    public float plax, play, plaz;
    public float shotx, shoty, shotz;

    
    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
    }
   
    public float timerOn()
    {
        return 0;
    }
    public bool CalCoolDown(float time, float cooldown)
    {
        if (time >= cooldown)
        {
            return false;
        }

        return true;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        dir = 1;
        cnt = 0;

        jumpcnt = 1; // 기본적으로 대부분의 캐릭터의 경우 이단점프 불가. 아이템이나 캐릭터 추가시 변경될 부분.


        
        attack_range = 5;
        maxHP = 50;
        nowHP = 50;
        atkDmg = 30;

        objcharacter.transform.localScale = new Vector3(size * dir, size, size);
        objcharacter.transform.position = new Vector3(2, 0, 0);

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
        plax = objcharacter.transform.position.x;
        play = objcharacter.transform.position.y;
        plaz = objcharacter.transform.position.z;

        





       

       
    }
}