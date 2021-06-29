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

    private GameObject inspector;
    private GameObject weapon;

    private GameData data;

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
        int size = inspector.GetComponent<Inspector>().EquipItemList.Count;
        weapon = null;
        for(int i=0;i<size;i++)
        {
            if (inspector.GetComponent<Inspector>().EquipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == "Weapon")
            {
                weapon = inspector.GetComponent<Inspector>().EquipItemList[i].SlotItem;
                break;
            }
        }

        if (weapon) Instantiate(weapon.GetComponent<ItemStatus>().GetAttackAnimation(), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        else Debug.Log("무기없음");
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();        
        attackTimer = Instantiate(prefabTimer).GetComponent<Timer>();
    }

    private void KeyInput()
    {
        attackSpeed = GetComponent<PlayerStatus>().Data.attackSpeed;

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
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (GetComponent<PlayerStatus>().Data != null)
        {
            attackSpeed = GetComponent<PlayerStatus>().Data.attackSpeed;
            attackTimer.SetCooldown(attackSpeed);
        }
    }
    
}
