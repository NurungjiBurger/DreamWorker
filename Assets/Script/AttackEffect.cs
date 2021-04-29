using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabEffect;
    [SerializeField]
    private GameObject[] prefabJudgement;


    private bool effect = false;
    private bool judgement = false;

    public bool Effect { get { return effect; } }
    public bool Judgement { get { return judgement; } }

    public void AttackJudgementCreate()
    {
        // 공격판정 생성
        if (GetComponent<MonsterAttack>().AttackRandom == 0)
        {
            // x dir 만큼 1 변경하고 dir 만큼 0~1
            // y 0.029 더하고 0~1
            // 메테오
            Instantiate(prefabJudgement[0], new Vector3(transform.position.x - (1.5f * GetComponent<MonsterAttack>().Direction), transform.position.y + 0.7f, transform.position.z), Quaternion.identity);
            Instantiate(prefabJudgement[0], new Vector3(transform.position.x - (1.9f * GetComponent<MonsterAttack>().Direction), transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
            Instantiate(prefabJudgement[0], new Vector3(transform.position.x - (3.1f * GetComponent<MonsterAttack>().Direction), transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
        }
        else
        {
            // 할퀴기
            // 왼쪽이면 -1 , -0.4
            // 오른쪽이면 1 , -0.4
            Instantiate(prefabJudgement[1], new Vector3(transform.position.x - (1.0f * GetComponent<MonsterAttack>().Direction), transform.position.y - 0.8f, transform.position.z), Quaternion.identity);
            Instantiate(prefabJudgement[1], new Vector3(transform.position.x - (1.5f * GetComponent<MonsterAttack>().Direction), transform.position.y, transform.position.z), Quaternion.identity);
        }

        judgement = true;
    }
    public void AttackEffectCreate()
    {
        // 이펙트 생성
        if (!judgement) // 공격 판정 이펙트가 없다면 진행
        {
            Vector3 createPositionPoint;

            if (GetComponent<MonsterAttack>().AttackRandom == 0) createPositionPoint = new Vector3(transform.position.x - (1.5f * GetComponent<MonsterAttack>().Direction), transform.position.y - 0.4f, transform.position.z);
            else createPositionPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Instantiate(prefabEffect[GetComponent<MonsterAttack>().AttackRandom], createPositionPoint, Quaternion.identity);
            AttackJudgementCreate();
        }
    }
    public void AttackMotion()
    {
        // 애니메이션 재생
        if (!effect) // 공격 이펙트가 없다면 진행
        {
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
        if (GameObject.FindGameObjectsWithTag("Monster_attack_effect").Length == 0) effect = false;
        if (GameObject.FindGameObjectsWithTag("Monster_attack_judgement").Length == 0) judgement = false;

        if (GetComponent<MonsterAttack>().CoolDownCheck)
        {
            effect = false;
            judgement = false;
        }

        //GetComponent<MonsterAttack>().VerifyAttack();
    }
}
