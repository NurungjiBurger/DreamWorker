using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_attack : MonoBehaviour
{
    
    private P_info info;
    private Rigidbody2D rigid2D;
    private BoxCollider2D col2D;

    [SerializeField]
    private GameObject prefab;

    void AttackAnimatestart()
    {
        info.Setp_position();
        Instantiate(prefab, new Vector3(info.Getp_position("plax"), info.Getp_position("play"), info.Getp_position("plaz")), Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<P_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();

        info.Settime("atktime", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (info.Getuseattack())
        {
            info.Setuseattack(info.CalCoolDown(info.Gettime("atktime"), info.Getatkspd()));
        }

        // 캐릭터 공격
        if (Input.GetKey(KeyCode.Q) && !info.Getuseattack())//!animator.GetCurrentAnimatorStateInfo(0).IsName("oz_attack"))
        {
            info.Setuseattack(true);
            info.Settime("atktime", 0);
            info.animator.SetTrigger("attack");
            // 오비탈 생성
            AttackAnimatestart();

        }
    }
}
