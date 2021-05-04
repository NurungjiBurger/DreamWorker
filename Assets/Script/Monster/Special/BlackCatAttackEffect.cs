using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCatAttackEffect : MonsterAttackEffect
{
    private void AttackJudgementCreate()
    {
        if (!judgement)
        {
            // �������� ����
            Instantiate(prefabJudgement[0], new Vector3(transform.position.x - (0.5f * GetComponent<MonsterAttack>().Direction), transform.position.y - 0.01f, transform.position.z), Quaternion.identity);

            judgement = true;
        }
    }
    private void AttackEffectCreate()
    {
        // ����Ʈ ����
        if (!effect) // ���� ���� ����Ʈ�� ���ٸ� ����
        {
            AttackJudgementCreate();
        }
    }

    private void AttackMotion()
    {
        if (!motion) // ���� ����Ʈ�� ���ٸ� ����
        {
            motion = true;
            string value = "attack";
            value = value + GetComponent<MonsterAttack>().AttackRandom.ToString();

            GetComponent<Animator>().SetTrigger(value);
            AttackEffectCreate();
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
