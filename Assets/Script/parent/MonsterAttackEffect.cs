using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackEffect : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] prefabEffect;
    [SerializeField]
    protected GameObject[] prefabJudgement;


    protected bool effect = false;
    protected bool judgement = false;
    protected bool motion = false;

    public bool Motion { get { return motion; } }
    public bool Effect { get { return effect; } }
    public bool Judgement { get { return judgement; } }

    public void MotionFalse() { motion = false; }

    public void VerifyQualification()
    {
        if (GameObject.FindGameObjectsWithTag("Monster_attack_effect").Length == 0) effect = false;
        if (GameObject.FindGameObjectsWithTag("Monster_attack_judgement").Length == 0) judgement = false;

        if (GetComponent<MonsterAttack>().CoolDownCheck)
        {
            effect = false;
            judgement = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
