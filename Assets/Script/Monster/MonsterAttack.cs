using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private int attackQuantity;
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private int attackType;
    [SerializeField]
    protected float sizeX;
    [SerializeField]
    protected float sizeY;

    private bool isAttack = false;
    private int attackRandom;

    private bool isUp;
    private int dir;

    private GameObject player;
    private Animator animator;

    private Timer attackTimer;

    
    public bool Attack { get { return isAttack; } }
    public float Range {  get { return attackRange; } }
    public int Quantity { get { return attackQuantity; } }
    public bool CoolDownCheck { get { return attackTimer.CooldownCheck(); } }
    public int AttackRandom { get { return attackRandom; } }
    public int Direction { get { return dir; } }
    public void RestoreCollidersize()
    {
        GetComponent<BoxCollider2D>().size = new Vector3(sizeX, sizeY, 0);
    }
    public void ModifyColliderSize()
    {
        GetComponent<BoxCollider2D>().size = new Vector3(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y, GetComponent<SpriteRenderer>().bounds.size.z);
    }
    public void AnimaotrSetFalse() { animator.SetBool("move", false); }
    protected void DecideAttack()
    {       
        // 공격범위내에 있다면
        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            if (!isAttack)
            {
                isAttack = true;
                attackTimer.TimerSetZero();
                if (GetComponent<SpriteRenderer>().flipX) dir = -1;
                else dir = 1;
                if (attackQuantity == 1)
                {
                    if (attackType == 0) isUp = true;
                }
                else
                {
                     attackRandom = Random.Range(0, attackQuantity);
                }
            }
        }
    }
    protected void Attacking()
    {
        if (isAttack)
        {
            if (attackTimer.CooldownCheck()) isAttack = false;

            if (attackQuantity == 0)
            {
                if (isUp && transform.position.y >= 0.075f * GetComponent<MonsterStatus>().JumpPower) isUp = false;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                if (attackType == 0) // 점프공격 
                {
                    if (isUp) transform.Translate(0, 0.075f * GetComponent<MonsterStatus>().JumpPower / (float)(attackSpeed * 4), 0);
                    else transform.Translate(-0.01f * dir * GetComponent<MonsterStatus>().MoveSpeed, -(0.075f * GetComponent<MonsterStatus>().JumpPower) / (float)attackSpeed, 0);

                    if (GetComponent<MonsterSensor>().Ground) isAttack = false;
                }
                else if (attackType == 1) // 돌진공격
                {
                    transform.Translate(-0.005f * dir * GetComponent<MonsterStatus>().MoveSpeed, 0, 0);
                }
                else if (attackType == 2) // 본체 공격애니메이션에 데미지
                {
                    animator.SetTrigger("attack");
                    GetComponent<Collider2D>().isTrigger = true;
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                }
            }
            else
            {

                // 종료조건
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    protected void Setting()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        attackTimer = Instantiate(prefabTimer).GetComponent<Timer>();
        attackTimer.SetCooldown(attackSpeed);
    }

    // Start is called before the first frame update
    private void Start()
    {
        Setting();
    }

    // Update is called once per frame
    private void Update()
    {
        DecideAttack();
    }

    private void FixedUpdate()
    {
        Attacking();
    }
}
