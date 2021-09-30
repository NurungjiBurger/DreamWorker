using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private int attackQuantity;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private GameObject prefabAttackEffect;
    [SerializeField]
    private float[] attackEffectOffset;
    [SerializeField]
    protected float sizeX;
    [SerializeField]
    protected float sizeY;

    private bool isAttack = false;
    private int attackRandom;

    private int dir;

    private GameObject player;

    private GameObject effect;
    private Animator animator;

    private Timer attackTimer;

    
    public bool Attack { get { return isAttack; } }
    public float Range {  get { return attackRange; } }
    public int Quantity { get { return attackQuantity; } }
    public bool CoolDownCheck { get { return attackTimer.CooldownCheck(); } }
    public int AttackRandom { get { return attackRandom; } }
    public int Direction { get { return dir; } }
    public void DestroyAll() { attackTimer.DestroyAll(); }

    public void RestoreCollidersize()
    {
        GetComponent<BoxCollider2D>().size = new Vector3(sizeX, sizeY, 0);
    }
    public void ModifyColliderSize()
    {
        GetComponent<BoxCollider2D>().size = new Vector3(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y, GetComponent<SpriteRenderer>().bounds.size.z);
    }
    //public void AnimaotrSetFalse() { animator.SetBool("move", false); }

    public void AttackEffectCreate()
    {
        if (effect == null && GetComponent<MonsterStatus>().Bone)
        {
            effect = Instantiate(prefabAttackEffect, GetComponent<MonsterStatus>().Bone.transform.position, Quaternion.identity);
            if (GetComponent<MonsterStatus>().Boss) effect.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            effect.GetComponent<MonsterEffectSensor>().dir = dir;
            if (effect.GetComponent<Projectile>()) effect.GetComponent<Projectile>().EntitySetting(gameObject);
        }
    }

    private void Attacking()
    {
        if (isAttack )
        {
            /*if (!GetComponent<MonsterMovement>().Animaing)*/ animator.SetTrigger("attack");
            //GetComponent<MonsterMovement>().Animaing = true;
            attackTimer.TimerSetZero();
        }
    }

    private void DecideAttack()
    {
        // 공격범위내에 있다면
        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            if (!isAttack)
            {
                isAttack = true;
                if (GetComponent<ObjectFlip>().flipX) dir = -1;
                else dir = 1;

                attackRandom = Random.Range(0, attackQuantity);

                Attacking();
            }
        }
    }

    private void Setting()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        attackTimer = Instantiate(prefabTimer).GetComponent<Timer>();
        attackTimer.SetCooldown(attackSpeed);
    }

    private void Start()
    {
        Setting();
    }

    private void Update()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            DecideAttack();
            if (attackTimer.CooldownCheck()) isAttack = false;
        }

    }

    private void FixedUpdate()
    {

    }
}
