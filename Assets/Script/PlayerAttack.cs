using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private GameObject prefabTimer;

    private Animator animator;

    private Timer attackTimer;

    private bool isAttack = false;

    private float coolDown = 0.5f;

    private void AttackAnimatestart()
    {
        Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();        
        attackTimer = Instantiate(prefabTimer).GetComponent<Timer>();

        attackTimer.SetCooldown(coolDown);
    }

    private void FixedUpdate()
    {
        if (isAttack)
        {
            isAttack = false;
            attackTimer.TimerSetZero();
            animator.SetTrigger("attack");
            // 오비탈 생성
            AttackAnimatestart();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // 캐릭터 공격
        if (Input.GetKey(KeyCode.Q) && attackTimer.CooldownCheck())
        {
            isAttack = true;
        }
    }
    
}
