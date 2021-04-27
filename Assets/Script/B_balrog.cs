using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_balrog : MonoBehaviour
{
    private M_info info;
    private P_info player;
    private BoxCollider2D col2D;
    private Rigidbody2D rigid2D;

    [SerializeField]
    private GameObject[] prf_attack_effect;
    [SerializeField]
    private GameObject[] prf_attack_judgement;

    private float mx, my, mz;


    void attack_judgement_summon()
    {
        // 실제 공격판정이 들어가는 이펙트 애니메이션 재생
        if (info.Getrandom("atkrandom") == 0)
        {
            // x dir 만큼 1 변경하고 dir 만큼 0~1
            // y 0.029 더하고 0~1
            // 메테오
            //Instantiate(prf_attack_judgement[0], new Vector3(-(mx + (-2 * -info.Getdir()) + (-Random.Range(1.0f, 2.0f) * -info.Getdir())), my + 0.03f + Random.Range(0.0f, 1.0f), mz), Quaternion.identity);
        }
        else
        {
            // 할퀴기
            // 왼쪽이면 -1 , -0.4
            // 오른쪽이면 1 , -0.4
            Instantiate(prf_attack_judgement[1], new Vector3(mx - ( 1.5f * info.Getdir() ) , my - 0.8f, mz), Quaternion.identity);
        }

        info.Setjudgement(true);
    }

    void attack_effect()
    {
        // print("now" + mx + " , " + my + " , " + mz);
        // 공격 이펙트가 진행되면서 공격 판정 소환물 생성
        if (!info.Getjudgement())
        {
            if (info.Getrandom("atkrandom") == 0)
            {
                info.animator.SetTrigger("attack");
                //Instantiate(prf_attack_effect[0], new Vector3(-(mx + (-2 * -info.Getdir())), my - 0.25f, mz), Quaternion.identity);
            }
            else
            {
                //왼쪽 2
                // x: -1
                // y: -0.25
                info.animator.SetTrigger("attack1");
                Instantiate(prf_attack_effect[1], new Vector3(mx , my , mz), Quaternion.identity);              
            }

            info.Seteffect(true);

            attack_judgement_summon();
        }

    }

    void attackmotion()
    {
        // 공격 모션이 시작되면서 공격 이펙트 생성
        if (!info.Geteffect()) attack_effect();

        if (info.Gettime("atktime") >= info.GetatkSpd()) info.Setattacked(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<M_info>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (info.Getattacked())
        {
            attackmotion();
        }
        else
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) <= (float)info.Getattackrange()) // 플레이어와의 거리가 공격 사거리보다 짧은 경우
            {
                // 공격사거리 내에 있는 경우 이므로 공격 실행
                if (!info.Getattacked() && rigid2D.velocity.y == 0) // 공격은 바닥에 붙어있을때만 가능
                {
                    if (player.transform.position.x > mx)
                    {
                        info.Setdir(-1);
                        this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    }
                    else
                    {
                        info.Setdir(1);
                        this.transform.localScale = new Vector3(info.Getsize(), info.Getsize(), info.Getsize());
                    }

                    info.animator.SetBool("move", false);
                    info.Setatkdone(false);
                    info.Setrandom("atkrandom", 1, 2);
                    info.Setattacked(true);
                    info.Settime("atktime", 0);
                    info.Setisground(true);

                    
                    
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
                    info.Setdir(-1);
                    this.transform.Translate(info.Getdir() * 0.01f, 0, 0);
                }
                else if (player.transform.position.x + 0.5 > this.transform.position.x) // 몬스터가 플레이어보다 왼쪽
                {
                    this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    info.Setdir(1);
                    this.transform.Translate(info.Getdir() * 0.01f, 0, 0);
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
                    info.Setdir(-1);
                    this.transform.Translate(info.Getdir() * 0.01f, 0, 0);
                }
                else if (info.Getrandom("mvrandom") == 1) // 좌로 이동 
                {
                    this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    info.Setdir(1);
                    this.transform.Translate(info.Getdir() * 0.01f, 0, 0);
                }
                else info.animator.SetBool("move", false); // stand 애니메이션 재생
            }
        }
    }

    void Update()
    {
        mx = this.transform.position.x;
        my = this.transform.position.y;
        mz = this.transform.position.z;
    }
}
