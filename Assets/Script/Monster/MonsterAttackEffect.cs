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

    public void TriggerOn()
    {
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void TriggerOff()
    {
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void MotionFalse() { motion = false; }

    public void RestoreMonsterTag() { gameObject.tag = "Monster"; }

    protected void VerifyQualification()
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
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
