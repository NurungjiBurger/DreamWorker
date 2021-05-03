using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VonleonAttackEffect : MonsterAttackEffect
{
    public void TeleportToPlayer()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        GetComponent<Animator>().SetBool("disappear", false);
    }

    public void AttackJudgementCreate()
    {
        if (!judgement)
        {
            // 공격판정 생성
            if (GetComponent<MonsterAttack>().AttackRandom == 0)
            {


            }
            else if (GetComponent<MonsterAttack>().AttackRandom == 1)
            {

            }
            else
            {

            }

            judgement = true;
        }
    }
    public void AttackMotion()
    {
        // 애니메이션 재생
        if (!motion) // 공격 이펙트가 없다면 진행
        {
            motion = true;
            string value = "attack";
            value = value + GetComponent<MonsterAttack>().AttackRandom.ToString();

            GetComponent<Animator>().SetTrigger(value);
            AttackJudgementCreate();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        VerifyQualification();

        if (GetComponent<MonsterAttack>().Attack)
        {
            AttackMotion();
        }
    }
}
