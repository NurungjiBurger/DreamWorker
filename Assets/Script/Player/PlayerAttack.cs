using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private GameObject prefab;
    private Animator animator;

    private GameObject weapon;

    private Timer attackTimer;
    private bool isAttack = false;

    private float attackSpeed;

    public bool IsAttack { get { return isAttack; } }
    public Timer AttackTimer { get { return attackTimer; } }

    public void IsAttackFalse()
    {
        isAttack = false;
    }

    private void AttackAnimatestart()
    {

        weapon = GameObject.Find("Canvas").transform.Find("Inspector(Clone)").transform.Find("Background").transform.Find("Weapon").GetChild(0).GetComponent<Slot>().SlotItem;

        // Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Instantiate(weapon.GetComponent<ItemStatus>().GetAttackAnimation(), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();        
        attackTimer = Instantiate(prefabTimer).GetComponent<Timer>();

        attackSpeed = GetComponent<PlayerStatus>().AttackSpeed;
        attackTimer.SetCooldown(attackSpeed);
    }

    private void KeyInput()
    {
        attackSpeed = GetComponent<PlayerStatus>().AttackSpeed;

        // 캐릭터 공격
        if (attackTimer.CooldownCheck())
        {
            if (Input.GetKey(KeyCode.Q) && attackTimer.CooldownCheck())
            {
                isAttack = true;
            }
        }
    }

    private void Attacking()
    {
        if (isAttack)
        {
            isAttack = false;
            animator.SetTrigger("attack");
            attackTimer.TimerSetZero();
            AttackAnimatestart();
        }
    }

    private void FixedUpdate()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause) Attacking();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause) KeyInput();
    }
    
}
