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
        GameObject.Find("Canvas").transform.Find("AttackButton").GetComponent<ButtonUI>().UIActive();
    }

    private void WeaponEnroll()
    {
        int size = inspector.GetComponent<Inspector>().EquipItemList.Count;
        weapon = null;
        for (int i = 0; i < size; i++)
        {
            if (inspector.GetComponent<Inspector>().EquipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == "Weapon")
            {
                weapon = inspector.GetComponent<Inspector>().EquipItemList[i].SlotItem;
                break;
            }
        }
    }

    private void AttackAnimatestart()
    {
        GameObject tmp;

        if (weapon)
        {

            if (weapon.GetComponent<ItemStatus>().EffectBone != null)
            {
                tmp = Instantiate(weapon.GetComponent<ItemStatus>().AttackEffect, weapon.GetComponent<ItemStatus>().EffectBone.transform.position, Quaternion.identity);
            }
            else tmp = Instantiate(weapon.GetComponent<ItemStatus>().AttackEffect, weapon.GetComponent<ItemStatus>().EffectBone.transform.position, Quaternion.identity);

            if (tmp.GetComponent<Projectile>())
            {
                tmp.GetComponent<Projectile>().EntitySetting(gameObject);
                tmp.GetComponent<Projectile>().WeaponSetting(weapon);
            }
        }
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
            if (GameObject.Find("Canvas").transform.Find("AttackButton").GetComponent<ButtonUI>().OnOff || Input.GetKey(KeyCode.Q))
            {
                Attacking();
            }
        }
    }

    private void Attacking()
    {
        if (!isAttack)
        {
            isAttack = true;

            WeaponEnroll();

            if (weapon)
            {

                GetComponent<Audio>().AudioPlay(1);

                animator.SetTrigger(weapon.GetComponent<ItemStatus>().AttackType);
                //animator.SetTrigger("attack");

                attackTimer.TimerSetZero();
            }
            else
            {
                Debug.Log("무기없음");
                isAttack = false;
            }
     //       AttackAnimatestart();
        }
    }

    private void FixedUpdate()
    {
        //if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause) Attacking();
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
