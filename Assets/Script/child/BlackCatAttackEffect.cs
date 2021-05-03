using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCatAttackEffect : MonsterAttackEffect
{
    public void AttackJudgementCreate()
    {
        if (!judgement)
        {
            // 공격판정 생성
            Instantiate(prefabJudgement[0], new Vector3(transform.position.x - (0.5f * GetComponent<MonsterAttack>().Direction), transform.position.y - 0.01f, transform.position.z), Quaternion.identity);

            judgement = true;
        }
    }
    public void AttackEffectCreate()
    {
        // 이펙트 생성
        if (!effect) // 공격 판정 이펙트가 없다면 진행
        {
            AttackJudgementCreate();
        }
    }

    private void AttackMotion()
    {
        if (!motion) // 공격 이펙트가 없다면 진행
        {
            motion = true;
            string value = "attack";
            value = value + GetComponent<MonsterAttack>().AttackRandom.ToString();

            GetComponent<Animator>().SetTrigger(value);
            AttackEffectCreate();
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
