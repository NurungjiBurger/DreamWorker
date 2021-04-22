using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_attack : MonoBehaviour
{
    private float AttackTime;
    private bool useAttack = false;
    private float atkSpeed = 0.5f; // ĳ������ ���� �ӵ� ( ���� ������ ���ݽð� )

    private P_info info;
    private Rigidbody2D rigid2D;
    private BoxCollider2D col2D;

    [SerializeField]
    private GameObject prefab;

    void AttackAnimatestart()
    {
        info.shotx = info.plax;
        info.shoty = info.play;
        info.shotz = info.plaz;
        Instantiate(prefab, new Vector3(info.plax, info.play, info.plaz), Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        AttackTime = 0;

        info = GetComponent<P_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackTime += Time.deltaTime;

        if (useAttack)
        {
            useAttack = info.CalCoolDown(AttackTime, atkSpeed);
        }

        // ĳ���� ����
        if (Input.GetKey(KeyCode.Q) && !useAttack)//!animator.GetCurrentAnimatorStateInfo(0).IsName("oz_attack"))
        {
            useAttack = true;
            AttackTime = info.timerOn();
            info.animator.SetTrigger("attack");
            // ����Ż ����
            AttackAnimatestart();
            useAttack = info.CalCoolDown(AttackTime, atkSpeed);


        }
    }
}
