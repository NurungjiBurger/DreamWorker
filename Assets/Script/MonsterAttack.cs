using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField]
    private float attackRange = 1.2f;
    [SerializeField]
    private int attackQuantity = 1;
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private float attackSpeed = 3;
    [SerializeField]
    private int attackType = 0;

    private bool isAttack = false;
    private int attackRandom;

    private int dir;

    private GameObject player;
    private Animator animator;

    private Timer attackTimer;

    public float Range
    {
        get
        {
            return attackRange;
        }
        set
        {
            Debug.Log("���� ������ �� �����ϴ�.");
        }
    }

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
            if (isAttack = !attackTimer.CooldownCheck())
            {
                isAttack = true;
                if (attackType == 0) transform.Translate(0, 1.5f, 0);
                if (GetComponent<SpriteRenderer>().flipX) dir = -1;
                else dir = 1;
            }
        }

    }

    private void FixedUpdate()
    {
        if(isAttack)
        {
            if (attackType == 0) // �������� 
            {
                transform.Translate(-0.1f * dir, -0.1f, 0);
            }
            else if (attackType == 1) // ��������
            {
                transform.Translate(-0.1f, 0, 0);
            }
        }
    }
}
