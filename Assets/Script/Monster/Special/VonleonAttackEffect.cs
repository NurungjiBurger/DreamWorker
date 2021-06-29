using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 3.5 // 5.08
public class VonleonAttackEffect : MonsterAttackEffect
{

    public void TeleportToPlayer()
    {
        Debug.Log("실행중이야");
        transform.position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        GetComponent<Animator>().SetBool("disappear", false);
    }

    public void AttackJudgementCreate()
    {
        if (!judgement)
        {
            // 공격판정 생성
            //if (GetComponent<MonsterSensor>().Player) Instantiate(prefabJudgement[GetComponent<MonsterAttack>().AttackRandom], GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, Quaternion.identity);

            judgement = true;
        }
    }
    private void AttackMotion()
    {
        // 애니메이션 재생
        if (!motion) // 공격 이펙트가 없다면 진행
        {
            motion = true;
            string value = "attack";
            value = value + GetComponent<MonsterAttack>().AttackRandom.ToString();

            if (GetComponent<MonsterAttack>().AttackRandom == 3) GetComponent<Animator>().SetBool("disappear", true);
            else GetComponent<Animator>().SetTrigger(value);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        VerifyQualification();

        if (GetComponent<MonsterAttack>().Attack)
        {
            AttackMotion();
        }
    }
}
