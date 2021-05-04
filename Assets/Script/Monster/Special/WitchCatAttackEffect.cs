using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCatAttackEffect : MonsterAttackEffect
{
    public void AttackJudgementCreate()
    {
        if (!judgement)
        {

        }
    }
    public void AttackEffectCreate()
    {
        // ����Ʈ ����
        if (!effect) // ���� ���� ����Ʈ�� ���ٸ� ����
        {
            Instantiate(prefabEffect[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

            effect = true;
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
