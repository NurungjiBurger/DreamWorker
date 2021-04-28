using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_move : MonoBehaviour
{
    private GameObject player;

    private float mx, my, mz;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            if (Vector3.Distance(player.transform.position, this.transform.position) <= (float)info.Getattackrange()) // 플레이어와의 거리가 공격 사거리보다 짧은 경우
            {
                // 공격사거리 내에 있는 경우 이므로 공격 실행
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
                            info.Setatkposition("atkx", -2.0f);
                            info.Setdir(-0.01f);
                        }
                        else
                        {
                            this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                            info.Setatkposition("atkx", 2.0f);
                            info.Setdir(0.01f);
                        }
                    }
                    else info.Setatkposition("atky", 2.0f);

                }
            }
            else if (Vector3.Distance(player.transform.position, this.transform.position) <= (float)info.Getrecogrange()) // 플레이어와의 거리가 공격 사거리보다 길고 인식 사거리보다 짧은 경우
            {
                // 공격사거리에 포함되지 않으므로 공격을 할 수는 없음
                // 인식사거리에 포함되므로 몬스터가 공격을 위해 점점 다가옴
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
            }
            else    // 플레이어와의 거리가 인식사거리도 벗어난 경우
            {
                // 완벽한 사거리 밖이므로 몬스터는 랜덤으로 행동
                // 사거리 밖에 플레이어가 있을 경우 몬스터가 할 수 있는 행동 3가지로 분류
                if (info.Gettime("mvtime") >= info.GetatkSpd())
                {
                    info.Settime("mvtime", 0);
                    info.Setrandom("mvrandom", 0, 3);
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
