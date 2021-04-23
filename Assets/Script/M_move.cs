using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_move : MonoBehaviour
{
    private M_info info;
    private M_attack atk;
    private P_info player;
    private BoxCollider2D col2D;
    private Rigidbody2D rigid2D;

    private float mx, my, mz;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<M_info>();
        atk = GetComponent<M_attack>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();

    }

    private void FixedUpdate()
    {
        if (info.Getattacked())
        {
            info.animator.SetTrigger("attack");
            atk.Attackmotion();
        }
        else
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) <= (float)info.Getrecogrange()) // 몬스터의 사거리 안에서
            {
                if (player.transform.position.x - 0.5 < this.transform.position.x) // 몬스터가 플레이어보다 오른쪽
                {
                    this.transform.localScale = new Vector3(info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    this.transform.Translate(-0.01f, 0, 0);
                }
                else if (player.transform.position.x + 0.5 > this.transform.position.x) // 몬스터가 플레이어보다 왼쪽
                {
                    this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    this.transform.Translate(0.01f, 0, 0);
                }
                if (Mathf.Abs(player.transform.position.x - this.transform.position.x) <= 1.5) // 몬스터와 플레이어의 위치가 같아지면 ( 리치가 닿는다면 )
                {
                    if (!info.Getattacked() && rigid2D.velocity.y == 0) // 공격은 바닥에 붙어있을때만 가능
                    {
                        info.Setatkdone(false);
                        col2D.isTrigger = true;
                        rigid2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                        info.animator.SetTrigger("attack");
                        info.Setattacked(true);
                        info.Settime("atktime", 0);
                        info.Setisground(true);
                        if (info.Getatktype() == 1)
                        {
                            if (player.transform.position.x < mx)
                            {
                                this.transform.localScale = new Vector3(info.Getsize(), info.Getsize(), info.Getsize());
                                info.Setatkposition("atkx", -1.5f);
                                info.Setdir(-0.01f);
                            }
                            else
                            {
                                this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                                info.Setatkposition("atkx", 1.5f);
                                info.Setdir(0.01f);
                            }
                        }
                        else info.Setatkposition("atky", 1.5f);

                    }
                }
            }
            else
            {
                if (info.Gettime("mvtime") >= info.GetatkSpd())
                {
                    info.Settime("mvtime", 0);
                    info.Setrandom("mvrandom", 0, 3);    // 플레이어가 사거리 밖에 있을때 취할수 있는 행동 3가지
                }
                if (info.Getrandom("mvrandom") == 0) // 우로 이동
                {
                    this.transform.localScale = new Vector3(info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    this.transform.Translate(-0.01f, 0, 0);
                }
                else if (info.Getrandom("mvrandom") == 1) // 좌로 이동 
                {
                    this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    this.transform.Translate(0.01f, 0, 0);
                }
                else info.animator.SetBool("move", false); // stand 애니메이션 재생

            }
        }
        // 속도 제한
        if (rigid2D.velocity.x >= 1.5f) rigid2D.velocity = new Vector2(1.5f, rigid2D.velocity.y);
        else if (rigid2D.velocity.x <= -1.5f) rigid2D.velocity = new Vector2(-1.5f, rigid2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        mx = this.transform.position.x;
        my = this.transform.position.y;
        mz = this.transform.position.z;
    }
}
