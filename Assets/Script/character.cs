using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class character : MonoBehaviour
{

    // 캐릭터 스탯 & 상태
    int maxHP;  
    int nowHP;
    static int size = 2;
    public int atkDmg; 
    public bool attacked = false;  // 캐릭터가 공격중인가?
    public bool gonext = false;
    public int attack_range;   
    float moveSpeed;    
    float jumpPower;
    public bool inputUp = false;
    bool inputRight = false;    
    bool inputLeft = false;
    public bool portal = false;
    bool collider = false;
    bool ground = false;
    bool inputJump = false; 
    bool inputDash = false; 
    bool detectwall = false;
    public int flag;
    public int dir;
    int cnt;
    int jumpcnt;

    float DashTime;
    bool useDash = false;
    float AttackTime;
    bool useAttack = false;
    float atkSpeed = 0.5f; // 캐릭터의 공격 속도 ( 공격 사이의 간격시간 )

    public GameObject prefab;
    public GameObject objcharacter;
    BoxCollider2D col2D;

    // 캐릭터 위치 및 사출기 위치
    Rigidbody2D rigid2D;
    public float plax, play, plaz;
    public float shotx, shoty, shotz;

   
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Portal")) // 포탈과 충돌하면
        {
            portal = true;
        }
        if (col.CompareTag("Ground")) // 땅과 닿아있다면
        {
            detectwall = false;
            ground = true;
        }
        if (col.CompareTag("Wall")) // 벽과 충돌하면
        {
            detectwall = true;
            col2D.isTrigger = false;
        }
    }
    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
    }
    void AttackAnimatestart()
    {
        shotx = plax;
        shoty = play;
        shotz = plaz;
        Instantiate(prefab, new Vector3(plax, play, plaz), Quaternion.identity);
    }
    float timerOn()
    {
        return 0;
    }
    bool CalCoolDown(float time, float cooldown)
    {
        if (time >= cooldown)
        {
           return  false;
        }

        return true;
    }

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        flag = 0;
        dir = 1;
        cnt = 0;
        moveSpeed = 0.1f;
        jumpPower = 0.4f;
        jumpcnt = 1; // 기본적으로 대부분의 캐릭터의 경우 이단점프 불가. 아이템이나 캐릭터 추가시 변경될 부분.

        DashTime = 0;
        AttackTime = 0;
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
        if (!detectwall || ground)
        {
            if (inputRight)
            {
                inputRight = false;
                rigid2D.AddForce(Vector2.right * moveSpeed);
            }
            if (inputLeft)
            {
                inputLeft = false;
                rigid2D.AddForce(Vector2.left * moveSpeed);
            }
        }
        if (inputJump)
        {
            inputJump = false;
            rigid2D.AddForce(Vector2.up * jumpPower);
        }
        if (inputDash)
        {
            inputDash = false;
            objcharacter.transform.position = new Vector3(plax + (dir * -2), play, plaz);
            DashTime = timerOn();
            useDash = true;
        }
        if(inputUp)
        {
            if(portal && gonext)
            {
                portal = false;
                gonext = false;
            }
            inputUp = false;
        }
        // 속도 제한
        if (rigid2D.velocity.x >= 2.5f) rigid2D.velocity = new Vector2(2.5f, rigid2D.velocity.y);
        else if (rigid2D.velocity.x <= -2.5f) rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        //nowHPbar.fillAmount = (float)nowHP / (float)maxHP;
        DashTime += Time.deltaTime;
        AttackTime += Time.deltaTime;
        //print(Mathf.Round(accumulTime));

        // 캐릭터의 현재 좌표
        plax = objcharacter.transform.position.x;
        play = objcharacter.transform.position.y;
        plaz = objcharacter.transform.position.z;

        // 땅 밟기
        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null) animator.SetBool("move", false);
        else animator.SetBool("move", true);

        // 캐릭터 이동 ( 좌우 )
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = -1;
            inputRight = true;
            objcharacter.transform.localScale = new Vector3(size * dir, size, size);
            animator.SetBool("move", true);
            col2D.isTrigger = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = 1;
            inputLeft = true;
            objcharacter.transform.localScale = new Vector3(size * dir, size, size);
            animator.SetBool("move", true);
            col2D.isTrigger = true;
        }
        else animator.SetBool("move", false);

        // 캐릭터 이동 ( 점프 )
        if (Input.GetKey(KeyCode.D) && ground )//rigid2D.velocity.y == 0) // 점프입력이 들어왔을때
        {
            // if (jumpcnt != 0) // 점프 횟수가 남아 있다면
            // jumpcnt--;
            inputJump = true;
            ground = false;
            objcharacter.transform.localScale = new Vector3(size * dir, size, size);
            animator.SetBool("move", true);
            col2D.isTrigger = true;
            
            
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputUp = true;
        }


        if (rigid2D.velocity.y < 0)
        {
            col2D.isTrigger = false;
        }

        

        // 캐릭터 회피 ( 대쉬 )
        if (Input.GetKey(KeyCode.Space) && !useDash )
        {
            inputDash = true;
        }

        // 대쉬 쿨다운 체크
        if (useDash)
        {
            useDash = CalCoolDown(DashTime, 5.0f);
        }

        if (useAttack)
        {
            useAttack = CalCoolDown(AttackTime, atkSpeed);
        }

        // 캐릭터 공격
        if (Input.GetKey(KeyCode.Q) && !useAttack)//!animator.GetCurrentAnimatorStateInfo(0).IsName("oz_attack"))
        {
            useAttack = true;
            AttackTime = timerOn();
            animator.SetTrigger("attack");
            // 오비탈 생성
            AttackAnimatestart();
            useAttack = CalCoolDown(AttackTime, atkSpeed);


        }
    }
}
