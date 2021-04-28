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

    private bool isAttack = false;
    private int attackRandom;

    private bool isUp;
    private int dir;

    private GameObject player;
    private Animator animator;

    private Timer attackTimer;

    public bool Attack { get { return isAttack; } }

    public float Range {  get { return attackRange; } }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        attackTimer = Instantiate(prefabTimer).GetComponent<Timer>();
        attackTimer.SetCooldown(attackSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            if (!isAttack)
            {
                isAttack = true;
                attackTimer.TimerSetZero();
                if (attackType == 0) isUp = true;
                if (GetComponent<SpriteRenderer>().flipX) dir = -1;
                else dir = 1;
            }
        }

    }

    private void FixedUpdate()
    {
        if (isAttack)
        {
            if (attackTimer.CooldownCheck()) isAttack = false;

            if (isUp && transform.position.y >= 0.075f * GetComponent<MonsterStatus>().JumpPower) isUp = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            if (attackType == 0) // 점프공격 
            {
                if (isUp) transform.Translate(0, 0.075f * GetComponent<MonsterStatus>().JumpPower / (float)(attackSpeed * 4), 0);
                else transform.Translate(-0.01f * dir * GetComponent<MonsterStatus>().MoveSpeed, -(0.075f * GetComponent<MonsterStatus>().JumpPower) / (float)attackSpeed,  0);

                if (GetComponent<MonsterSensor>().Ground) isAttack = false;
            }
            else if (attackType == 1) // 돌진공격
            {
                transform.Translate(-0.005f * dir * GetComponent<MonsterStatus>().MoveSpeed, 0, 0);
            }
        }
        else
        {
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
