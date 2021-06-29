using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 3.5 // 5.08
public class VonleonAttackEffect : MonsterAttackEffect
{

    public void TeleportToPlayer()
    {
        Debug.Log("�������̾�");
        transform.position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        GetComponent<Animator>().SetBool("disappear", false);
    }

    public void AttackJudgementCreate()
    {
        if (!judgement)
        {
            // �������� ����
            //if (GetComponent<MonsterSensor>().Player) Instantiate(prefabJudgement[GetComponent<MonsterAttack>().AttackRandom], GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, Quaternion.identity);

            judgement = true;
        }
    }
    private void AttackMotion()
    {
        // �ִϸ��̼� ���
        if (!motion) // ���� ����Ʈ�� ���ٸ� ����
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
