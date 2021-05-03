using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalrogAttackEffect : MonsterAttackEffect
{

    public void AttackJudgementCreate()
    {
        if (!judgement)
        {
            // �������� ����
            if (GetComponent<MonsterAttack>().AttackRandom == 0)
            {
                // x dir ��ŭ 1 �����ϰ� dir ��ŭ 0~1
                // y 0.029 ���ϰ� 0~1
                // ���׿�
                Instantiate(prefabJudgement[0], new Vector3(transform.position.x - (1.5f * GetComponent<MonsterAttack>().Direction), transform.position.y + 0.7f, transform.position.z), Quaternion.identity);
                Instantiate(prefabJudgement[0], new Vector3(transform.position.x - (1.9f * GetComponent<MonsterAttack>().Direction), transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
                Instantiate(prefabJudgement[0], new Vector3(transform.position.x - (3.1f * GetComponent<MonsterAttack>().Direction), transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
            }
            else
            {
                // ������
                // �����̸� -1 , -0.4
                // �������̸� 1 , -0.4
                Instantiate(prefabJudgement[1], new Vector3(transform.position.x - (1.0f * GetComponent<MonsterAttack>().Direction), transform.position.y - 0.8f, transform.position.z), Quaternion.identity);
                Instantiate(prefabJudgement[1], new Vector3(transform.position.x - (1.5f * GetComponent<MonsterAttack>().Direction), transform.position.y, transform.position.z), Quaternion.identity);
            }

            judgement = true;
        }
    }
    public void AttackEffectCreate()
    {
        // ����Ʈ ����
        if (!effect) // ���� ���� ����Ʈ�� ���ٸ� ����
        {
            Vector3 createPositionPoint;

            if (GetComponent<MonsterAttack>().AttackRandom == 0) createPositionPoint = new Vector3(transform.position.x - (1.5f * GetComponent<MonsterAttack>().Direction), transform.position.y - 0.4f, transform.position.z);
            else createPositionPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Instantiate(prefabEffect[GetComponent<MonsterAttack>().AttackRandom], createPositionPoint, Quaternion.identity);
            effect = true;
            AttackJudgementCreate();
        }
    }
    public void AttackMotion()
    {
        // �ִϸ��̼� ���
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
